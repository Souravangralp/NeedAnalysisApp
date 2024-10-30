using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IMessageService
{
    Task<MessageDto> SendMessageAsync(MessageDto messageDto);
    Task<IEnumerable<MessageDto>> GetMessagesAsync(string senderId, string receiverId);
    Task<MessageDto> GetMessageByIdAsync(Guid messageId);
}
