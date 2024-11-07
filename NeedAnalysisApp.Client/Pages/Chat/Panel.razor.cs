using System.Runtime.CompilerServices;

namespace NeedAnalysisApp.Client.Pages.Chat;

public partial class Panel
{
    #region Fields

    [Inject] public AuthenticationStateProvider PersistentAuthenticationStateProvider { get; set; } = null!;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject] public IUserClientService _userClientService { get; set; } = null!;

    [Inject] public IMessageClientService _messageClientService { get; set; } = null!;

    [Inject] public IFilesClientService _filesClientService { get; set; } = null!;

    [Inject] public NavigationManager _navigationManager { get; set; } = null!;

    [Parameter] public EventCallback<bool> OnReadAllMessages { get; set; }

    [Comment("Logged-in person selected this person to send messages Receiver Id")]
    [Parameter] public required string UserId { get; set; }

    [Comment("Used for displaying a blank panel when no chat is selected")]
    [Parameter] public bool IsDefault { get; set; }

    [Comment("Logged-in person")]
    UserDto CurrentPerson { get; set; } = new();

    [Comment("Selected person to chat with")]
    UserDto ChatPerson { get; set; } = new();

    [Comment("Managing the scroll to bottom event when panel populated")]
    private bool _scrollToBottom = false;

    private HubConnection? _hubConnection;

    List<IBrowserFile> _files = [];

    private List<UserDto> Users = [];

    private List<ChatDto> Chats { get; set; } = [];

    private string MessageText { get; set; } = string.Empty;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = ConfigureHub();

        await _hubConnection.StartAsync();

        if (!string.IsNullOrWhiteSpace(UserId))
        {
            await LoadMessagesAsync();
        }

        Users = await _userClientService.GetAllAsync(null);

        StateHasChanged();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserId))
        {
            ChatPerson = await _userClientService.GetWithIdAsync(UserId);

            await LoadMessagesAsync();

            var currentUser = await GetCurrentUserAsync();

            CurrentPerson = await _userClientService.GetWithIdAsync(currentUser.Id);

            _scrollToBottom = true;

            _files.Clear();

            Users = await _userClientService.GetAllAsync(null);
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
                await MarkMessageAsReadAsync(lastMessage.UniqueId, ChatPerson.Id);

                await OnReadAllMessages.InvokeAsync(true);
            }
        }

        await JsRuntime.InvokeVoidAsync("scrollToBottom", "chatContainer");
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
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.UnreadMessagesCount = unreadMessagesCount;
                StateHasChanged();
            }
        });

        return hubConnection;
    }

    private async Task LoadMessagesAsync()
    {
        Chats.Clear();

        var currentUser = await GetCurrentUserAsync();

        var messages = await _messageClientService.GetAll(currentUser.Id, UserId);

        List<ChatDto> chats = [];

        foreach (var message in messages)
        {
            chats.Add(new ChatDto()
            {
                User = await _userClientService.GetWithIdAsync(message.SenderId),
                Message = message
            });
        }

        Chats = chats;

        _scrollToBottom = true;

        StateHasChanged();
    }

    private async Task<UserDto> GetCurrentUserAsync()
    {
        var userDto = new UserDto();

        var authState = await PersistentAuthenticationStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity.IsAuthenticated)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            userDto = await _userClientService.GetWithIdAsync(userId);
        }

        return userDto;
    }

    private async Task MarkMessageAsReadAsync(string messageId, string receiverId)
    {
        var response = await _messageClientService.MarkMessageRead(messageId, receiverId);

        if (response)
        {
            var message = Chats.FirstOrDefault(chat => chat.Message.UniqueId == messageId)?.Message;

            if (message != null)
            {
                message.IsRead = true;

            }

            StateHasChanged();
        }
    }

    private async Task RemoveImageAsync()
    {
        _files.Clear();

        await Task.Delay(1);

        StateHasChanged();
    }

    private async Task SendMessageAsync()
    {
        if (!string.IsNullOrWhiteSpace(MessageText) || _files.Any())
        {
            var messageDto = new MessageDto
            {
                Content = MessageText,
                SenderId = CurrentPerson.Id, 
                ReceiverId = ChatPerson.Id, 
                Timestamp = DateTime.Now,
                File = await GetFileAsync()
            };

            var response = await _messageClientService.Send(messageDto);

            if (response)
            {
                MessageText = string.Empty;

                await LoadMessagesAsync();

                _scrollToBottom = true;

                _files.Clear();

                StateHasChanged();
            }
        }
    }

    private async Task<FileDto> GetFileAsync()
    {
        FileDto? file = null;

        var browserFile = _files.FirstOrDefault();

        return browserFile != null ? await _filesClientService.Upload(browserFile) : new FileDto();
    }

    private void UploadFiles(IBrowserFile file)
    {
        _files.Add(file);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
            await _hubConnection.DisposeAsync();
    }

    #endregion
}