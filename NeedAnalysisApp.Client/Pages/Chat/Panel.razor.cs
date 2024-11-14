namespace NeedAnalysisApp.Client.Pages.Chat;

public partial class Panel : IAsyncDisposable
{
    #region Fields

    [Inject] public AuthenticationStateProvider PersistentAuthenticationStateProvider { get; set; } = null!;

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject] public IUserClientService _userClientService { get; set; } = null!;

    [Inject] public IMessageClientService _messageClientService { get; set; } = null!;

    [Inject] public IFilesClientService _filesClientService { get; set; } = null!;

    [Inject] public NavigationManager _navigationManager { get; set; } = null!;

    [Inject] private ISnackbar SnackBar { get; set; } = null!;

    [Parameter] public EventCallback<string> OnMessageSent { get; set; }

    [Parameter] public EventCallback<bool> OnReadAllMessages { get; set; }

    [Comment("Logged-in person selected this person to send messages Receiver Id")]
    [Parameter] public required string UserId { get; set; }
    
    //[Parameter] public required List<UserDto> Users { get; set; } = [];

    [Parameter] public EventCallback<string> ChatPersonId { get; set; }

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

    private List<ChatDto> Chats { get; set; } = [];
    private List<UserDto> Users { get; set; } = [];

    private string MessageText { get; set; } = string.Empty;

    #endregion

    #region Methods

    protected override async Task OnInitializedAsync()
    {
        Users = await _userClientService.GetAllAsync(null);

        _hubConnection = ConfigureHub();

        await _hubConnection.StartAsync();

        if (!string.IsNullOrWhiteSpace(UserId))
        {
            await LoadMessagesAsync();
        }


        StateHasChanged();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserId))
        {
            ChatPerson = Users.FirstOrDefault(user => user.Id == UserId) ?? new(); // await _userClientService.GetWithIdAsync(UserId);

            await ChatPersonId.InvokeAsync(UserId);

            await LoadMessagesAsync();

            var currentUser = await GetCurrentUserAsync();

            CurrentPerson = await _userClientService.GetWithIdAsync(currentUser.Id);

            await _messageClientService.MarkAllMessageRead(ChatPerson.Id, CurrentPerson.Id);

            _scrollToBottom = true;

            _files.Clear();

            //Users = await _userClientService.GetAllAsync(null);

            MessageText = string.Empty;
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
                await _messageClientService.MarkAllMessageRead(ChatPerson.Id, CurrentPerson.Id);

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

        //hubConnection.On<ICollection<UserDto>>(nameof(IBlazingChatHubClient.OnlineUsersList), (onlineUsers) =>
        //{
        //    foreach (var user in Users)
        //    {
        //        if (onlineUsers.Any(u => u.Id == user.Id))
        //        {
        //            user.IsOnline = true;
        //        }
        //    }
        //    StateHasChanged();
        //});

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

            ShowSnackBarWithAvatar(fromUser?.FirstName, messageDto.Content, fromUser.ProfilePictureUrl);

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

        hubConnection.On<string, string, int>(nameof(IBlazingChatHubClient.UpdateUnreadMessagesCount), (senderId, receiverId, unreadMessagesCount) =>
        {
            var user = Users.FirstOrDefault(u => u.Id == receiverId);
            if (user != null)
            {
                user.UnreadMessagesCount = unreadMessagesCount;
                StateHasChanged();
            }
        });

        return hubConnection;
    }

    public void ShowSnackBarWithAvatar(string fromUserName, string messageContent, string avatarUrl)
    {
        SnackBar.Add(builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "d-flex align-items-center");

            builder.OpenComponent<MudAvatar>(2);
            builder.AddAttribute(3, "Src", avatarUrl);  // Avatar image URL
            builder.AddAttribute(4, "Size", Size.Large);  // You can adjust the size of the avatar here
            builder.CloseComponent(); // Close MudAvatar

            builder.OpenElement(5, "span");
            builder.AddAttribute(6, "class", "ml-2");
            builder.AddContent(7, $"Received a new message from: {fromUserName}");
            builder.CloseElement();

            builder.CloseElement(); // Close the div container
        }, Severity.Info);
    }

    private async Task LoadMessagesAsync()
    {
        Chats.Clear();

        var currentUser = await GetCurrentUserAsync();

        var messages = await _messageClientService.GetAll(currentUser.Id, UserId);

        var chats = await Task.WhenAll(messages.Select(async message =>
        {
            var user = await _userClientService.GetWithIdAsync(message.SenderId);
            return new ChatDto
            {
                User = user,
                Message = message
            };
        }));

        Chats = chats.ToList() ?? [];

        //_scrollToBottom = true;

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

    private async Task MarkMessageAsReadAsync(MessageDto message)
    {
        var response = await _messageClientService.MarkMessageRead(message);

        if (response)
        {
            var messageToBeMarkedAsRead = Chats.FirstOrDefault(chat => chat.Message.UniqueId == message.UniqueId)?.Message;

            if (messageToBeMarkedAsRead != null)
            {
                messageToBeMarkedAsRead.IsRead = true;

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
            var messageDto = await GetMessageAsync();

            var response = await _messageClientService.Send(messageDto);

            if (response)
            {
                await OnMessageSent.InvokeAsync(messageDto.ReceiverId);

                MessageText = string.Empty;

                await LoadMessagesAsync();

                _scrollToBottom = true;

                _files.Clear();

                await _hubConnection.SendAsync(nameof(IBlazingChatHubClient.MessageSentConfirmation), messageDto);

                StateHasChanged();
            }
        }
    }

    private async Task<MessageDto> GetMessageAsync()
    {
        var browserFile = _files.FirstOrDefault();

        var file = browserFile != null ? await _filesClientService.Upload(browserFile) : new FileDto();

        return new MessageDto
        {
            Content = MessageText,
            SenderId = CurrentPerson.Id,
            ReceiverId = ChatPerson.Id,
            Timestamp = DateTime.Now,
            File = file
        };
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
