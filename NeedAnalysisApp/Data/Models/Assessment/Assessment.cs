using NeedAnalysisApp.Data.Models.Common;
using NeedAnalysisApp.Data.Models.Industries;

namespace NeedAnalysisApp.Data.Models.Assessment;

public class Assessment : BaseEntity
{
    public int? Assessment_IndustryID { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double? TotalScore { get; set; }

    public bool IsLive { get; set; }

    public Industry? Assessment_Industry { get; set; }

    public List<Question> Questions { get; set; } = [];
}
