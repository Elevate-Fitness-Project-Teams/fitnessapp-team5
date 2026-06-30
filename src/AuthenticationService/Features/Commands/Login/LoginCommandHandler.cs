using AuthenticationService.Common;
using AuthenticationService.Persistence.Repositories.RefreshToken;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using AuthenticationService.Persistence.Repositories.User;
using AuthenticationService.Services.Jwt;
using AuthenticationService.Services.Password;
using AuthenticationService.Services.RefreshToken;
using MediatR;

namespace AuthenticationService.Features.Commands.Login
{
    public class LoginCommandHandler(IUserRepository _userRepository,
                                     IPasswordService _passwordService,
                                     IJwtService _jwtService,
                                     IRefreshTokenService _refreshTokenService,
                                     IRefreshTokenRepository _refreshTokenRepository,
                                     IUnitOfWork _unitOfWork)
        : IRequestHandler<LoginCommand, RequestResult<LoginResponse>>
    {
        public async Task<RequestResult<LoginResponse>> Handle(LoginCommand request,
                                                         CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByEmailAsync(
                request.Email, cancellationToken);

            if (user is null || !_passwordService.Verify(
                request.Password, user.PasswordHash))
            {
                return RequestResult<LoginResponse>.Failure(
                    "Invalid email or password.", statusCode: StatusCodes.Status401Unauthorized);
            }

            var accessToken = _jwtService.GenerateAccessToken(user);

            var refreshToken = _refreshTokenService.Generate(user);

            _refreshTokenRepository.Add(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return RequestResult<LoginResponse>.Success(
                new LoginResponse
                (
                    accessToken,
                    refreshToken.Token,
                    user.ProfileCompleted,
                    user.IsPremium
                ));
        }
    }
}