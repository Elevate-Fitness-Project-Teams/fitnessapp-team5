using AuthenticationService.Common;
using AuthenticationService.Persistence.Repositories.RefreshToken;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using AuthenticationService.Persistence.Repositories.User;
using AuthenticationService.Services.Jwt;
using AuthenticationService.Services.RefreshToken;
using MediatR;

namespace AuthenticationService.Features.Refresh_Token
{
    public sealed class RefreshTokenCommandHandler(
        IRefreshTokenRepository _refreshTokenRepository,
        IUserRepository _userRepository,
        IJwtService _jwtService,
        IRefreshTokenService _refreshTokenService,
        IUnitOfWork _unitOfWork)
        : IRequestHandler<RefreshTokenCommand, RequestResult<RefreshTokenResponse>>
    {
        public async Task<RequestResult<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(
                request.RefreshToken,
                cancellationToken);

            if (refreshToken is null || refreshToken.RevokedAt is not null)
            {
                return RequestResult<RefreshTokenResponse>.Failure(
                    "AUTH_TOKEN_INVALID",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                return RequestResult<RefreshTokenResponse>.Failure(
                    "AUTH_TOKEN_EXPIRED",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var user = await _userRepository.GetByIdAsync(
                refreshToken.UserId,
                cancellationToken);

            if (user is null)
            {
                return RequestResult<RefreshTokenResponse>.Failure(
                    "AUTH_TOKEN_INVALID",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            // Revoke old refresh token
            refreshToken.Revoke();

            // Generate new access token
            var accessToken = _jwtService.GenerateAccessToken(user);

            // Generate new refresh token
            var newRefreshToken = _refreshTokenService.Generate(user);

            _refreshTokenRepository.Add(newRefreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RequestResult<RefreshTokenResponse>.Success(
                new RefreshTokenResponse(
                    accessToken,
                    newRefreshToken.Token),
                "Token refreshed successfully.");
        }
    }}
