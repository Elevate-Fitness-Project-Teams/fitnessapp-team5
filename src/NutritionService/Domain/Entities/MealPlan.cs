using NutritionService.Domain.Entities._Common;

namespace NutritionService.Domain.Entities
{
    public class MealPlan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double TargetCalorieRangeMin { get; set; }
        public double TargetCalorieRangeMax { get; set; }
        
        //Navigation Properties
        public ICollection<MealPlanItem> MealPlanItems { get; set; } = new List<MealPlanItem>();
    }
}
