using AuthenticationService.Common;
using AuthenticationService.Entities;
using AuthenticationService.Persistence.Repositories.UnitOfWork;
using AuthenticationService.Persistence.Repositories.User;
using AuthenticationService.Services.Password;
using MediatR;

namespace AuthenticationService.Features.Commands.Register
{
    public class RegisterCommandHandler(IUserRepository _userRepository,
                                        IPasswordService _passwordService,
                                        IUnitOfWork _unitOfWork) 
        : IRequestHandler<RegisterCommand, RequestResult<RegisterResponse>>
    {
        public async Task<RequestResult<RegisterResponse>> Handle(
              RegisterCommand request,
              CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);

            if (existingUser is not null)
            {
                return RequestResult<RegisterResponse>.Failure(
                    message: "Email already registered.",
                    errors: new[]
                    {
                    "AUTH_EMAIL_EXISTS"
                    },
                    statusCode: StatusCodes.Status409Conflict);
            }

            // Hash password
            var passwordHash = _passwordService.Hash(request.Password);

            // Create User
            var user = new User
            {
                Id = Guid.CreateVersion7(),

                FirstName = request.FirstName,

                LastName = request.LastName,

                Email = request.Email.Trim().ToLowerInvariant(),

                PasswordHash = passwordHash,

                PhoneNumber = request.PhoneNumber
            };

            // Save
            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Response
            return RequestResult<RegisterResponse>.Success(
                new RegisterResponse(
                    user.Id,
                    true),
                "User registered successfully.");
        }
    }
}
