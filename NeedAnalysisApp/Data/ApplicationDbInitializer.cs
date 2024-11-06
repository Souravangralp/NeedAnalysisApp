using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeedAnalysisApp.Data.Models;
using NeedAnalysisApp.Data.Models.Assessment;
using NeedAnalysisApp.Data.Models.Common;
using NeedAnalysisApp.Data.Models.Industries;
using NeedAnalysisApp.Data.Models.Scores;
using NeedAnalysisApp.Shared.Common.Utilities;

namespace NeedAnalysisApp.Data;

public class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbInitializer(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        await InitializeWithMigrationsAsync();
    }

    private async Task InitializeWithMigrationsAsync()
    {
        if (_context.Database.IsSqlServer())
        {
            await _context.Database.MigrateAsync();
        }
        else
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }

    public async Task SeedAsync()
    {
        if (!_context.Industries.Any())
        {
            var industries = new List<Industry>()
            {
                new() { Code = "100", Name = "Hospitality", Description = "Hospitality", IsActive = true, IsDeleted = false },
                new() { Code = "101", Name = "IT", Description = "IT", IsActive = true, IsDeleted = false },
                new() { Code = "102", Name = "ECommerce", Description = "ECommerce", IsActive = true, IsDeleted = false },
                new() { Code = "103", Name = "Banking", Description = "Banking", IsActive = true, IsDeleted = false }
            };

            await _context.Industries.AddRangeAsync(industries);

            await _context.SaveChangesAsync();
        }

        if (!_context.ScoreCategories.Any())
        {
            var scoreCategories = new List<ScoreCategory>()
            {
                new() { Value = "Innovation-Lagging Organization", PointsFrom = 0, PointsTo = 60, Recommendation = "Focus on building leadership alignment and improving your innovation culture by implementing leadership training programs and fostering open communication around innovation goals", IsActive = true, IsDeleted= false},
                new() { Value = "Innovation-Aware Organization", PointsFrom = 61, PointsTo = 120, Recommendation = "Yes You can do it", IsActive = true, IsDeleted= false},
                new() { Value = "Innovation-Engaged Organization", PointsFrom = 121, PointsTo = 180, Recommendation = "You have a solid foundation for innovation. Now, focus on streamlining your processes and encouraging more cross-departmental collaboration to further embed innovation", IsActive = true, IsDeleted= false},
                new() { Value = "Innovation-Driven Organization", PointsFrom = 181, PointsTo = 240, Recommendation = "", IsActive = true, IsDeleted= false},
                new() { Value = "Innovation-Integrated Leader", PointsFrom = 241, PointsTo = 300, Recommendation = "", IsActive = true, IsDeleted= false}
            };

            await _context.ScoreCategories.AddRangeAsync(scoreCategories);

            await _context.SaveChangesAsync();
        }

        if (!_context.GeneralLookUps.Any())
        {
            var generalLookUps = DatabaseSeeder.GetDemoGeneralLookups();

            await _context.GeneralLookUps.AddRangeAsync(generalLookUps);

            await _context.SaveChangesAsync();
        }

        if (!_context.Assessments.Any())
        {
            var assessments = DatabaseSeeder.GetDemoAssessments();

            await _context.Assessments.AddRangeAsync(assessments);

            await _context.SaveChangesAsync();
        }
    }

    public async Task InitializeRoles()
    {
        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // CreateAsync a default admin user
        var userCollections = DatabaseSeeder.GetDemoUserCollection();

        foreach (var userCollection in userCollections)
        {
            var user = await _userManager.FindByEmailAsync(userCollection.User.Email);
            if (user == null)
            {
                var createUser = await _userManager.CreateAsync(userCollection.User, userCollection.Password);

                if (createUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userCollection.User, userCollection.Role);
                }
            }
        }
    }
}