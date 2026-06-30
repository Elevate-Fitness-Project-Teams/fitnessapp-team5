using System;
using System.Collections.Generic;
using FCEService.Domain.Common;
using FCEService.Domain.Common.Results;

namespace FCEService.Domain.Entities.UserPlanHistory
{
    public class UserPlanHistory : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string PlanId { get; private set; }
        public DateTime? EndedAt { get; private set; }
        public string ReasonForChange { get; private set; }

        private UserPlanHistory() { }

        private UserPlanHistory(
            Guid id,
            Guid userId,
            string planId,
            DateTime? endedAt,
            string reasonForChange,
            string createdBy) : base(id)
        {
            UserId = userId;
            PlanId = planId;
            EndedAt = endedAt;
            ReasonForChange = reasonForChange;
            CreatedAtUtc = DateTimeOffset.UtcNow;
            CreatedBy = createdBy;
        }

        public static Result<UserPlanHistory> Create(
            Guid userId,
            string planId,
            string reasonForChange,
            string createdBy)
        {
            var errors = new List<Error>();

            if (userId == Guid.Empty)
            {
                errors.Add(UserPlanHistoryErrors.UserIdRequired);
            }

            if (string.IsNullOrWhiteSpace(planId))
            {
                errors.Add(UserPlanHistoryErrors.PlanIdRequired);
            }
            else if (planId.Length > 50)
            {
                errors.Add(UserPlanHistoryErrors.PlanIdTooLong);
            }

            if (string.IsNullOrWhiteSpace(reasonForChange))
            {
                errors.Add(UserPlanHistoryErrors.ReasonForChangeRequired);
            }
            else if (reasonForChange.Length > 255)
            {
                errors.Add(UserPlanHistoryErrors.ReasonForChangeTooLong);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new UserPlanHistory(
                Guid.NewGuid(),
                userId,
                planId,
                null,
                reasonForChange,
                createdBy);
        }

        public Result<Updated> End(DateTime endedAt, string modifiedBy)
        {
            if (endedAt < CreatedAtUtc.UtcDateTime)
            {
                return UserPlanHistoryErrors.EndedAtInvalid;
            }

            EndedAt = endedAt;
            LastModifiedUtc = DateTimeOffset.UtcNow;
            LastModifiedBy = modifiedBy;
            return Result.Updated;
        }
    }
}
