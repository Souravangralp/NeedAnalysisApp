namespace NeedAnalysisApp.Shared.Dto;

public class ScoreCategoryDto
{
    public string Value { get; set; }

    public double PointsFrom { get; set; }

    public double PointsTo { get; set; }

    [DefaultValue("")]
    public string? UniqueId { get; set; }

    public string? Recommendation { get; set; }

    [DefaultValue(false)]
    public bool IsActive { get; set; }
}
