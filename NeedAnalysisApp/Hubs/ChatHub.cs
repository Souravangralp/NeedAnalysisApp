using Microsoft.AspNetCore.Authorization;

namespace NeedAnalysisApp.Hubs;

[Authorize]
public class ChatHub : Hub<IBlazingChatHubClient>, IBlazingChatHubServer
{
    private static readonly Dictionary<string, UserDto> _onlineUsers = new Dictionary<string, UserDto>();
    private static readonly Dictionary<string, int> _unreadMessagesCount = new Dictionary<string, int>();

    public ChatHub()
    {

    }

    public override Task OnConnectedAsync()
    {
        var onlineUsers = _onlineUsers.Values;
        Clients.Caller.OnlineUsersList(onlineUsers);
        return base.OnConnectedAsync();
    }
    public async Task SetUserOnline(UserDto user)
    {
        // Send the list of online users to the caller
        await Clients.Caller.OnlineUsersList(_onlineUsers.Values);

        // Add the user to the online users dictionary if they're not already present
        if (!_onlineUsers.ContainsKey(user.Id))
        {
            _onlineUsers.Add(user.Id, user);
            // Notify other clients that this user is online
            await Clients.Others.UserIsOnline(user.Id);
        }

        // Initialize unread messages count for the user
        if (!_unreadMessagesCount.ContainsKey(user.Id))
        {
            _unreadMessagesCount.Add(user.Id, 0);
        }
    }
    //public async Task SetUserOnline(UserDto user)
    //{
    //    await Clients.Caller.OnlineUsersList(_onlineUsers.Values);
    //    if (!_onlineUsers.ContainsKey(user.Id))
    //    {
    //        _onlineUsers.Add(user.Id, user);
    //        await Clients.Others.UserIsOnline(user.Id);
    //    }
    //}

    public async Task SendMessage(string senderId, string receiverId, string content)
    {
        // Send the message to the receiver
        var messageDto = new MessageDto
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            Timestamp = DateTime.UtcNow,
            IsRead = false // Initially, messages are unread
        };

        // Send the message to the receiver
        await Clients.User(receiverId).MessageReceived(messageDto);

        // UpdateAsync the unread message count for the receiver
        if (_unreadMessagesCount.ContainsKey(receiverId))
        {
            _unreadMessagesCount[receiverId]++;
            // Send the updated unread message count to the receiver
            await Clients.User(receiverId).UpdateUnreadMessagesCount(receiverId, _unreadMessagesCount[receiverId]);
        }

        // Optionally, notify sender that the message was successfully sent
        //await Clients.Caller.MessageSentConfirmation("Message sent successfully.");
    }

    // When a user reads a message, reset their unread count
    public async Task MarkMessagesAsRead(string userId)
    {
        if (_unreadMessagesCount.ContainsKey(userId))
        {
            _unreadMessagesCount[userId] = 0; // Reset unread message count
            await Clients.User(userId).UpdateUnreadMessagesCount(userId, 0);
        }
    }

    // Handle when a user disconnects
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier; // User identifier from the connection
        if (_onlineUsers.ContainsKey(userId))
        {
            _onlineUsers.Remove(userId); // Remove user from online users
            _unreadMessagesCount.Remove(userId); // Optionally remove unread count for the user
                                                 // Notify others that the user has gone offline
            //await Clients.Others.UserIsOffline(userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}