using FCEService.Domain.Common.Constants;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.CalculatedMetrics;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Services;

public static class FitnessCalculator
{
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
        double multiplier;
        switch (activityLevel)
        {
            case ActivityLevel.Rookie:
                multiplier = FCEConstants.ActivityMultipliers.Rookie;
                break;
            case ActivityLevel.Beginner:
                multiplier = FCEConstants.ActivityMultipliers.Beginner;
                break;
            case ActivityLevel.Intermediate:
                multiplier = FCEConstants.ActivityMultipliers.Intermediate;
                break;
            case ActivityLevel.Advance:
                multiplier = FCEConstants.ActivityMultipliers.Advance;
                break;
            case ActivityLevel.TrueBeast:
                multiplier = FCEConstants.ActivityMultipliers.TrueBeast;
                break;
            default:
                return CalculatedMetricsErrors.ActivityLevelInvalid;
        }

        return bmr * multiplier;
    }

    public static Result<double> CalculateCalorieTarget(double tdee, Goal goal)
    {
        switch (goal)
        {
            case Goal.LoseWeight:
                return tdee + FCEConstants.CalorieAdjustments.LoseWeight;
            case Goal.GetFitter:
                return tdee + FCEConstants.CalorieAdjustments.GetFitter;
            case Goal.GainWeight:
                return tdee + FCEConstants.CalorieAdjustments.GainWeight;
            default:
                return CalculatedMetricsErrors.GoalInvalid;
        }
    }

    public static FitnessStatus DetermineStatus(double calorieTarget)
    {
        if (calorieTarget <= FCEConstants.StatusThresholds.WeakMax)
        {
            return FitnessStatus.Weak;
        }
        else if (calorieTarget <= FCEConstants.StatusThresholds.NormalMax)
        {
            return FitnessStatus.Normal;
        }
        else
        {
            return FitnessStatus.Hard;
        }
    }
}
