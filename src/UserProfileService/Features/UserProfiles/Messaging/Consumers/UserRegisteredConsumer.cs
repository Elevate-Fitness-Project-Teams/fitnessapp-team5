using Bulk.Shared.Contracts.Events;
using Mapster;
using MassTransit;
using MediatR;
using UserProfileService.Features.UserProfiles.CreateUserProfile;

namespace UserProfileService.Features.UserProfiles.Messaging.Consumers;

public class UserRegisteredConsumer(ISender sender) : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var message = context.Message;

        var command = message.Adapt<CreateUserProfileCommand>();

        await sender.Send(command);
    }
}
