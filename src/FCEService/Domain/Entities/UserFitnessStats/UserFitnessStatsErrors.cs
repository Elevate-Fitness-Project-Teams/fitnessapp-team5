using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.UserFitnessStats
{
    public static class UserFitnessStatsErrors
    {
        public static Error UserIdRequired => Error.Validation(nameof(UserIdRequired), "User ID is required.");
        public static Error WeightInvalid => Error.Validation(nameof(WeightInvalid), "Weight must be between 40 and 200 kg.");
        public static Error HeightInvalid => Error.Validation(nameof(HeightInvalid), "Height must be between 140 and 220 cm.");
        public static Error AgeInvalid => Error.Validation(nameof(AgeInvalid), "Age must be between 16 and 100 years.");
        public static Error BirthDateRequired => Error.Validation(nameof(BirthDateRequired), "Birth date is required.");
        public static Error GenderInvalid => Error.Validation(nameof(GenderInvalid), "Gender must be a valid option (Male or Female).");
        public static Error GoalInvalid => Error.Validation(nameof(GoalInvalid), "Goal must be a valid option.");
        public static Error ActivityLevelInvalid => Error.Validation(nameof(ActivityLevelInvalid), "Activity level must be a valid option.");
        public static Error UserAlreadyExists => Error.Conflict(nameof(UserAlreadyExists), "UserFitnessStats for this user already exists.");
        public static Error UserNotFound => Error.NotFound(nameof(UserNotFound), "UserFitnessStats for this user was not found.");
    }
}
