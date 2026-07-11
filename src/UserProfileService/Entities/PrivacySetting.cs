namespace UserProfileService.Entities;

public sealed class PrivacySetting
{
    public Guid UserId { get; set; }
    public string ProfileVisibility { get; set; } = "private";
    public bool ShowProgressToFriends { get; set; } = false;
    public bool AllowDataSharing { get; set; } = false;

    public UserProfile UserProfile { get; set; } = default!;

    private PrivacySetting() { }

    public static PrivacySetting CreateStub(Guid userId) => new() { UserId = userId };

    public IReadOnlyCollection<string> Update(string? profileVisibility, bool? showProgressToFriends, bool? allowDataSharing)
    {
        var changedProperties = new List<string>();

        if (!string.IsNullOrWhiteSpace(profileVisibility) && ProfileVisibility != profileVisibility)
        {
            ProfileVisibility = profileVisibility;
            changedProperties.Add(nameof(ProfileVisibility));
        }

        if (showProgressToFriends.HasValue && ShowProgressToFriends != showProgressToFriends.Value)
        {
            ShowProgressToFriends = showProgressToFriends.Value;
            changedProperties.Add(nameof(ShowProgressToFriends));
        }

        if (allowDataSharing.HasValue && AllowDataSharing != allowDataSharing.Value)
        {
            AllowDataSharing = allowDataSharing.Value;
            changedProperties.Add(nameof(AllowDataSharing));
        }

        return changedProperties;
    }
}
