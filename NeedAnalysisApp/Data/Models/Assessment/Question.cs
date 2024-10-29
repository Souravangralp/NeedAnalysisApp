using NeedAnalysisApp.Data.Models.Common;

namespace NeedAnalysisApp.Data.Models.Assessment;

public class Question : BaseEntity
{
    public int? Question_AssessmentId { get; set; }

    public int? GeneralLookUp_QuestionTypeId { get; set; }

    public int? GeneralLookUp_SectionTypeId { get; set; }

    public string? Value { get; set; }

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public List<Option> Options { get; set; } = [];

    public GeneralLookUp? GeneralLookUp_QuestionType { get; set; }

    public GeneralLookUp? GeneralLookUp_SectionType { get; set; }

    public Assessment? Question_Assessment { get; set; }
}
