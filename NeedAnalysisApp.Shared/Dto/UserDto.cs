namespace NeedAnalysisApp.Shared.Dto;

public class UserDto
{
    public string Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public DateTime? Dob { get; set; }

    public string? IndustryType { get; set; }

    public string? GenderType { get; set; }

    [DefaultValue(false)]
    public bool IsOnline { get; set; }
}
