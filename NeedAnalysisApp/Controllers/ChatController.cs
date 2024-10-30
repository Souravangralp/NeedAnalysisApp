using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NeedAnalysisApp.Common;
using NeedAnalysisApp.Repositories.Interfaces;
using NeedAnalysisApp.Shared.Dto;
using NeedAnalysisApp.Shared.Dto.Chat;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IUserService _userService; // Service for user operations
    private readonly IMessageService _messageService; // Service for message operations
    private readonly IHubContext<ChatHub> _hubContext; // SignalR Hub context

    public ChatController(IUserService userService, IMessageService messageService, IHubContext<ChatHub> hubContext)
    {
        _userService = userService;
        _messageService = messageService;
        _hubContext = hubContext;
    }

    // Get all users
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAll("Client");
        return Ok(users);
    }

    // Send a message
    [HttpPost("messages")]
    public async Task<ActionResult<MessageDto>> SendMessage([FromBody] MessageDto messageDto)
    {
        if (messageDto == null || string.IsNullOrEmpty(messageDto.Content))
        {
            return BadRequest("Message content cannot be empty.");
        }

        var message = await _messageService.SendMessageAsync(messageDto);

        // Notify clients via SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

        return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, message);
    }

    // Upload a file
    [HttpPost("upload")]
    public async Task<ActionResult<string>> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var uploads = Path.Combine("uploads"); // Define your upload path
        if (!Directory.Exists(uploads))
        {
            Directory.CreateDirectory(uploads); // Create directory if it doesn't exist
        }

        var filePath = Path.Combine(uploads, file.FileName); // Save file with original name
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { fileUrl = filePath });
    }

    // Get messages between two users
    [HttpGet("messages/{senderId}/{receiverId}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(string senderId, string receiverId)
    {
        var messages = await _messageService.GetMessagesAsync(senderId, receiverId);
        return Ok(messages);
    }

    // Get message by ID
    [HttpGet("messages/{id}")]
    public async Task<ActionResult<MessageDto>> GetMessageById(Guid id)
    {
        var message = await _messageService.GetMessageByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }
        return Ok(message);
    }

    // Update user's online status
    //[HttpPut("users/{id}/status")]
    //public async Task<IActionResult> UpdateUserStatus(string id, [FromBody] bool isOnline)
    //{
    //    var result = await _userService.UpdateUserStatusAsync(id, isOnline);
    //    if (!result)
    //    {
    //        return NotFound();
    //    }

    //    // Notify clients via SignalR
    //    await _hubContext.Clients.All.SendAsync("UserStatusUpdated", id, isOnline);

    //    return NoContent();
    //}
}
