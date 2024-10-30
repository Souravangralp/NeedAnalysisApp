using NeedAnalysisApp.Data.Models.Common;

namespace NeedAnalysisApp.Data.Models.Chat;

public class UserSession : BaseEntity
{
    public string UserId { get; set; }
    public DateTime ConnectedAt { get; set; }
    public DateTime DisconnectedAt { get; set; }
}