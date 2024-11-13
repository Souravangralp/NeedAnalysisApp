namespace NeedAnalysisApp.Repositories.Services;

public class MessageService : IMessageService
{
    #region Fields

    private readonly IHubContext<ChatHub, IBlazingChatHubClient> _hubContext;
    private readonly ApplicationDbContext _context;

    #endregion

    #region Ctor

    public MessageService(ApplicationDbContext context, 
        IHubContext<ChatHub, IBlazingChatHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    #endregion

    #region Methods

    public async Task<Result> SendMessageAsync(MessageDto messageDto)
    {
        if (string.IsNullOrWhiteSpace(messageDto.ReceiverId) || string.IsNullOrWhiteSpace(messageDto.SenderId))
            return new Result() { Success = false };

        var message = GetMessageAsync(messageDto);

        await _context.Messages.AddAsync(message);

        if (await _context.SaveChangesAsync() > 0)
        {
            var responseMessageDto = new MessageDto()
            {
                ReceiverId = message.ApplicationUser_ReceiverId,
                SenderId = message.ApplicationUser_SenderId,
                Content = message.Content,
                Timestamp = message.SentOn
            };

            await _hubContext.Clients.User(messageDto.ReceiverId).MessageReceived(messageDto);

            var unreadMessageCount = await _context.Messages.Where(m => m.ApplicationUser_SenderId == responseMessageDto.SenderId && m.ApplicationUser_ReceiverId == responseMessageDto.ReceiverId).CountAsync(x => !x.IsRead);

            await _hubContext.Clients.User(messageDto.SenderId).UpdateUnreadMessagesCount(messageDto.SenderId, messageDto.ReceiverId, unreadMessageCount);

            return new Result() { Success = true };
        }
        else
        {
            return new Result() { Success = false };
        }
    }

    public async Task<List<MessageDto>> GetMessages(string senderId, string receiverId)
    {
        var messages = await _context.Messages
                    .AsNoTracking()
                    .Where(m =>
                        (m.ApplicationUser_SenderId == senderId && m.ApplicationUser_ReceiverId == receiverId) ||
                        (m.ApplicationUser_ReceiverId == senderId && m.ApplicationUser_SenderId == receiverId)
                    )
                    .Include(m => m.File)
                    .Select(m => new MessageDto()
                    {
                        UniqueId = m.UniqueId,
                        ReceiverId = m.ApplicationUser_ReceiverId,
                        SenderId = m.ApplicationUser_SenderId,
                        Content = m.Content,
                        Timestamp = m.SentOn,
                        IsRead = m.IsRead,
                        File = new()
                        {
                            FileName = m.File.FileName,
                            FileUrl = m.File.FileUrl,
                            FileType = m.File.FileType
                        }
                    })
                    .ToListAsync();

        return messages;
    }

    public Task<MessageDto> GetMessageByIdAsync(Guid messageId)
    {
        throw new NotImplementedException();
    }

    private static Message GetMessageAsync(MessageDto messageDto)
    {
        #region remove this
        //make use of auto mapper

        //var file = (messageDto.File != null) ? new Data.Models.Chat.File
        //{
        //    FileName = messageDto.File.FileName,
        //    FileUrl = messageDto.File.FileUrl,
        //    FileType = messageDto.File.FileType,
        //} : null;

        //var unReadMessages = new Message
        //{
        //    ApplicationUser_SenderId = messageDto.SenderId,
        //    ApplicationUser_ReceiverId = messageDto.ReceiverId,
        //    Content = messageDto.Content,
        //    SentOn = DateTime.Now,
        //    File = file,
        //};

        //return unReadMessages;

        #endregion

        //make use of auto mapper
        return new Message()
        {
            ApplicationUser_SenderId = messageDto.SenderId,
            ApplicationUser_ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
            SentOn = DateTime.Now,
            File = (messageDto.File != null) ? new Data.Models.Chat.File
            {
                FileName = messageDto.File.FileName,
                FileUrl = messageDto.File.FileUrl,
                FileType = messageDto.File.FileType,
            } : null
        };
    }

    public async Task<bool> MarkReadAsync(string senderId, string receiverId, string messageId)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.UniqueId == messageId);

        if (message == null) return false;

        // Mark the unReadMessages as read
        message.IsRead = true;
        await _context.SaveChangesAsync();

        // Decrement the unread unReadMessages count for the receiver
        await _hubContext.Clients.User(receiverId).UpdateUnreadMessagesCount(senderId, receiverId, 0);

        return true;
    }

    public async Task<bool> MarkReadAllAsync(string senderId, string receiverId) 
    {
        var unReadMessages = await _context.Messages.Where(x => x.ApplicationUser_SenderId == senderId && 
                x.ApplicationUser_ReceiverId == receiverId &&
                !x.IsRead
                ).ToListAsync();

        if (unReadMessages == null) return false;

        unReadMessages.ForEach(x => x.IsRead = true);

        _context.UpdateRange(unReadMessages);
        await _context.SaveChangesAsync();

        // Decrement the unread unReadMessages count for the receiver
        await _hubContext.Clients.User(receiverId).UpdateUnreadMessagesCount(senderId, receiverId, 0);

        return true;
    }

    #endregion

    #region Do not remove below listed methods

    //private readonly IHubContext<ChatHub, IBlazingChatHubClient> _hubContext;
    //private readonly ApplicationDbContext _context;

    //public MessageService(ApplicationDbContext context,
    //    IHubContext<ChatHub, IBlazingChatHubClient> hubContext)
    //{
    //    _context = context;
    //    _hubContext = hubContext;

    //}

    //public async Task<List<MessageDto>> GetMessages(string senderId, string receiverId)
    //{
    //    var messages = await _context.Messages
    //                    .AsNoTracking()
    //                    .Where(m =>
    //                        (m.ApplicationUser_SenderId == senderId && m.ApplicationUser_ReceiverId == receiverId) ||
    //                        (m.ApplicationUser_ReceiverId == senderId && m.ApplicationUser_SenderId == receiverId)
    //                    )
    //                    .Include(m=> m.File)
    //                    .Select(m => new MessageDto()
    //                    {
    //                        UniqueId = m.UniqueId,
    //                        ReceiverId = m.ApplicationUser_ReceiverId,
    //                        SenderId = m.ApplicationUser_SenderId,
    //                        Content = m.Content,
    //                        Timestamp = m.SentOn,
    //                        File = new() 
    //                        {
    //                            FileName = m.File.FileName,
    //                            FileUrl = m.File.FileUrl,
    //                            FileType = m.File.FileType
    //                        }
    //                    })
    //                    .ToListAsync();

    //    return messages;
    //}

    //public Task<MessageDto> GetMessageByIdAsync(Guid messageId)
    //{
    //    throw new NotImplementedException();
    //}

    //public async Task<Result> SendMessageAsync(MessageDto messageDto)
    //{
    //    if (string.IsNullOrWhiteSpace(messageDto.ReceiverId) || string.IsNullOrWhiteSpace(messageDto.SenderId))
    //        return new Result() { Success = false };

    //    var file = (messageDto.File != null) ? new Data.Models.Chat.File
    //    {
    //        FileName = messageDto.File.FileName,
    //        FileUrl = messageDto.File.FileUrl,
    //        FileType = messageDto.File.FileType,
    //    } : null;

    //    var unReadMessages = new Message
    //    {
    //        ApplicationUser_SenderId = messageDto.SenderId,
    //        ApplicationUser_ReceiverId = messageDto.ReceiverId,
    //        Content = messageDto.Content,
    //        SentOn = DateTime.Now,
    //        File = file,
    //    };

    //    await _context.Messages.AddAsync(unReadMessages);

    //    if (await _context.SaveChangesAsync() > 0)
    //    {
    //        var responseMessageDto = new MessageDto()
    //        {
    //            ReceiverId = unReadMessages.ApplicationUser_ReceiverId,
    //            SenderId = unReadMessages.ApplicationUser_SenderId,
    //            Content = unReadMessages.Content,
    //            Timestamp = unReadMessages.SentOn
    //        };

    //        await _hubContext.Clients.User(messageDto.ReceiverId.ToString())
    //                    .MessageReceived(responseMessageDto);
    //        //return new OkResult();
    //        return new Result() { Success = true };
    //    }
    //    else
    //    {
    //        return new Result() { Success = false };
    //    }
    //}

    //public async Task<bool> MarkReadAsync(string messageId)
    //{
    //    var unReadMessages = await _context.Messages.FirstOrDefaultAsync(x => x.UniqueId == messageId);

    //    if (unReadMessages == null) { return false; }

    //    unReadMessages.IsRead = true;
    //    await _context.SaveChangesAsync();

    //    await _hubContext.Clients.User("")
    //                .UpdateUnreadMessagesCount("", 2);

    //    return true;
    //}

    #endregion

    #region Do not remove below listed methods

    //private readonly ApplicationDbContext _context; // Replace with your actual DbContext
    //private readonly IMapper _mapper; // Assuming you use AutoMapper

    //public MessageService(ApplicationDbContext context, IMapper mapper)
    //{
    //    _context = context;
    //    _mapper = mapper;
    //}


    //// Send a unReadMessages
    //public async Task<MessageDto> SendMessageAsync(MessageDto messageDto)
    //{
    //    var unReadMessages = _mapper.Map<Data.Models.Chat.Message>(messageDto); // Map DTO to Entity
    //                    // Generate a new MessageId
    //    unReadMessages.SentOn = DateTime.UtcNow; // Set the timestamp
    //    unReadMessages.IsRead = false; // Default value

    //    await _context.Messages.AddAsync(unReadMessages);
    //    await _context.SaveChangesAsync();

    //    return _mapper.Map<MessageDto>(unReadMessages); // Map back to DTO
    //}

    //// Get messages between two users
    //public async Task<IEnumerable<MessageDto>> GetMessagesAsync(string senderId, string receiverId)
    //{
    //    var messages = await _context.Messages
    //        .Where(m => (m.ApplicationUser_SenderId.ToString() == senderId && m.ApplicationUser_ReceiverId.ToString() == receiverId) ||
    //                     (m.ApplicationUser_SenderId.ToString() == receiverId && m.ApplicationUser_ReceiverId.ToString() == senderId))
    //        .OrderBy(m => m.SentOn)
    //        .ToListAsync();

    //    return _mapper.Map<IEnumerable<MessageDto>>(messages); // Map to DTOs
    //}

    //// Get a specific unReadMessages by ID
    //public async Task<MessageDto> GetMessageByIdAsync(Guid messageId)
    //{
    //    var unReadMessages = await _context.Messages.FindAsync(messageId);
    //    return unReadMessages == null ? null : _mapper.Map<MessageDto>(unReadMessages); // Map to DTO
    //}

    #endregion 
}