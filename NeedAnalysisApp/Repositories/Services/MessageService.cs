using Microsoft.AspNetCore.SignalR;
using NeedAnalysisApp.Client.Pages.Assessments;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Hubs;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Repositories.Services;

public class MessageService : IMessageService
{
    #region Working

    private readonly IHubContext<ChatHub, IBlazingChatHubClient> _hubContext;
    private readonly ApplicationDbContext _context;

    public MessageService(ApplicationDbContext context,
        IHubContext<ChatHub, IBlazingChatHubClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;

    }

    public async Task<List<MessageDto>> GetMessages(string senderId,string receiverId)
    {
        var messages = await _context.Messages
                        .AsNoTracking()
                        .Where(m =>
                            (m.ApplicationUser_SenderId == senderId && m.ApplicationUser_ReceiverId == receiverId) || 
                            (m.ApplicationUser_ReceiverId == senderId && m.ApplicationUser_SenderId == receiverId)
                        )
                        .Select(m => new MessageDto()
                        {
                            ReceiverId = m.ApplicationUser_ReceiverId,
                            SenderId = m.ApplicationUser_SenderId,
                            Content = m.Content,
                            Timestamp = m.SentOn,
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
        if (string.IsNullOrWhiteSpace(messageDto.ReceiverId) || string.IsNullOrWhiteSpace(messageDto.Content))
            return new Result() { Success = false };

        var message = new Message
        {
            ApplicationUser_SenderId = messageDto.SenderId,
            ApplicationUser_ReceiverId = messageDto.ReceiverId,
            Content = messageDto.Content,
            SentOn = DateTime.Now
        };

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

            await _hubContext.Clients.User(messageDto.ReceiverId.ToString())
                        .MessageReceived(responseMessageDto);
            //return new OkResult();
            return new Result() { Success = true };
        }
        else
        {
            return new Result() { Success = false };
        }
    }

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
