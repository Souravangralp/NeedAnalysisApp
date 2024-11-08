namespace NeedAnalysisApp.Controllers;

[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("api/messages")]
    public async Task<Result> SendMessage(MessageDto messageDto)
    {
        return await _messageService.SendMessageAsync(messageDto);
    }

    [HttpGet("api/messages")]
    public async Task<List<MessageDto>> GetMessages(string senderId, string receiverId)
    {
        return await _messageService.GetMessages(senderId, receiverId);
    }

    [HttpPost("api/messages/{messageId}/markRead/senderId:{senderId}/receiverId:{receiverId}")]
    public async Task<bool> MarkRead(string senderId, string receiverId, string messageId)
    {
        return await _messageService.MarkReadAsync(senderId, receiverId, messageId);
    }
}