namespace NeedAnalysisApp.Shared.Dto.Chat;

public class UserChatDto
{
    public UserDto User { get; set; }
    public List<MessageDto> Messages { get; set; }
}
