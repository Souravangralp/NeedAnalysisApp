namespace NeedAnalysisApp.Client.Pages.Chat;

public partial class Home
{
    #region Injecting services

    [Inject] public IUserClientService _userClientService { get; set; } = null!;
    [Inject] public NavigationManager _navigationManager { get; set; } = null!;
    [Inject] IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public AuthenticationStateProvider PersistentAuthenticationStateProvider { get; set; } = null!;

    private HubConnection? _hubConnection;

    #endregion

    #region Fields

    public UserDto ChatPerson { get; set; } = new();
    public UserDto CurrentPerson { get; set; } = new();
    public List<UserDto> Users { get; set; } = [];
    public List<ChatDto> Chats { get; set; } = [];
    public Dictionary<string, object> parameters { get; set; } = new();

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = ConfigureHubConnection();

        await _hubConnection.StartAsync();

        parameters["IsDefault"] = true; // Set default state

        var currentUserId = await GetCurrentUserId();

        var currentUser = await _userClientService.GetWithId(currentUserId);

        var allUsers = await _userClientService.GetAll(null);

        if (allUsers.Any(u => u.Id == currentUserId))
        {
            var existingUser = allUsers.FirstOrDefault(u => u.Id == currentUserId);
            allUsers.Remove(existingUser);
            allUsers.Insert(0, existingUser);
        }

        Users.AddRange(allUsers);

        await _hubConnection.SendAsync(nameof(IBlazingChatHubServer.SetUserOnline), currentUser);

        StateHasChanged();
    }

    private async Task<string> GetCurrentUserId()
    {
        string userId = "";

        var authState = await PersistentAuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        return userId;
    }

    private HubConnection ConfigureHubConnection()
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

        hubConnection.On<string, int>(nameof(IBlazingChatHubClient.UpdateUnreadMessagesCount), (userId, unreadMessagesCount) =>
        {
            // Find the user and update the unread message count
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.UnreadMessagesCount = unreadMessagesCount;
                StateHasChanged();
            }
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

            if (ChatPerson?.Id == messageDto.SenderId)
            {
                Chats.Add(new ChatDto()
                {
                    Message = messageDto,
                    User = CurrentPerson
                });

                // Update the unread message count for the receiver (ChatPerson)
                var receiver = Users.FirstOrDefault(u => u.Id == messageDto.SenderId);
                if (receiver != null)
                {
                    receiver.UnreadMessagesCount++;
                }
            }

            StateHasChanged();
        });

        return hubConnection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    public void OpenChat(string id)
    {
        parameters.Remove("UserId");
        parameters.Remove("IsDefault");
        parameters["UserId"] = id;
        parameters["IsDefault"] = false;

        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            user.UnreadMessagesCount = 0;
        }

        StateHasChanged();
    }

    #endregion
}