using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.CalculatedMetrics
{
    public static class CalculatedMetricsErrors
    {
        public static Error UserIdRequired => Error.Validation(nameof(UserIdRequired), "User ID is required.");
        public static Error BmrInvalid => Error.Validation(nameof(BmrInvalid), "BMR must be greater than zero.");
        public static Error TdeeInvalid => Error.Validation(nameof(TdeeInvalid), "TDEE must be greater than zero.");
        public static Error StatusInvalid => Error.Validation(nameof(StatusInvalid), "Status must be Weak, Normal, or Hard.");
        public static Error NullStats => Error.Validation(nameof(NullStats), "User fitness stats cannot be null.");
        public static Error GenderInvalid => Error.Validation(nameof(GenderInvalid), "Gender is invalid.");
        public static Error ActivityLevelInvalid => Error.Validation(nameof(ActivityLevelInvalid), "Activity level is invalid.");
        public static Error GoalInvalid => Error.Validation(nameof(GoalInvalid), "Goal is invalid.");
    }
}
