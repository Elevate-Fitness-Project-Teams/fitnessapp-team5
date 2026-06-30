using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.UserPlanHistory
{
    public static class UserPlanHistoryErrors
    {
        public static Error UserIdRequired => Error.Validation(nameof(UserIdRequired), "User ID is required.");
        public static Error PlanIdRequired => Error.Validation(nameof(PlanIdRequired), "Plan ID is required.");
        public static Error PlanIdTooLong => Error.Validation(nameof(PlanIdTooLong), "Plan ID must be 50 characters or less.");
        public static Error EndedAtInvalid => Error.Validation(nameof(EndedAtInvalid), "Ended at date cannot be before assigned at date.");
        public static Error ReasonForChangeRequired => Error.Validation(nameof(ReasonForChangeRequired), "Reason for change is required.");
        public static Error ReasonForChangeTooLong => Error.Validation(nameof(ReasonForChangeTooLong), "Reason for change must be 255 characters or less.");
    }
}
