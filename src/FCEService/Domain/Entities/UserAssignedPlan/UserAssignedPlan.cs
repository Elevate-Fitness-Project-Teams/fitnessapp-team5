using System;
using System.Collections.Generic;
using FCEService.Domain.Common;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserAssignedPlan;

namespace FCEService.Domain.Entities.UserAssignedPlan
{
    public class UserAssignedPlan : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public Guid PlanId { get; private set; }
        public bool IsActive { get; private set; }

        private UserAssignedPlan() { }

        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        private UserAssignedPlan(
            Guid id,
            Guid userId,
            Guid planId,
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
            Guid planId,
            string planName,
            string createdBy,
            bool isActive = true)
        {
            var errors = new List<Error>();

            if (userId == Guid.Empty)
            {
                errors.Add(UserAssignedPlanErrors.UserIdRequired);
            }

            if (planId == Guid.Empty)
            {
                errors.Add(UserAssignedPlanErrors.PlanIdRequired);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            var plan = new UserAssignedPlan(
                Guid.NewGuid(),
                userId,
                planId,
                isActive,
                createdBy);

            // Raises the bridge event → MassTransit handler will publish Integration Event
            plan.AddDomainEvent(new UserAssignedPlanCreatedDomainEvent(userId, planId, planName));

            return plan;
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            LastModifiedUtc = DateTimeOffset.UtcNow;
            LastModifiedBy = modifiedBy;
        }
    }
}
