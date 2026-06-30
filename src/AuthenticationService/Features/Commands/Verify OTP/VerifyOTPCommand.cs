using AuthenticationService.Common;
using MediatR;

namespace AuthenticationService.Features.Verify_OTP
{
    public sealed record VerifyOTPCommand(string Email, string Otp) 
        : IRequest<RequestResult<VerifyOTPResponse>>;
}
