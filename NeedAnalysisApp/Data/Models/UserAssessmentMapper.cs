using NeedAnalysisApp.Data.Models.Common;

namespace NeedAnalysisApp.Data.Models;

public class UserAssessmentMapper : BaseEntity
{
    public required string UserAssessmentMapper_UserId { get; set; }

    public int? UserAssessmentMapper_AssessmentId { get; set; }

    public int? GeneralLookUp_AssessmentStatusTypeId { get; set; }

    public GeneralLookUp? GeneralLookUp_AssessmentStatusType { get; set; }
}
