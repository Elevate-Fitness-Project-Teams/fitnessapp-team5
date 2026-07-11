using NutritionService.Domain.Entities._Common;
using NutritionService.Domain.Enums;

namespace NutritionService.Domain.Entities
{
    public class Meal : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public MealTypeEnum Type { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public string IngredientsJson { get; set; } = string.Empty;
        public string InstructionsJson { get; set; } = string.Empty;
        public string? VariationsJson { get; set; }
        public string AllergensJson { get; set; } = string.Empty;
        public string TagsJson { get; set; } = string.Empty;
    }
}
