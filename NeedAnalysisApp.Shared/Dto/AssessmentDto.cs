namespace NeedAnalysisApp.Shared.Dto;

public class AssessmentDto
{
    public string? IndustryType { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public required string Name { get; set; }
    
    public double TotalScore { get; set; }
   
    public bool IsLive { get; set; }

    [DefaultValue(false)]
    public bool IsActive { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    public string? UniqueId { get; set; }
}
