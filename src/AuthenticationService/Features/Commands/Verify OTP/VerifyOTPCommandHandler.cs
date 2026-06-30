using AuthenticationService.Common;
using AuthenticationService.Entities;
using AuthenticationService.Persistence.Repositories.OtpCode;
using AuthenticationService.Persistence.Repositories.ResetToken;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using AuthenticationService.Persistence.Repositories.User;
using AuthenticationService.Services.OtpCode;
using AuthenticationService.Services.ResetToken;
using MediatR;

namespace AuthenticationService.Features.Verify_OTP
{
    public class VerifyOTPCommandHandler(IUserRepository _userRepository,
                                         IOtpCodeRepository _otpCodeRepository,
                                         IOtpCodeService _otpCodeService,
                                         IResetTokenService _resetTokenService,
                                         IResetTokenRepository _resetTokenRepository,
                                         IUnitOfWork _unitOfWork)
    : IRequestHandler<VerifyOTPCommand, RequestResult<VerifyOTPResponse>>
    {
        public async Task<RequestResult<VerifyOTPResponse>> Handle(
            VerifyOTPCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);

            if (user is null)
            {
                return RequestResult<VerifyOTPResponse>.Failure(
                    "AUTH_INVALID_OTP",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var otp = await _otpCodeRepository.GetLatestUnusedByUserIdAsync(
                user.Id,
                cancellationToken);

            if (otp is null)
            {
                return RequestResult<VerifyOTPResponse>.Failure(
                    "AUTH_INVALID_OTP",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (otp.ExpiresAt <= DateTime.UtcNow)
            {
                return RequestResult<VerifyOTPResponse>.Failure(
                    "AUTH_OTP_EXPIRED",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var isValid = _otpCodeService.Verify(request.Otp, otp.CodeHash);

            if (!isValid)
            {
                return RequestResult<VerifyOTPResponse>.Failure(
                    "AUTH_INVALID_OTP",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            otp.MarkAsUsed();

            // Valid only for the Reset Password endpoint.
            // Cannot be used as an Access Token or Refresh Token.
            var resetToken = _resetTokenService.GenerateToken();

            var resetTokenEntity = ResetToken.Create(
                user.Id,
                _resetTokenService.Hash(resetToken),
                DateTime.UtcNow.AddMinutes(10));

            _resetTokenRepository.Add(resetTokenEntity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResult<VerifyOTPResponse>.Success(
                new VerifyOTPResponse(resetToken),
                "OTP verified successfully.");
        }
    }
}