namespace NeedAnalysisApp.Shared.Dto;

public class OptionDto
{
    [DefaultValue("")]
    public string? UniqueId { get; set; }

    public int DisplayOrder { get; set; }

    [DefaultValue("")]
    public string? Value { get; set; }

    [DefaultValue(false)]
    public bool ISCorrect { get; set; }

    public double Point { get; set; }
}
