using UserProfileService.Abstractions;

namespace UserProfileService.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =
        new("User.NotFound", $"user with id was not found.", StatusCodes.Status404NotFound);

    public static readonly Error InvalidToken =
        new("User.InvalidToken", $"Invalid Token.", StatusCodes.Status401Unauthorized);

    public static readonly Error EmailIsExists =
        new("User.EmailIsExists", $"You entered email is already exists.", StatusCodes.Status409Conflict);

    public static readonly Error UserPicture =
        new("User.UserPicture", $"Failed to upload image.", StatusCodes.Status400BadRequest);

    public static readonly Error UpdateSettingFailed =
        new("User.UpdateSettingFailed", $"Failed to update user setting.", StatusCodes.Status400BadRequest);
}
