using FluentValidation;

namespace SmartCoachService.Features.GetChatHistory
{
    public class GetChatHistoryValidator : AbstractValidator<GetChatHistoryRequest>
    {
        public GetChatHistoryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must be between 1 and 100.");
        }
    }
}
