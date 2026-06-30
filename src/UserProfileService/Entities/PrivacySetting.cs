namespace UserProfileService.Entities;

public sealed class PrivacySetting
{
    public Guid UserId { get; set; }
    public string ProfileVisibility { get; set; } = "private";
    public bool ShowProgressToFriends { get; set; } = false;
    public bool AllowDataSharing { get; set; } = false;

    public UserProfile UserProfile { get; set; } = default!;
}
