namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

public sealed class GetBiometricsByIdResponse
{
    public Guid UserId { get; init; }
    public double Weight { get; init; }
    public double Height { get; init; }
    public DateTime BirthDate { get; init; }
    public FCEService.Domain.Enums.Gender Gender { get; init; }
    public FCEService.Domain.Enums.Goal Goal { get; init; }
    public FCEService.Domain.Enums.ActivityLevel ActivityLevel { get; init; }
    public DateTimeOffset? LastModified { get; init; }
}

public static class GetBiometricsMapping
{
    public static GetBiometricsByIdResponse ToResponse(this FCEService.Domain.Entities.UserFitnessStats.UserFitnessStats stats)
    {
        return new GetBiometricsByIdResponse
        {
            UserId = stats.UserId,
            Weight = stats.Weight,
            Height = stats.Height,
            BirthDate = stats.BirthDate,
            Gender = stats.Gender,
            Goal = stats.Goal,
            ActivityLevel = stats.ActivityLevel,
            LastModified = stats.LastModifiedUtc
        };
    }
}
