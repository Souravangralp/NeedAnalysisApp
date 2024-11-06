

namespace NeedAnalysisApp.Repositories.Services;

public class MessageService : IMessageService
{

    private readonly IHubContext<ChatHub, IBlazingChatHubClient> _hubContext;
    private readonly ApplicationDbContext _context;

    public MessageService(ApplicationDbContext context, IHubContext<ChatHub, IBlazingChatHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    #region Working Methods

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

    public async Task<Result> SendMessageAsync(MessageDto messageDto)
    {
        if (string.IsNullOrWhiteSpace(messageDto.ReceiverId) || string.IsNullOrWhiteSpace(messageDto.SenderId))
            return new Result() { Success = false };

        var file = (messageDto.File != null) ? new Data.Models.Chat.File
        {
            FileName = messageDto.File.FileName,
            FileUrl = messageDto.File.FileUrl,
            FileType = messageDto.File.FileType,
        } : null;

        var message = new Message
        {
            ApplicationUser_SenderId = messageDto.SenderId,
            ApplicationUser_ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
            SentOn = DateTime.Now,
            File = file,
        };

        await _context.Messages.AddAsync(message);

        if (await _context.SaveChangesAsync() > 0)
        {
            // Prepare the response message DTO
            var responseMessageDto = new MessageDto()
            {
                ReceiverId = message.ApplicationUser_ReceiverId,
                SenderId = message.ApplicationUser_SenderId,
                Content = message.Content,
                Timestamp = message.SentOn
            };

            // Send the message to the receiver
            await _hubContext.Clients.User(messageDto.ReceiverId).MessageReceived(responseMessageDto);

            // Increment the unread message count for the receiver
            await _hubContext.Clients.User(messageDto.ReceiverId).UpdateUnreadMessagesCount(messageDto.ReceiverId, 1);

            return new Result() { Success = true };
        }
        else
        {
            return new Result() { Success = false };
        }
    }

    // Mark a message as read and update unread message count
    public async Task<bool> MarkRead(string messageId, string receiverId)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.UniqueId == messageId);

        if (message == null) return false;

        // Mark the message as read
        message.IsRead = true;
        await _context.SaveChangesAsync();

        // Decrement the unread message count for the receiver
        await _hubContext.Clients.User(receiverId).UpdateUnreadMessagesCount(receiverId, 0);

        return true;
    }

    #endregion

    #region Test (commented out)

    #region Working

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

    //    var message = new Message
    //    {
    //        ApplicationUser_SenderId = messageDto.SenderId,
    //        ApplicationUser_ReceiverId = messageDto.ReceiverId,
    //        Content = messageDto.Content,
    //        SentOn = DateTime.Now,
    //        File = file,
    //    };

    //    await _context.Messages.AddAsync(message);

    //    if (await _context.SaveChangesAsync() > 0)
    //    {
    //        var responseMessageDto = new MessageDto()
    //        {
    //            ReceiverId = message.ApplicationUser_ReceiverId,
    //            SenderId = message.ApplicationUser_SenderId,
    //            Content = message.Content,
    //            Timestamp = message.SentOn
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

    //public async Task<bool> MarkRead(string messageId)
    //{
    //    var message = await _context.Messages.FirstOrDefaultAsync(x => x.UniqueId == messageId);

    //    if (message == null) { return false; }

    //    message.IsRead = true;
    //    await _context.SaveChangesAsync();

    //    await _hubContext.Clients.User("")
    //                .UpdateUnreadMessagesCount("", 2);

    //    return true;
    //}

    #endregion

    #endregion

    #region Test

    //private readonly ApplicationDbContext _context; // Replace with your actual DbContext
    //private readonly IMapper _mapper; // Assuming you use AutoMapper

    //public MessageService(ApplicationDbContext context, IMapper mapper)
    //{
    //    _context = context;
    //    _mapper = mapper;
    //}


    //// Send a message
    //public async Task<MessageDto> SendMessageAsync(MessageDto messageDto)
    //{
    //    var message = _mapper.Map<Data.Models.Chat.Message>(messageDto); // Map DTO to Entity
    //                    // Generate a new MessageId
    //    message.SentOn = DateTime.UtcNow; // Set the timestamp
    //    message.IsRead = false; // Default value

    //    await _context.Messages.AddAsync(message);
    //    await _context.SaveChangesAsync();

    //    return _mapper.Map<MessageDto>(message); // Map back to DTO
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

    //// Get a specific message by ID
    //public async Task<MessageDto> GetMessageByIdAsync(Guid messageId)
    //{
    //    var message = await _context.Messages.FindAsync(messageId);
    //    return message == null ? null : _mapper.Map<MessageDto>(message); // Map to DTO
    //}

    #endregion

}
