using AuthenticationService.Common;
using AuthenticationService.Entities;
using AuthenticationService.Persistence.Repositories.OtpCode;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using AuthenticationService.Persistence.Repositories.User;
using AuthenticationService.Services.Email;
using AuthenticationService.Services.OtpCode;
using MediatR;

namespace AuthenticationService.Features.Forgot_Password
{
    public class ForgotPasswordCommandHandler(IUserRepository _userRepository,
                                              IOtpCodeRepository _otpCodeRepository,
                                              IOtpCodeService _otpCodeService,
                                              IEmailService _emailService,
                                              IUnitOfWork _unitOfWork) 
        : IRequestHandler<ForgotPasswordCommand, RequestResult<ForgotPasswordResponse>>
    {
        public async Task<RequestResult<ForgotPasswordResponse>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(
                                             request.Email,
                                             cancellationToken);

            //Because User Enumeration Attack
            if (user is null)
            {
                return RequestResult<ForgotPasswordResponse>.Success(
                    new ForgotPasswordResponse(
                        OtpExpiresIn: 600,
                        CanResendIn: 30));
            }

            var lastOtp = await _otpCodeRepository.GetLatestByUserIdAsync(
                user.Id,
                cancellationToken);

            if (lastOtp is not null &&
                lastOtp.CreatedAt.AddSeconds(30) > DateTime.UtcNow)
            {
                return RequestResult<ForgotPasswordResponse>.Failure(
                    "RATE_OTP_RESEND_TOO_SOON",
                    statusCode: StatusCodes.Status429TooManyRequests);
            }

            var otp = _otpCodeService.GenerateCode(); // ex: 541920

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(otp);
            Console.ResetColor();

            var otpHash = _otpCodeService.Hash(otp);

            var otpEntity = new OtpCode
            {
                Id = Guid.CreateVersion7(),
                UserId = user.Id,
                CodeHash = otpHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _otpCodeRepository.Add(otpEntity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

           // await _emailService.SendOtpAsync(user.Email, otp);

            return RequestResult<ForgotPasswordResponse>.Success(
                new ForgotPasswordResponse(
                    OtpExpiresIn: 600,
                    CanResendIn: 30),
                "OTP sent successfully.");
        }

    }
}
