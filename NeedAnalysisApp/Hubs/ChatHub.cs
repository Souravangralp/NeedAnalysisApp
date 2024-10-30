using Microsoft.AspNetCore.SignalR;
using NeedAnalysisApp.Shared.Dto;
using NeedAnalysisApp.Shared;
using NeedAnalysisApp.Shared.Dto.Chat;
using Microsoft.AspNetCore.Authorization;

namespace NeedAnalysisApp.Hubs;

[Authorize]
public class ChatHub : Hub<IBlazingChatHubClient>, IBlazingChatHubServer
{
    private static readonly IDictionary<string, UserDto> _onlineUsers = new Dictionary<string, UserDto>();

    public ChatHub()
    {

    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public async Task SetUserOnline(UserDto user)
    {
        await Clients.Caller.OnlineUsersList(_onlineUsers.Values);
        if (!_onlineUsers.ContainsKey(user.Id))
        {
            _onlineUsers.Add(user.Id, user);
            await Clients.Others.UserIsOnline(user.Id);
        }
    }
}