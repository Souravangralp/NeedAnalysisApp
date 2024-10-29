using Microsoft.AspNetCore.Identity;
using NeedAnalysisApp.Data.Models.Common;
using NeedAnalysisApp.Data.Models.Industries;

namespace NeedAnalysisApp.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? Dob { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public int? ApplicationUser_IndustryId { get; set; }

        public int? ApplicationUser_GenderId { get; set; }

        public GeneralLookUp? ApplicationUser_Gender { get; set; }

        public Industry? ApplicationUser_Industry { get; set; }
    }
}