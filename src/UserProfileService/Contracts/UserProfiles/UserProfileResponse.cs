namespace UserProfileService.Contracts.UserProfiles;

public record UserProfileResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string ProfilePictureUrl
);
