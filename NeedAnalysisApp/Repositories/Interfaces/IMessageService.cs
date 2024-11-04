using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IMessageService
{
    Task<Result> SendMessageAsync(MessageDto messageDto);
    Task<List<MessageDto>> GetMessages(string otherUserId, string receiverId);

    //Task<MessageDto> SendMessageAsync(MessageDto messageDto);
    //Task<IEnumerable<MessageDto>> GetMessagesAsync(string senderId, string receiverId);
    Task<MessageDto> GetMessageByIdAsync(Guid messageId);
}
