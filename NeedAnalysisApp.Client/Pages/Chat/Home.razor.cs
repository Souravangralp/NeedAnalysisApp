﻿namespace NeedAnalysisApp.Client.Pages.Chat;

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

    public List<ChatDto> Chats { get; set; } = [];

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
            var fromUser = Users.FirstOrDefault(u => u.Id == messageDto.SenderId);

            // If the message is for the currently open chat
            if (ChatPerson?.Id == messageDto.SenderId)
            {
                Chats.Add(new ChatDto()
                {
                    Message = messageDto,
                    User = CurrentPerson
                });

                // UpdateAsync the unread message count for the receiver (ChatPerson)
                var receiver = Users.FirstOrDefault(u => u.Id == messageDto.SenderId);
                if (receiver != null)
                {
                    // If the receiver is not the current chat person, increase their unread message count
                    if (ChatPerson?.Id != messageDto.ReceiverId)
                    {
                        receiver.UnreadMessagesCount++;
                    }
                }
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

        if (string.IsNullOrEmpty(value))
        {
            var userCollection = await _userClientService.GetAllAsync(null);
            var currentUser = await GetCurrentUser();

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

        //List<UserChatDto> userChats = [];

        //foreach (var user in allUsers)
        //{
        //    var messages = await _messageClientService.GetAll(currentUser.Id, user.Id);

        //    messages = messages.Where(x => !x.IsRead).ToList();

        //    userChats.Add(new UserChatDto() { Messages = messages, User = user });
        //}

        //UserChats = userChats;

        if (allUsers.Any(u => u.Id == currentUser.Id))
        {
            var existingUser = allUsers.FirstOrDefault(u => u.Id == currentUser.Id);
            allUsers.Remove(existingUser);
            allUsers.Insert(0, existingUser);
        }

        Users.AddRange(allUsers);

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

    #endregion
}