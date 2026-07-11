namespace UserProfileService.Entities;

public sealed class UserProfile
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public bool IsPremiumCached { get; set; }
    public DateTime MemberSince { get; set; } = DateTime.UtcNow;

    public UserPreference Preference { get; set; } = null!;
    public NotificationSetting NotificationSetting { get; set; } = null!;
    public PrivacySetting PrivacySetting { get; set; } = null!;

    private UserProfile() { }

    public static UserProfile CreateStub(Guid userId) => new() { UserId = userId };

    public static UserProfile Create(Guid userId, string firstName, string lastName, string email, string phone) =>
        new()
        { UserId = userId, FirstName = firstName, LastName = lastName, Email = email, Phone = phone };

    public void Update(string? firstName, string? lastName, string? phone)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;

        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;

        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;
    }
}
