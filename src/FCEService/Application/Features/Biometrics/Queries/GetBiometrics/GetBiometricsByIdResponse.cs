namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

public class GetBiometricsByIdResponse
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
