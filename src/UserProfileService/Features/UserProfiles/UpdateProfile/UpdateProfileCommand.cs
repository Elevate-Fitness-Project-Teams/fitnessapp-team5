using FluentValidation;
using MediatR;
using UserProfileService.Abstractions;

namespace UserProfileService.Features.UserProfiles.UpdateProfile;

public record UpdateProfileCommand(
    string FirstName,
    string LastName,
    string Phone
) : IRequest<Result>;

public class UpdateProfileCommandValidation : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidation()
    {
        RuleFor(x => x.FirstName).NotEmpty().Length(3, 100);
        RuleFor(x => x.LastName).NotEmpty().Length(3, 100);
        RuleFor(x => x.Phone).NotEmpty().Length(5, 15);
    }
}