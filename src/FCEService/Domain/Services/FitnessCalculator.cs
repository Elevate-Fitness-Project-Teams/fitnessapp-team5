using FCEService.Domain.Common.Constants;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Services;

public static class FitnessCalculator
{
    // Dictionary replaces the switch — adding a new ActivityLevel requires only one line here
    private static readonly Dictionary<ActivityLevel, double> _activityMultipliers = new()
    {
        { ActivityLevel.Rookie,       FCEConstants.ActivityMultipliers.Rookie },
        { ActivityLevel.Beginner,     FCEConstants.ActivityMultipliers.Beginner },
        { ActivityLevel.Intermediate, FCEConstants.ActivityMultipliers.Intermediate },
        { ActivityLevel.Advance,      FCEConstants.ActivityMultipliers.Advance },
        { ActivityLevel.TrueBeast,    FCEConstants.ActivityMultipliers.TrueBeast },
    };

    private static readonly Dictionary<Goal, double> _calorieAdjustments = new()
    {
        { Goal.LoseWeight, FCEConstants.CalorieAdjustments.LoseWeight },
        { Goal.GetFitter,  FCEConstants.CalorieAdjustments.GetFitter },
        { Goal.GainWeight, FCEConstants.CalorieAdjustments.GainWeight },
    };

    public static double CalculateBmr(double weight, double height, int age, Gender gender)
    {
        if (gender == Gender.Male)
        {
            return (FCEConstants.Bmr.WeightMultiplier * weight)
                + (FCEConstants.Bmr.HeightMultiplier * height)
                - (FCEConstants.Bmr.AgeMultiplier * age)
                + FCEConstants.Bmr.MaleOffset;
        }
        else
        {
            return (FCEConstants.Bmr.WeightMultiplier * weight)
                + (FCEConstants.Bmr.HeightMultiplier * height)
                - (FCEConstants.Bmr.AgeMultiplier * age)
                - FCEConstants.Bmr.FemaleOffset;
        }
    }

    public static Result<double> CalculateTdee(double bmr, ActivityLevel activityLevel)
    {
        return _activityMultipliers.TryGetValue(activityLevel, out var multiplier)
            ? bmr * multiplier
            : CalculatedMetricsErrors.ActivityLevelInvalid;
    }

    public static Result<double> CalculateCalorieTarget(double tdee, Goal goal)
    {
        return _calorieAdjustments.TryGetValue(goal, out var adjustment)
            ? tdee + adjustment
            : CalculatedMetricsErrors.GoalInvalid;
    }

    public static FitnessStatus DetermineStatus(double calorieTarget)
    {
        if (calorieTarget <= FCEConstants.StatusThresholds.WeakMax)
            return FitnessStatus.Weak;

        if (calorieTarget <= FCEConstants.StatusThresholds.NormalMax)
            return FitnessStatus.Normal;

        return FitnessStatus.Hard;
    }
}
