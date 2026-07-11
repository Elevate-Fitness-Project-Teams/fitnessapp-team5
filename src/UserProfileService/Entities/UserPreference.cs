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

    private UserPreference() { }

    public static UserPreference CreateStub(Guid userId) => new() { UserId = userId };

    public IReadOnlyCollection<string> Update(
        string? language, string? theme, string? weightUnit, string? heightUnit, string? distanceUnit)
    {
        var changedProperties = new List<string>();

        if (!string.IsNullOrWhiteSpace(language) && Language != language)
        {
            Language = language;
            changedProperties.Add(nameof(Language));
        }

        if (!string.IsNullOrWhiteSpace(theme) && Theme != theme)
        {
            Theme = theme;
            changedProperties.Add(nameof(Theme));
        }

        if (!string.IsNullOrWhiteSpace(weightUnit) && WeightUnit != weightUnit)
        {
            WeightUnit = weightUnit;
            changedProperties.Add(nameof(WeightUnit));
        }

        if (!string.IsNullOrWhiteSpace(heightUnit) && HeightUnit != heightUnit)
        {
            HeightUnit = heightUnit;
            changedProperties.Add(nameof(HeightUnit));
        }

        if (!string.IsNullOrWhiteSpace(distanceUnit) && DistanceUnit != distanceUnit)
        {
            DistanceUnit = distanceUnit;
            changedProperties.Add(nameof(DistanceUnit));
        }

        return changedProperties;
    }
}
