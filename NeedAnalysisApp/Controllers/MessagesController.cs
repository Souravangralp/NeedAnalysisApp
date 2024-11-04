using Microsoft.AspNetCore.Mvc;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Common;
using NeedAnalysisApp.Shared.Dto.Chat;

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
}
