using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeedAnalysisApp.Data.Models;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Data.Models.Common;
using NeedAnalysisApp.Data.Models.Industries;
using NeedAnalysisApp.Data.Models.Scores;

namespace NeedAnalysisApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Industry> Industries { get; set; }

        public DbSet<Assessment> Assessments { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<GeneralLookUp> GeneralLookUps { get; set; }

        public DbSet<ScoreCategory> ScoreCategories { get; set; }

        public DbSet<UserAssessmentMapper> UserAssessmentMapper { get; set; }
    }
}
