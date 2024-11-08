using Microsoft.AspNetCore.Authorization;
using System.Collections.Concurrent;

namespace NeedAnalysisApp.Hubs;

/// <summary>
/// The ChatHub class handles real-time communication for the chat functionality,
/// including user connections, message sending, and unread message tracking.
/// </summary>
[Authorize]
public class ChatHub : Hub<IBlazingChatHubClient>, IBlazingChatHubServer
{
    #region Fields

    // Thread-safe collection for online users and unread message counts
    private static readonly Dictionary<string, UserDto> _onlineUsers = new Dictionary<string, UserDto>();
    private static readonly Dictionary<string, int> _unreadMessagesCount = new Dictionary<string, int>();

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatHub"/> class.
    /// </summary>
    public ChatHub()
    {

    }

    #endregion

    #region Methods

    /// <summary>
    /// Handles the logic when a new user connects to the chat hub.
    /// </summary>
    public override Task OnConnectedAsync()
    {
        var onlineUsers = _onlineUsers.Values;

        Clients.Others.OnlineUsersList(onlineUsers);

        return base.OnConnectedAsync();
    }

    /// <summary>
    /// Sets a user as online and informs other clients about the user's status.
    /// </summary>
    /// <param name="user">The user who is going online.</param>
    public async Task SetUserOnline(UserDto user)
    {
        await Clients.Caller.OnlineUsersList(_onlineUsers.Values);

        if (!_onlineUsers.ContainsKey(user.Id))
        {
            _onlineUsers.Add(user.Id, user);

            await Clients.Others.UserIsOnline(user.Id);
        }

        if (!_unreadMessagesCount.ContainsKey(user.Id))
        {
            _unreadMessagesCount.Add(user.Id, 0);
        }
    }

    /// <summary>
    /// Sends a message from one user to another.
    /// </summary>
    /// <param name="senderId">The ID of the user sending the message.</param>
    /// <param name="receiverId">The ID of the user receiving the message.</param>
    /// <param name="content">The content of the message.</param>
    public async Task SendMessage(string senderId, string receiverId, string content)
    {
        var messageDto = GetMessage(senderId, receiverId, content);

        // Send the message to the receiver
        await Clients.User(receiverId).MessageReceived(messageDto);

        if (_unreadMessagesCount.ContainsKey(receiverId))
        {
            _unreadMessagesCount[receiverId]++;

            await Clients.User(senderId).UpdateUnreadMessagesCount(senderId, receiverId, _unreadMessagesCount[receiverId]);
        }
    }

    /// <summary>
    /// Creates a message DTO from the sender, receiver, and content.
    /// </summary>
    /// <param name="senderId">The sender's user ID.</param>
    /// <param name="receiverId">The receiver's user ID.</param>
    /// <param name="content">The message content.</param>
    /// <returns>A new instance of <see cref="MessageDto"/>.</returns>
    private static MessageDto GetMessage(string senderId, string receiverId, string content)
    {
        // do use auto mapper for this
        return new MessageDto
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };
    }

    /// <summary>
    /// Marks all messages from a specific sender to a receiver as read and resets the unread message count.
    /// </summary>
    /// <param name="senderId">The sender's user ID.</param>
    /// <param name="receiverId">The receiver's user ID.</param>
    public async Task MarkMessagesAsRead(string senderId, string receiverId)
    {
        // Reset unread message count for the receiver
        if (_unreadMessagesCount.ContainsKey(receiverId))
        {
            _unreadMessagesCount[receiverId] = 0;

            // Notify the user of the updated unread message count
            await Clients.User(receiverId).UpdateUnreadMessagesCount(senderId, receiverId, 0);
        }
    }

    /// <summary>
    /// Handles the logic when a user disconnects from the chat hub.
    /// </summary>
    /// <param name="exception">An optional exception that occurred during the disconnection.</param>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null && _onlineUsers.ContainsKey(userId))
        {
            _onlineUsers.Remove(userId, out _);
            _unreadMessagesCount.Remove(userId, out _);

            // Notify others that the user has disconnected
            await Clients.Others.UserIsOffline(userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    #endregion
}