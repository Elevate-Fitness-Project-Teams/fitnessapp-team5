using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.UserAssignedPlan
{
    public static class UserAssignedPlanErrors
    {
        public static Error UserIdRequired => Error.Validation(nameof(UserIdRequired), "User ID is required.");
        public static Error PlanIdRequired => Error.Validation(nameof(PlanIdRequired), "Plan ID is required.");
        public static Error NotFound => Error.NotFound(nameof(NotFound), "No active assigned plan was found for this user.");
    }
}
