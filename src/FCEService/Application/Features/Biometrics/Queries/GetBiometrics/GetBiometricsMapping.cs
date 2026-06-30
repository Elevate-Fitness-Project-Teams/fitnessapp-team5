namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

using FCEService.Domain.Entities.UserFitnessStats;

public static class GetBiometricsMapping
{
    public static GetBiometricsByIdResponse ToResponse(this UserFitnessStats stats)
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
