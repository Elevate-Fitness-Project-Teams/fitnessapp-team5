namespace UserProfileService.Contracts.UserProfiles;

public record UploadPictureRequest(
    IFormFile Image
);