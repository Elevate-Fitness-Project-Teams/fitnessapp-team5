using Mapster;
using MassTransit;
using MediatR;
using UserProfileService.Features.UserProfiles.CreateUserProfile;
using UserProfileService.Features.UserProfiles.Messaging.Events;

namespace UserProfileService.Features.UserProfiles.Messaging.Consumers;

public class UserRegisteredConsumer(ISender sender) : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var message = context.Message;

        //var command = new CreateUserProfileCommand(
        //    message.UserId,
        //    message.Email,
        //    message.FirstName,
        //    message.LastName,
        //    message.PhoneNumber);

        var command = message.Adapt<CreateUserProfileCommand>();

        await sender.Send(command);
    }
}
