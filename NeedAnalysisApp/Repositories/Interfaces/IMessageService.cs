namespace NeedAnalysisApp.Repositories.Interfaces;

public interface IMessageService
{
    Task<Result> SendMessageAsync(MessageDto messageDto);

    Task<List<MessageDto>> GetMessages(string otherUserId, string receiverId);

    Task<bool> MarkReadAsync(string senderId, string receiverId, string messageId);

    Task<MessageDto> GetMessageByIdAsync(Guid messageId);
}
