using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.FitnessPlanConfig
{
    public static class FitnessPlanConfigErrors
    {
        public static Error UserFitnessStatsAlreadyExists(int userId) => Error.Validation("UserFitnessStatsAlreadyExists", $"UserFitnessStats for this UserId:{userId} already exists");
        public static Error PlanIdRequired => Error.Validation(nameof(PlanIdRequired), "Plan ID is required.");
        public static Error PlanIdTooLong => Error.Validation(nameof(PlanIdTooLong), "Plan ID must be 50 characters or less.");
        public static Error PlanNameRequired => Error.Validation(nameof(PlanNameRequired), "Plan name is required.");
        public static Error PlanNameTooLong => Error.Validation(nameof(PlanNameTooLong), "Plan name must be 100 characters or less.");
        public static Error GoalInvalid => Error.Validation(nameof(GoalInvalid), "Goal is invalid.");
        public static Error StatusInvalid => Error.Validation(nameof(StatusInvalid), "Status must be Weak, Normal, or Hard.");
        public static Error MinCalorieInvalid => Error.Validation(nameof(MinCalorieInvalid), "Minimum calorie must be greater than or equal to zero.");
        public static Error MaxCalorieInvalid => Error.Validation(nameof(MaxCalorieInvalid), "Maximum calorie must be greater than or equal to minimum calorie.");
        public static Error WorkoutsPerWeekInvalid => Error.Validation(nameof(WorkoutsPerWeekInvalid), "Workouts per week must be greater than zero.");
    }
}
