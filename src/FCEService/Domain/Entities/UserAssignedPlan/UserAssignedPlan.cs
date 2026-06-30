using System;
using System.Collections.Generic;
using FCEService.Domain.Common;
using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.UserAssignedPlan
{
    public class UserAssignedPlan : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string PlanId { get; private set; }
        public bool IsActive { get; private set; }

        private UserAssignedPlan() { }

        private UserAssignedPlan(
            Guid id,
            Guid userId,
            string planId,
            bool isActive,
            string createdBy) : base(id)
        {
            UserId = userId;
            PlanId = planId;
            IsActive = isActive;
            CreatedAtUtc = DateTimeOffset.UtcNow;
            CreatedBy = createdBy;
        }

        public static Result<UserAssignedPlan> Create(
            Guid userId,
            string planId,
            string createdBy,
            bool isActive = true)
        {
            var errors = new List<Error>();

            if (userId == Guid.Empty)
            {
                errors.Add(UserAssignedPlanErrors.UserIdRequired);
            }

            if (string.IsNullOrWhiteSpace(planId))
            {
                errors.Add(UserAssignedPlanErrors.PlanIdRequired);
            }
            else if (planId.Length > 50)
            {
                errors.Add(UserAssignedPlanErrors.PlanIdTooLong);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new UserAssignedPlan(
                Guid.NewGuid(),
                userId,
                planId,
                isActive,
                createdBy);
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            LastModifiedUtc = DateTimeOffset.UtcNow;
            LastModifiedBy = modifiedBy;
        }
    }
}
