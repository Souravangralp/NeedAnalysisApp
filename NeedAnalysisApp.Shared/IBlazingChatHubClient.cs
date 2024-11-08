namespace NeedAnalysisApp.Shared;

public interface IBlazingChatHubClient
{
    Task UserConnected(UserDto user);

    Task OnlineUsersList(IEnumerable<UserDto> users);

    Task UserIsOnline(string userId);

    Task UserIsOffline(string userId);

    Task MessageReceived(MessageDto messageDto);

    Task UpdateUnreadMessagesCount(string senderId, string receiverId, int unreadMessagesCount);

    Task MessageSentConfirmation(MessageDto messageDto);
}
