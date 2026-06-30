namespace UserProfileService.Entities;

public sealed class UserPreference
{
    public Guid UserId { get; set; }
    public string Language { get; set; } = "en";
    public string Theme { get; set; } = "light";
    public string WeightUnit { get; set; } = "kg";
    public string HeightUnit { get; set; } = "cm";
    public string DistanceUnit { get; set; } = "km";

    public UserProfile UserProfile { get; set; } = default!;
}
