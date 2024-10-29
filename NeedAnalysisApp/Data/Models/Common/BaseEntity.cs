namespace NeedAnalysisApp.Data.Models.Common;

public class BaseEntity
{
    public int Id { get; set; }

    public string UniqueId { get; set; } = Guid.NewGuid().ToString();

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDateUTC { get; set; } = DateTime.UtcNow;
}
