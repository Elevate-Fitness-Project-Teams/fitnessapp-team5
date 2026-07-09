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

        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
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

            var entity = new CalculatedMetrics(
                Guid.NewGuid(),
                userId,
                bmr,
                tdee,
                calorieTarget,
                status,
                createdBy);

            entity.AddDomainEvent(new CalculatedMetricsCreatedDomainEvent(userId, status, calorieTarget));

            return entity;
        }

        public Result<CalculatedMetrics> Update(
            double bmr,
            double tdee,
            double calorieTarget,
            FitnessStatus status,
            string modifiedBy,
            FCEService.Domain.Entities.UserFitnessStats.FitnessUpdateReason reason,
            FCEService.Domain.Enums.Goal goal)
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

            AddDomainEvent(new CalculatedMetricsUpdatedDomainEvent(UserId, status, calorieTarget, reason, goal));

            return this;
        }


    }
}
