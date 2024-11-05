namespace NeedAnalysisApp.Client.Pages.Chat;

public partial class Panel
{
    #region Injecting service

    [Inject] public IUserClientService _userClientService { get; set; } = null!;
    [Inject] public IMessageClientService _messageClientService { get; set; } = null!;
    [Inject] public IFilesClientService _filesClientService { get; set; } = null!;
    [Inject] public AuthenticationStateProvider PersistentAuthenticationStateProvider { get; set; } = null!;
    [Inject] public NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    private HubConnection? _hubConnection;

    #endregion

    #region Fields

    // logged in person
    UserDto CurrentPerson { get; set; } = new();

    // selected person
    UserDto ChatPerson { get; set; } = new();

    [Parameter] public required string UserId { get; set; }

    [Parameter] public bool IsDefault { get; set; }

    List<IBrowserFile> _files = [];

    private bool _scrollToBottom = false;

    private List<UserDto> Users = [];

    private List<ChatDto> Chats { get; set; } = [];

    private MessageDto NewMessage { get; set; } = new();

    private string MessageText { get; set; } = "";

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = ConfigureHubConnection();

        await _hubConnection.StartAsync();

        if (!string.IsNullOrWhiteSpace(UserId))
        {
            await LoadMessagesAsync();
        }

        Users = await _userClientService.GetAll(null);

        StateHasChanged();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserId))
        {
            ChatPerson = await _userClientService.GetWithId(UserId);

            await LoadMessagesAsync();

            var currentUserId = await GetCurrentUserId();

            CurrentPerson = await _userClientService.GetWithId(currentUserId);

            _scrollToBottom = true;

            _files.Clear();

            Users = await _userClientService.GetAll(null);
        }

        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_scrollToBottom)
        {
            _scrollToBottom = false;

            var lastMessage = Chats.LastOrDefault()?.Message;
            if (lastMessage != null && !lastMessage.IsRead)
            {
                await MarkMessageAsRead(lastMessage.UniqueId);
            }
        }

        await JsRuntime.InvokeVoidAsync("scrollToBottom", "chatContainer");
    }

    private async Task MarkMessageAsRead(string messageId)
    {
        var response = await _messageClientService.MarkMessageRead(messageId);
        if (response)
        {
            var message = Chats.FirstOrDefault(chat => chat.Message.UniqueId == messageId)?.Message;
            if (message != null)
            {
                message.IsRead = true;
                
            }
            StateHasChanged(); // Refresh UI
        }
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

    private async Task LoadMessagesAsync()
    {
        Chats.Clear();

        var senderId = await GetCurrentUserId();

        var sender = await _userClientService.GetWithId(senderId);

        var messages = await _messageClientService.GetAll(senderId, UserId);

        List<ChatDto> newMessages = [];

        foreach (var message in messages)
        {
            newMessages.Add(new ChatDto()
            {
                User = await _userClientService.GetWithId(message.SenderId),
                Message = message
            });
        }

        Chats = newMessages;

        _scrollToBottom = true;

        StateHasChanged();
    }

    private async Task RemoveImage()
    {
        _files.Clear();
        StateHasChanged();
    }

    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(MessageText) || _files.Any())
        {
            var messageDto = new MessageDto
            {
                Content = MessageText,
                SenderId = CurrentPerson.Id, // Ensure you have the current user's ID
                ReceiverId = ChatPerson.Id, // Ensure you have the current user's ID
                Timestamp = DateTime.Now,
                File = await GetFile()
            };

            var response = await _messageClientService.Send(messageDto);

            if (response)
            {
                MessageText = ""; // Clear the input field

                await LoadMessagesAsync();

                _scrollToBottom = true;

                _files.Clear();

                StateHasChanged();
            }
        }
    }

    private async Task<FileDto> GetFile()
    {
        FileDto? file = null;

        var browserFile = _files.FirstOrDefault();

        return browserFile != null ? await _filesClientService.Upload(browserFile) : new FileDto();
    }

    private void UploadFiles(IBrowserFile file)
    {
        _files.Add(file);
        //TODO upload the files to the server
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

        return hubConnection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
            await _hubConnection.DisposeAsync();
    }

    #endregion
}