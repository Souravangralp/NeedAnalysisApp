using NeedAnalysisApp.Shared.Dto.Chat;

namespace NeedAnalysisApp.Client.Repositories.Interfaces;

public interface IMessageClientService
{
    Task<bool> Send(MessageDto message);

    Task<List<MessageDto>> GetAll(string senderId, string receiverId);

    Task<bool> MarkMessageRead(string messageId, string receiverId);
}