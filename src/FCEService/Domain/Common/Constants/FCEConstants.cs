namespace FCEService.Domain.Common.Constants
{
    public static class FCEConstants
    {
        public const string SystemUser = "System";

        // Validation Constraints
        public static class Validation
        {
            public const double MinWeight = 40.0;
            public const double MaxWeight = 200.0;
            public const double MinHeight = 140.0;
            public const double MaxHeight = 220.0;
            public const int MinAge = 16;
            public const int MaxAge = 100;
        }

        // Metabolic Calculation Constants
        public static class Bmr
        {
            public const double WeightMultiplier = 10.0;
            public const double HeightMultiplier = 6.25;
            public const double AgeMultiplier = 5.0;
            public const double MaleOffset = 5.0;
            public const double FemaleOffset = 161.0;
        }

        // Activity Level Multipliers
        public static class ActivityMultipliers
        {
            public const double Rookie = 1.2;
            public const double Beginner = 1.375;
            public const double Intermediate = 1.55;
            public const double Advance = 1.725;
            public const double TrueBeast = 1.9;
        }

        // Calorie Target Adjustments
        public static class CalorieAdjustments
        {
            public const double LoseWeight = -500.0;
            public const double GetFitter = 0.0;
            public const double GainWeight = 300.0;
            public const double GainMoreFlexible = 150.0;
            public const double LearnTheBasic = 0.0;
        }

        // Calorie Status Thresholds
        public static class StatusThresholds
        {
            public const double WeakMax = 1800.0;
            public const double NormalMax = 2500.0;
        }
    }
}