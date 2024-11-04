namespace NeedAnalysisApp.Shared;

public interface IBlazingChatHubClient
{
    Task UserConnected(UserDto user);

    Task OnlineUsersList(IEnumerable<UserDto> users);

    Task UserIsOnline(string userId);

    Task MessageReceived(MessageDto messageDto);
}
