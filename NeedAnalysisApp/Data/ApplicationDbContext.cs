namespace NeedAnalysisApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Industry> Industries { get; set; }

    public DbSet<Assessment> Assessments { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<Option> Options { get; set; }

    public DbSet<GeneralLookUp> GeneralLookUps { get; set; }

    public DbSet<ScoreCategory> ScoreCategories { get; set; }

    public DbSet<UserAssessmentMapper> UserAssessmentMapper { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Models.Chat.File> Files { get; set; }

    public DbSet<UserSession> UserSessions { get; set; }
}
