﻿namespace NeedAnalysisApp.Shared.Dto.Chat;

public class MessageDto
{
    public string UniqueId { get; set; } = "";
    public string SenderId { get; set; } = "";
    public string ReceiverId { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime? Timestamp { get; set; }
    public bool IsRead { get; set; }
    public FileDto? File { get; set; } = null;
}