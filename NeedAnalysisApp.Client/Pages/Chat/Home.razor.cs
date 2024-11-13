namespace NeedAnalysisApp.Client.Pages.Chat;

public partial class Home
{
    #region Fields

    [Inject] public AuthenticationStateProvider PersistentAuthenticationStateProvider { get; set; } = null!;

    [Inject] public IUserClientService _userClientService { get; set; } = null!;

    [Inject] public IMessageClientService _messageClientService { get; set; } = null!;

    [Inject] public NavigationManager _navigationManager { get; set; } = null!;

    private HubConnection? _hubConnection;

    public UserDto ChatPerson { get; set; } = new();

    public UserDto CurrentPerson { get; set; } = new();

    public List<UserDto> Users { get; set; } = [];

    public Dictionary<string, object> parameters { get; set; } = new();

    private string SearchedUser = "";

    private List<UserChatDto> UserChats { get; set; } = [];

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = ConfigureHub();

        await _hubConnection.StartAsync();

        parameters["IsDefault"] = true; // Set default state

        var currentUser = await GetCurrentUser();

        CurrentPerson = currentUser;

        SetCurrentUserToTop(currentUser);

        await _hubConnection.SendAsync(nameof(IBlazingChatHubServer.SetUserOnline), currentUser);

        StateHasChanged();
    }

    private HubConnection ConfigureHub()
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/hubs/blazing-chat"))
            .Build();

        hubConnection.On<UserDto>(nameof(IBlazingChatHubClient.UserConnected), (newUser) =>
        {
            Users.Add(newUser);
            StateHasChanged();
        });

        hubConnection.On<ICollection<UserDto>>(nameof(IBlazingChatHubClient.OnlineUsersList), (onlineUsers) =>
        {
            foreach (var user in Users)
            {
                if (onlineUsers.Any(u => u.Id == user.Id))
                {
                    user.IsOnline = true;
                }
            }

            StateHasChanged();
        });

        hubConnection.On<string>(nameof(IBlazingChatHubClient.UserIsOnline), (userId) =>
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user is not null)
            {
                user.IsOnline = true;
                StateHasChanged();
            }
        });

        hubConnection.On<MessageDto>(nameof(IBlazingChatHubClient.MessageReceived), (messageDto) =>
        {
            //var fromUser = Users.FirstOrDefault(u => u.Id == messageDto.SenderId);

            var receiver = Users.FirstOrDefault(u => u.Id == messageDto.SenderId);
            if (receiver != null)
            {
                // If the receiver is not the current chat person, increase their unread message count
                if (ChatPerson?.Id != messageDto.SenderId)
                {
                    //receiver.UnreadMessagesCount++;

                    if (parameters != null && parameters.ContainsKey("UserId") && parameters["UserId"] is string userId && userId == receiver.Id)
                    {
                        receiver.UnreadMessagesCount = 0;
                    }
                    else
                    {
                        receiver.UnreadMessagesCount++;
                    }
                }
            }

            // Find the current user
            var currentUser = Users.FirstOrDefault(u => u.Id == CurrentPerson.Id); // Replace `CurrentUserId` with the actual current user identifier

            if (currentUser != null)
            {
                // First, remove the current user from the list (if they exist)
                var otherUsers = Users.Where(u => u.Id != currentUser.Id).OrderByDescending(u => u.UnreadMessagesCount).ToList();

                // Add the current user at the first position
                Users = new List<UserDto> { currentUser }.Concat(otherUsers).ToList();
            }
            else
            {
                // If currentUser is not found, just sort the list by UnreadMessagesCount
                Users = Users.OrderByDescending(u => u.UnreadMessagesCount).ToList();
            }

            StateHasChanged();  // Refresh UI after processing message
        });

        hubConnection.On<string, int>(nameof(IBlazingChatHubClient.UpdateUnreadMessagesCount), async (userId, unreadMessagesCount) =>
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.UnreadMessagesCount = unreadMessagesCount;
                StateHasChanged();
            }
        });

        return hubConnection;
    }

    private async Task<IEnumerable<string>> OnSearch(string value, CancellationToken token)
    {
        await Task.Delay(5, token);

        var currentUser = await GetCurrentUser();

        if (string.IsNullOrEmpty(value))
        {
            var userCollection = await _userClientService.GetAllAsync(null);
            
            Users.Clear();

            SetCurrentUserToTop(currentUser);

            return Enumerable.Empty<string>();
        }

        SearchedUser = value;

        var matchedUsers = Users
            .Where(x =>
                x.FirstName.StartsWith(value, StringComparison.InvariantCultureIgnoreCase) ||
                x.FirstName.Equals(value, StringComparison.InvariantCultureIgnoreCase)
            )
            .ToList();

        var finalUsers = Users
            .Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        var combinedUsers = finalUsers.Concat(matchedUsers).Distinct().ToList();

        Users = combinedUsers;

        //SetCurrentUserToTop(currentUser);

        await _hubConnection.SendAsync(nameof(IBlazingChatHubServer.SetUserOnline), currentUser);

        StateHasChanged();

        return combinedUsers.Select(x => x.FirstName);
    }

    private async Task<UserDto> GetCurrentUser()
    {
        var userDto = new UserDto();

        var authState = await PersistentAuthenticationStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;

        if (user != null && user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            userDto = await _userClientService.GetWithIdAsync(userId);
        }

        return userDto;
    }

    private async void SetCurrentUserToTop(UserDto currentUser)
    {
        var allUsers = await _userClientService.GetAllAsync(null);

        if (allUsers.Any(u => u.Id == currentUser.Id))
        {
            var existingUser = allUsers.FirstOrDefault(u => u.Id == currentUser.Id);
            allUsers.Remove(existingUser);
            allUsers.Insert(0, existingUser);
        }

        foreach (var user in allUsers)
        {
            if (user.Id != currentUser.Id) 
            {
                var messages = await _messageClientService.GetAll(user.Id, currentUser.Id);
                user.UnreadMessagesCount = messages.Count(x => !x.IsRead);
            }
        }

        Users.AddRange(allUsers);

        await _hubConnection.SendAsync(nameof(IBlazingChatHubServer.SetUserOnline), currentUser);

        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    private async void OpenChat(string userId)
    {
        parameters.Remove("UserId");
        parameters.Remove("IsDefault");
        parameters["UserId"] = userId;
        parameters["IsDefault"] = false;
        parameters["OnReadAllMessages"] = EventCallback.Factory.Create<bool>(
                                                this, HandleMessageRead);
        parameters["OnMessageSent"] = EventCallback.Factory.Create<string>(
                                                this, HandleMessageNotification);
        parameters["ChatPersonId"] = EventCallback.Factory.Create<string>(
                                                this, HandleChatAlreadyOpen);
        var user = Users.FirstOrDefault(u => u.Id == userId);

        if (user != null)
        {
            user.UnreadMessagesCount = 0;
        }

        StateHasChanged();
    }

    private async void HandleMessageRead()
    {
        //var currentUser = await GetCurrentUser();

        //SetCurrentUserToTop(currentUser);

        StateHasChanged();
    }

    private async void HandleChatAlreadyOpen(string chatPersonId)
    {
        await _messageClientService.MarkAllMessageRead(chatPersonId, CurrentPerson.Id);

        StateHasChanged();
    }


    private async void HandleMessageNotification(string receiverId)
    {
        if (parameters.ContainsKey("IsDefault") && parameters.ContainsKey("UserId")) { }

        var currentPerson = await GetCurrentUser();

        var messages = await _messageClientService.GetAll(currentPerson.Id, receiverId);

        var unReadMessageCount = messages.Count(x => !x.IsRead);

        foreach (var user in Users)
        {
            if (user.Id.Equals(receiverId)) 
            {
                user.UnreadMessagesCount = unReadMessageCount;
            }
        }

        StateHasChanged();
    }

    #endregion
}