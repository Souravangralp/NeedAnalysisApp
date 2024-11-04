namespace NeedAnalysisApp.Shared;

public interface IBlazingChatHubServer
{
    Task SetUserOnline(UserDto user);
}
