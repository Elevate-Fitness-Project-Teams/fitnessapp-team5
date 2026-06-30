using NutritionService.Domain.Entities._Common;

namespace NutritionService.Domain.Entities
{
    public class MealPlan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double TargetCalorieTangeMin { get; set; }
        public double TargetCalorieTangeMax { get; set; }
        
        //Navigation Properties
       public IEnumerable<MealPlanItem> MealPlanItems { get; set; } = new List<MealPlanItem>();
    }
}
