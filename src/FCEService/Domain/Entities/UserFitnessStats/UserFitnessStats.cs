using System;
using System.Collections.Generic;
using FCEService.Domain.Common;
using FCEService.Domain.Common.Constants;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Enums;

namespace FCEService.Domain.Entities.UserFitnessStats
{
    public class UserFitnessStats : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; private set; }
        public Goal Goal { get; private set; }
        public ActivityLevel ActivityLevel { get; private set; }

        public int Age => CalculateAge(BirthDate);

        private UserFitnessStats() { }

        private UserFitnessStats(
            Guid id,
            Guid userId,
            double weight,
            double height,
            DateTime birthDate,
            Gender gender,
            Goal goal,
            ActivityLevel activityLevel,
            string createdBy) : base(id)
        {
            UserId = userId;
            Weight = weight;
            Height = height;
            BirthDate = birthDate;
            Gender = gender;
            Goal = goal;
            ActivityLevel = activityLevel;
            CreatedAtUtc = DateTimeOffset.UtcNow;
            CreatedBy = createdBy;
        }

        public static Result<UserFitnessStats> Create(
            Guid userId,
            double weight,
            double height,
            DateTime birthDate,
            Gender gender,
            Goal goal,
            ActivityLevel activityLevel,
            string createdBy)
        {
            var errors = Validate(userId, weight, height, birthDate, gender, goal, activityLevel);

            if (errors.Count > 0)
            {
                return errors;
            }

            var userFitnessStats = new UserFitnessStats(
                Guid.NewGuid(),
                userId,
                weight,
                height,
                birthDate,
                gender,
                goal,
                activityLevel,
                createdBy);

            userFitnessStats.AddDomainEvent(new UserFitnessStatsCreatedDomainEvent(userFitnessStats));

            return userFitnessStats;
        }

        public Result<UserFitnessStats> Update(
            double weight,
            double height,
            DateTime birthDate,
            Gender gender,
            Goal goal,
            ActivityLevel activityLevel,
            string modifiedBy)
        {
            var errors = Validate(UserId, weight, height, birthDate, gender, goal, activityLevel);

            if (errors.Count > 0)
            {
                return errors;
            }

            Weight = weight;
            Height = height;
            BirthDate = birthDate;
            Gender = gender;
            Goal = goal;
            ActivityLevel = activityLevel;
            LastModifiedUtc = DateTimeOffset.UtcNow;
            LastModifiedBy = modifiedBy;

            AddDomainEvent(new UserFitnessStatsUpdatedDomainEvent(this));

            return this;
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.UtcNow.Date;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        private static List<Error> Validate(
            Guid userId,
            double weight,
            double height,
            DateTime birthDate,
            Gender gender,
            Goal goal,
            ActivityLevel activityLevel)
        {
            var errors = new List<Error>();

            if (userId == Guid.Empty)
            {
                errors.Add(UserFitnessStatsErrors.UserIdRequired);
            }

            if (weight < FCEConstants.Validation.MinWeight || weight > FCEConstants.Validation.MaxWeight)
            {
                errors.Add(UserFitnessStatsErrors.WeightInvalid);
            }

            if (height < FCEConstants.Validation.MinHeight || height > FCEConstants.Validation.MaxHeight)
            {
                errors.Add(UserFitnessStatsErrors.HeightInvalid);
            }

            if (birthDate == default || birthDate > DateTime.UtcNow)
            {
                errors.Add(UserFitnessStatsErrors.BirthDateRequired);
            }
            else
            {
                var age = CalculateAge(birthDate);
                if (age < FCEConstants.Validation.MinAge || age > FCEConstants.Validation.MaxAge)
                {
                    errors.Add(UserFitnessStatsErrors.AgeInvalid);
                }
            }

            if (!Enum.IsDefined(typeof(Gender), gender))
            {
                errors.Add(UserFitnessStatsErrors.GenderInvalid);
            }

            if (!Enum.IsDefined(typeof(Goal), goal))
            {
                errors.Add(UserFitnessStatsErrors.GoalInvalid);
            }

            if (!Enum.IsDefined(typeof(ActivityLevel), activityLevel))
            {
                errors.Add(UserFitnessStatsErrors.ActivityLevelInvalid);
            }

            return errors;
        }
    }
}
