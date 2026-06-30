using AuthenticationService.Common;
using AuthenticationService.Features.Commands.Reset_Password;
using AuthenticationService.Persistence.Repositories.RefreshToken;
using AuthenticationService.Persistence.Repositories.ResetToken;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using AuthenticationService.Persistence.Repositories.User;
using AuthenticationService.Services.Password;
using AuthenticationService.Services.ResetToken;
using MediatR;

namespace AuthenticationService.Features.Reset_Password
{
    public class ResetPasswordCommandHandler (IPasswordService _passwordService,
                                              IUserRepository _userRepository,
                                              IResetTokenRepository _resetTokenRepository,
                                              IRefreshTokenRepository _refreshTokenRepository,
                                              IResetTokenService _resetTokenService,
                                              IUnitOfWork _unitOfWork)
    : IRequestHandler<ResetPasswordCommand, RequestResult<ResetPasswordResponse>>
{
    public async Task<RequestResult<ResetPasswordResponse>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        // Passwords must match
        if (request.NewPassword != request.ConfirmedPassword)
        {
            return RequestResult<ResetPasswordResponse>.Failure(
                "AUTH_PASSWORD_MISMATCH",
                statusCode: StatusCodes.Status400BadRequest);
        }

        // Hash incoming token
        var tokenHash = _resetTokenService.Hash(request.ResetToken);

        // Get reset token
        var resetToken = await _resetTokenRepository.GetByHashAsync(
            tokenHash,
            cancellationToken);

        if (resetToken is null ||
            resetToken.IsUsed ||
            resetToken.ExpiresAt <= DateTime.UtcNow)
        {
            return RequestResult<ResetPasswordResponse>.Failure(
                "AUTH_RESET_TOKEN_INVALID",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var user = await _userRepository.GetByIdAsync(
            resetToken.UserId,
            cancellationToken);

        if (user is null)
        {
            return RequestResult<ResetPasswordResponse>.Failure(
                "AUTH_RESET_TOKEN_INVALID",
                statusCode: StatusCodes.Status400BadRequest);
        }

        // Update password
        user.UpdatePassword(_passwordService.Hash(request.NewPassword), resetToken.UserId);

        // Single-use token
        resetToken.MarkAsUsed();

        // Revoke all active refresh tokens
        var refreshTokens = await _refreshTokenRepository
            .GetActiveByUserIdAsync(user.Id, cancellationToken);

        foreach (var refresh in refreshTokens)
        {
            refresh.Revoke();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResult<ResetPasswordResponse>.Success(
            new ResetPasswordResponse(true),
            "Password changed successfully.");
    }
}
}       
