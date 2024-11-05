namespace NeedAnalysisApp.Data.Models.Chat;

public class File : BaseEntity
{
    public int? File_MessageId { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public string? FileUrl { get; set; }
    public Message? File_Message { get; set; }
}
