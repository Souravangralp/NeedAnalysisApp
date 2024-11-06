namespace NeedAnalysisApp.Data.Models.Chat;

public class Message : BaseEntity
{
    public string? ApplicationUser_SenderId { get; set; }
    public string? ApplicationUser_ReceiverId { get; set; }
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public DateTime? SentOn { get; set; }
    public bool IsRead { get; set; }
    public File? File { get; set; }

    public ApplicationUser? ApplicationUser_Sender { get; set; }
    public ApplicationUser? ApplicationUser_Receiver { get; set; }
}