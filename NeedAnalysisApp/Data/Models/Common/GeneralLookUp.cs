namespace NeedAnalysisApp.Data.Models.Common;

public class GeneralLookUp : BaseEntity
{
    public string? Type { get; set; }
    public string? Value { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}
