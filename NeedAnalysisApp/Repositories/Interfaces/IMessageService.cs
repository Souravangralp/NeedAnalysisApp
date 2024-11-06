namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IMessageService
{
    Task<Result> SendMessageAsync(MessageDto messageDto);

    Task<List<MessageDto>> GetMessages(string otherUserId, string receiverId);

    Task<bool> MarkRead(string messageId, string receiverId);

    Task<MessageDto> GetMessageByIdAsync(Guid messageId);
}
