using System;
using System.Collections.Generic;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities.FitnessPlanConfig
{
    public class FitnessPlanConfig
    {
        public string PlanId { get; private set; }
        public string PlanName { get; private set; }
        public string Description { get; private set; }
        public Goal Goal { get; private set; }
        public FitnessStatus Status { get; private set; }
        public double MinCalorie { get; private set; }
        public double MaxCalorie { get; private set; }
        public string EstimatedDuration { get; private set; }
        public int WorkoutsPerWeek { get; private set; }
        public string ProgramType { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; init; }
        public string? CreatedBy { get; init; }
        public DateTimeOffset LastModifiedUtc { get; set; }
        public string? LastModifiedBy { get; set; }

        private FitnessPlanConfig() { }

        private FitnessPlanConfig(
            string planId,
            string planName,
            string description,
            Goal goal,
            FitnessStatus status,
            double minCalorie,
            double maxCalorie,
            string estimatedDuration,
            int workoutsPerWeek,
            string programType,
            string createdBy)
        {
            PlanId = planId;
            PlanName = planName;
            Description = description;
            Goal = goal;
            Status = status;
            MinCalorie = minCalorie;
            MaxCalorie = maxCalorie;
            EstimatedDuration = estimatedDuration;
            WorkoutsPerWeek = workoutsPerWeek;
            ProgramType = programType;
            CreatedAtUtc = DateTimeOffset.UtcNow;
            CreatedBy = createdBy;
        }

        public static Result<FitnessPlanConfig> Create(
            string planId,
            string planName,
            string description,
            Goal goal,
            FitnessStatus status,
            double minCalorie,
            double maxCalorie,
            string estimatedDuration,
            int workoutsPerWeek,
            string programType,
            string createdBy)
        {
            var errors = Validate(planId, planName, goal, status, minCalorie, maxCalorie, workoutsPerWeek);

            if (errors.Count > 0)
            {
                return errors;
            }

            return new FitnessPlanConfig(
                planId,
                planName,
                description,
                goal,
                status,
                minCalorie,
                maxCalorie,
                estimatedDuration,
                workoutsPerWeek,
                programType,
                createdBy);
        }

        public Result<FitnessPlanConfig> Update(
            string planName,
            string description,
            Goal goal,
            FitnessStatus status,
            double minCalorie,
            double maxCalorie,
            string estimatedDuration,
            int workoutsPerWeek,
            string programType,
            string modifiedBy)
        {
            var errors = Validate(PlanId, planName, goal, status, minCalorie, maxCalorie, workoutsPerWeek);

            if (errors.Count > 0)
            {
                return errors;
            }

            PlanName = planName;
            Description = description;
            Goal = goal;
            Status = status;
            MinCalorie = minCalorie;
            MaxCalorie = maxCalorie;
            EstimatedDuration = estimatedDuration;
            WorkoutsPerWeek = workoutsPerWeek;
            ProgramType = programType;
            LastModifiedUtc = DateTimeOffset.UtcNow;
            LastModifiedBy = modifiedBy;

            return this;
        }

        private static List<Error> Validate(
            string planId,
            string planName,
            Goal goal,
            FitnessStatus status,
            double minCalorie,
            double maxCalorie,
            int workoutsPerWeek)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(planId))
            {
                errors.Add(FitnessPlanConfigErrors.PlanIdRequired);
            }
            else if (planId.Length > 50)
            {
                errors.Add(FitnessPlanConfigErrors.PlanIdTooLong);
            }

            if (string.IsNullOrWhiteSpace(planName))
            {
                errors.Add(FitnessPlanConfigErrors.PlanNameRequired);
            }
            else if (planName.Length > 100)
            {
                errors.Add(FitnessPlanConfigErrors.PlanNameTooLong);
            }

            if (!Enum.IsDefined(typeof(Goal), goal))
            {
                errors.Add(FitnessPlanConfigErrors.GoalInvalid);
            }

            if (!Enum.IsDefined(typeof(FitnessStatus), status))
            {
                errors.Add(FitnessPlanConfigErrors.StatusInvalid);
            }

            if (minCalorie < 0)
            {
                errors.Add(FitnessPlanConfigErrors.MinCalorieInvalid);
            }

            if (maxCalorie < minCalorie)
            {
                errors.Add(FitnessPlanConfigErrors.MaxCalorieInvalid);
            }

            if (workoutsPerWeek <= 0)
            {
                errors.Add(FitnessPlanConfigErrors.WorkoutsPerWeekInvalid);
            }

            return errors;
        }
    }
}
