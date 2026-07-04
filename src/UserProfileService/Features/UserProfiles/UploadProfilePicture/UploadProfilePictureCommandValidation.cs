using FluentValidation;
using UserProfileService.Abstractions.Constants;

namespace UserProfileService.Features.UserProfiles.UploadProfilePicture;

public class UploadProfilePictureCommandValidation : AbstractValidator<UploadProfilePictureCommand>
{
    public UploadProfilePictureCommandValidation()
    {
        RuleFor(x => x.Image)
             .Must((request, context) =>
             {
                 var extension = Path.GetExtension(request.Image.FileName.ToLower());
                 return FileSettings.AllowedImagesExtensions.Contains(extension);
             })
            .WithMessage("Only JPG and PNG files are allowed.")
            .When(x => x.Image is not null);

        RuleFor(x => x.Image)
            .Must(x => x.Length <= FileSettings.MaxFileSizeInBytes)
            .WithMessage("File size must not exceed 5MB.")
            .When(x => x.Image is not null);
    }
}