using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NeedAnalysisApp.Data;
using NeedAnalysisApp.Hubs;
using NeedAnalysisApp.Shared;
using NeedAnalysisApp.Shared.Dto.Chat;

namespace BlazingChat.Server.Controllers
{
    //[Authorize]
    [ApiController]
    public class MessagesController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub, IBlazingChatHubClient> _hubContext;

        public MessagesController(ApplicationDbContext context, IHubContext<ChatHub, IBlazingChatHubClient> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("api/messages")]
        public async Task<IActionResult> SendMessage(MessageDto messageDto, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(messageDto.ReceiverId) || !string.IsNullOrWhiteSpace(messageDto.Content))
                return new NotFoundResult();

            var message = new Message
            {
                ApplicationUser_SenderId = messageDto.SenderId,
                ApplicationUser_ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content,
                SentOn = DateTime.Now
            };

            await _context.Messages.AddAsync(message, cancellationToken);

            if (await _context.SaveChangesAsync(cancellationToken) > 0)
            {
                var responseMessageDto = new MessageDto()
                {
                    ReceiverId = message.ApplicationUser_ReceiverId,
                    SenderId = message.ApplicationUser_SenderId,
                    Content = message.Content,
                    Timestamp = message.SentOn
                };

                await _hubContext.Clients.User(messageDto.ReceiverId)
                            .MessageRecieved(responseMessageDto);
                return new OkResult();
            }
            else
            {
                return new NotFoundResult();
            }

        }

        [HttpGet("api/messages/{otherUserId}")]
        public async Task<IEnumerable<MessageDto>> GetMessages(string otherUserId, CancellationToken cancellationToken)
        {
            var messages = await _context.Messages
                            .AsNoTracking()
                            .Where(m =>
                                (m.ApplicationUser_SenderId == otherUserId && m.ApplicationUser_ReceiverId == "")
                                || (m.ApplicationUser_ReceiverId == otherUserId && m.ApplicationUser_SenderId == "UserId")
                            )
                            .Select(m => new MessageDto()
                            {
                                ReceiverId = m.ApplicationUser_ReceiverId,
                                SenderId = m.ApplicationUser_SenderId,
                                Content = m.Content,
                                Timestamp = m.SentOn,
                            })
                            .ToListAsync(cancellationToken);

            return messages;
        }
    }
}
