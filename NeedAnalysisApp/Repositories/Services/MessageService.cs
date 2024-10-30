using AutoMapper;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Repositories.Services;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context; // Replace with your actual DbContext
    private readonly IMapper _mapper; // Assuming you use AutoMapper

    public MessageService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Send a message
    public async Task<MessageDto> SendMessageAsync(MessageDto messageDto)
    {
        var message = _mapper.Map<Data.Models.Chat.Message>(messageDto); // Map DTO to Entity
                        // Generate a new MessageId
        message.SentOn = DateTime.UtcNow; // Set the timestamp
        message.IsRead = false; // Default value

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        return _mapper.Map<MessageDto>(message); // Map back to DTO
    }

    // Get messages between two users
    public async Task<IEnumerable<MessageDto>> GetMessagesAsync(string senderId, string receiverId)
    {
        var messages = await _context.Messages
            .Where(m => (m.ApplicationUser_SenderId.ToString() == senderId && m.ApplicationUser_ReceiverId.ToString() == receiverId) ||
                         (m.ApplicationUser_SenderId.ToString() == receiverId && m.ApplicationUser_ReceiverId.ToString() == senderId))
            .OrderBy(m => m.SentOn)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MessageDto>>(messages); // Map to DTOs
    }

    // Get a specific message by ID
    public async Task<MessageDto> GetMessageByIdAsync(Guid messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        return message == null ? null : _mapper.Map<MessageDto>(message); // Map to DTO
    }
}
