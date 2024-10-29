namespace NeedAnalysisApp.Shared.Dto;

public class IndustryDto
{
    public required string Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }

    [DefaultValue(false)]
    public bool IsActive { get; set; }

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    public string? UniqueId { get; set; }
}
