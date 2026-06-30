using System;
using System.Collections.Generic;
using FCEService.Domain.Common;
using FCEService.Domain.Common.Constants;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities.CalculatedMetrics
{
    public class CalculatedMetrics : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public double Bmr { get; private set; }
        public double Tdee { get; private set; }
        public double CalorieTarget { get; private set; }
        public FitnessStatus Status { get; private set; }

        private CalculatedMetrics() { }

        private CalculatedMetrics(
            Guid id,
            Guid userId,
            double bmr,
            double tdee,
            double calorieTarget,
            FitnessStatus status,
            string createdBy) : base(id)
        {
            UserId = userId;
            Bmr = bmr;
            Tdee = tdee;
            CalorieTarget = calorieTarget;
            Status = status;
            CreatedAtUtc = DateTimeOffset.UtcNow;
            CreatedBy = createdBy;
        }

        public static Result<CalculatedMetrics> Create(
            Guid userId,
            double bmr,
            double tdee,
            double calorieTarget,
            FitnessStatus status,
            string createdBy)
        {
            var errors = new List<Error>();

            if (userId == Guid.Empty)
            {
                errors.Add(CalculatedMetricsErrors.UserIdRequired);
            }

            if (bmr <= 0)
            {
                errors.Add(CalculatedMetricsErrors.BmrInvalid);
            }

            if (tdee <= 0)
            {
                errors.Add(CalculatedMetricsErrors.TdeeInvalid);
            }

            if (!Enum.IsDefined(typeof(FitnessStatus), status))
            {
                errors.Add(CalculatedMetricsErrors.StatusInvalid);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new CalculatedMetrics(
                Guid.NewGuid(),
                userId,
                bmr,
                tdee,
                calorieTarget,
                status,
                createdBy);
        }

        public Result<CalculatedMetrics> Update(
            double bmr,
            double tdee,
            double calorieTarget,
            FitnessStatus status,
            string modifiedBy)
        {
            var errors = new List<Error>();

            if (bmr <= 0)
            {
                errors.Add(CalculatedMetricsErrors.BmrInvalid);
            }

            if (tdee <= 0)
            {
                errors.Add(CalculatedMetricsErrors.TdeeInvalid);
            }

            if (!Enum.IsDefined(typeof(FitnessStatus), status))
            {
                errors.Add(CalculatedMetricsErrors.StatusInvalid);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            Bmr = bmr;
            Tdee = tdee;
            CalorieTarget = calorieTarget;
            Status = status;
            LastModifiedUtc = DateTimeOffset.UtcNow;
            LastModifiedBy = modifiedBy;

            return this;
        }

        public static Result<CalculatedMetrics> Calculate(UserFitnessStats.UserFitnessStats stats, string createdBy)
        {
            if (stats == null)
            {
                return CalculatedMetricsErrors.NullStats;
            }

            double bmr;
            if (stats.Gender == Gender.Male)
            {
                bmr = (FCEConstants.Bmr.WeightMultiplier * stats.Weight) 
                    + (FCEConstants.Bmr.HeightMultiplier * stats.Height) 
                    - (FCEConstants.Bmr.AgeMultiplier * stats.Age) 
                    + FCEConstants.Bmr.MaleOffset;
            }
            else
            {
                bmr = (FCEConstants.Bmr.WeightMultiplier * stats.Weight) 
                    + (FCEConstants.Bmr.HeightMultiplier * stats.Height) 
                    - (FCEConstants.Bmr.AgeMultiplier * stats.Age) 
                    - FCEConstants.Bmr.FemaleOffset;
            }

            double multiplier;
            switch (stats.ActivityLevel)
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

            double tdee = bmr * multiplier;

            double calorieTarget;
            switch (stats.Goal)
            {
                case Goal.LoseWeight:
                    calorieTarget = tdee + FCEConstants.CalorieAdjustments.LoseWeight;
                    break;
                case Goal.GetFitter:
                    calorieTarget = tdee + FCEConstants.CalorieAdjustments.GetFitter;
                    break;
                case Goal.GainWeight:
                    calorieTarget = tdee + FCEConstants.CalorieAdjustments.GainWeight;
                    break;
                case Goal.GainMoreFlexible:
                    calorieTarget = tdee + FCEConstants.CalorieAdjustments.GainMoreFlexible;
                    break;
                case Goal.LearnTheBasic:
                    calorieTarget = tdee + FCEConstants.CalorieAdjustments.LearnTheBasic;
                    break;
                default:
                    return CalculatedMetricsErrors.GoalInvalid;
            }

            FitnessStatus status;
            if (calorieTarget <= FCEConstants.StatusThresholds.WeakMax)
            {
                status = FitnessStatus.Weak;
            }
            else if (calorieTarget <= FCEConstants.StatusThresholds.NormalMax)
            {
                status = FitnessStatus.Normal;
            }
            else
            {
                status = FitnessStatus.Hard;
            }

            return Create(stats.UserId, bmr, tdee, calorieTarget, status, createdBy);
        }
    }
}
