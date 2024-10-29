using NeedAnalysisApp.Data.Models.Common;

namespace NeedAnalysisApp.Data.Models.Assessment;

public class Option : BaseEntity
{
    public int? Option_QuestionID { get; set; }

    public string? Value { get; set; }

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public double Point { get; set; }

    public bool ISInCorrectMatch { get; set; }

    public Question? Option_Question { get; set; }
}