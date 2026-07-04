using MediatR;
using UserProfileService.Abstractions;

namespace UserProfileService.Features.UserSettings.UpdateSetting;   

public record UpdatePreferencesCommand(
    string Language,
    string Theme,
    string WeightUnit,
    string HeightUnit,
    string DistanceUnit
) : IRequest<Result>;
