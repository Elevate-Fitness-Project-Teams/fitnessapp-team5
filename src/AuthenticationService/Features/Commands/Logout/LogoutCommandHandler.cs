using AuthenticationService.Common;
using AuthenticationService.Persistence.Repositories.RefreshToken;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using MediatR;

namespace AuthenticationService.Features.Commands.Logout
{
    public class LogoutCommandHandler(IRefreshTokenRepository _refreshTokenRepository,
                                      IUnitOfWork _unitOfWork)
        : IRequestHandler<LogoutCommand, RequestResult<LogoutResponse>>
    {
        public async Task<RequestResult<LogoutResponse>> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(
                request.RefreshToken,
                cancellationToken);

            if (refreshToken is null || refreshToken.IsRevoked)
            {
                return RequestResult<LogoutResponse>.Failure(
                    "AUTH_TOKEN_INVALID",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            refreshToken.Revoke();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResult<LogoutResponse>.Success(
                new LogoutResponse(true));
        }
    }
}
