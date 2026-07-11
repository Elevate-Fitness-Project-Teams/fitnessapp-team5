namespace UserProfileService.Contracts.UserProfiles;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string Phone
);
