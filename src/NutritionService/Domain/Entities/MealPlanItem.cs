using NutritionService.Domain.Entities._Common;

namespace NutritionService.Domain.Entities
{
    public class MealPlanItem : BaseEntity
    {
        public Guid MealPlanId { get; set; }
        public MealPlan? MealPlan { get; set; }
        public Guid MealId { get; set; }
        public Meal? Meal { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly MealTime { get; set; }

    }
}
