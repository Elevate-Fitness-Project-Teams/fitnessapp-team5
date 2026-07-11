using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Enums;

namespace NutritionService.Infrastructure.Persistence
{
    public class NutritionDbInitializer
    {
        private readonly NutritionDbContext _context;

        public NutritionDbInitializer(NutritionDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Meals.AnyAsync())
            {
                return;
            }

            var breakfast = new Meal
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000000001"),
                Name = "Greek Yogurt Protein Bowl",
                Type = MealTypeEnum.Breakfast,
                Calories = 420,
                Protein = 38,
                Carbs = 44,
                Fats = 10,
                IngredientsJson = """["Greek yogurt","Oats","Blueberries","Chia seeds","Honey"]""",
                InstructionsJson = """["Add yogurt to a bowl","Top with oats and blueberries","Sprinkle chia seeds","Drizzle honey before serving"]""",
                VariationsJson = """["Use strawberries instead of blueberries","Replace honey with stevia"]""",
                AllergensJson = """["Dairy"]""",
                TagsJson = """["High Protein","Quick","Breakfast"]"""
            };

            var lunch = new Meal
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000000002"),
                Name = "Chicken Quinoa Power Plate",
                Type = MealTypeEnum.Lunch,
                Calories = 680,
                Protein = 52,
                Carbs = 62,
                Fats = 20,
                IngredientsJson = """["Chicken breast","Quinoa","Broccoli","Olive oil","Lemon"]""",
                InstructionsJson = """["Grill chicken until cooked through","Cook quinoa","Steam broccoli","Plate together and finish with olive oil and lemon"]""",
                VariationsJson = """["Swap quinoa for brown rice","Add avocado for more fats"]""",
                AllergensJson = """[]""",
                TagsJson = """["High Protein","Balanced","Lunch"]"""
            };

            var dinner = new Meal
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000000003"),
                Name = "Salmon Sweet Potato Dinner",
                Type = MealTypeEnum.Dinner,
                Calories = 760,
                Protein = 46,
                Carbs = 58,
                Fats = 34,
                IngredientsJson = """["Salmon fillet","Sweet potato","Green beans","Olive oil","Garlic"]""",
                InstructionsJson = """["Bake sweet potato","Season and bake salmon","Saute green beans with garlic","Serve together"]""",
                VariationsJson = """["Use white fish for fewer fats","Add extra sweet potato for higher carbs"]""",
                AllergensJson = """["Fish"]""",
                TagsJson = """["Omega 3","Dinner","Balanced"]"""
            };

            var snack = new Meal
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000000004"),
                Name = "Cottage Cheese Apple Snack",
                Type = MealTypeEnum.Snack,
                Calories = 260,
                Protein = 24,
                Carbs = 28,
                Fats = 5,
                IngredientsJson = """["Cottage cheese","Apple","Cinnamon"]""",
                InstructionsJson = """["Slice the apple","Serve with cottage cheese","Sprinkle cinnamon on top"]""",
                VariationsJson = """["Use pear instead of apple","Add walnuts for extra fats"]""",
                AllergensJson = """["Dairy"]""",
                TagsJson = """["Snack","High Protein","Quick"]"""
            };

            var maintenancePlan = new MealPlan
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000000101"),
                Name = "Balanced Maintenance",
                Description = "A balanced daily structure for moderate calorie targets.",
                TargetCalorieRangeMin = 1900,
                TargetCalorieRangeMax = 2300
            };

            var performancePlan = new MealPlan
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000000102"),
                Name = "Performance Build",
                Description = "Higher calorie structure for active training days.",
                TargetCalorieRangeMin = 2301,
                TargetCalorieRangeMax = 2800
            };

            maintenancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001001"),
                MealPlanId = maintenancePlan.Id,
                MealId = breakfast.Id,
                Meal = breakfast,
                DayOfWeek = DayOfWeek.Monday,
                MealTime = new TimeOnly(8, 0)
            });
            maintenancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001002"),
                MealPlanId = maintenancePlan.Id,
                MealId = lunch.Id,
                Meal = lunch,
                DayOfWeek = DayOfWeek.Monday,
                MealTime = new TimeOnly(13, 0)
            });
            maintenancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001003"),
                MealPlanId = maintenancePlan.Id,
                MealId = dinner.Id,
                Meal = dinner,
                DayOfWeek = DayOfWeek.Monday,
                MealTime = new TimeOnly(19, 0)
            });

            performancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001004"),
                MealPlanId = performancePlan.Id,
                MealId = breakfast.Id,
                Meal = breakfast,
                DayOfWeek = DayOfWeek.Tuesday,
                MealTime = new TimeOnly(8, 0)
            });
            performancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001005"),
                MealPlanId = performancePlan.Id,
                MealId = lunch.Id,
                Meal = lunch,
                DayOfWeek = DayOfWeek.Tuesday,
                MealTime = new TimeOnly(13, 0)
            });
            performancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001006"),
                MealPlanId = performancePlan.Id,
                MealId = snack.Id,
                Meal = snack,
                DayOfWeek = DayOfWeek.Tuesday,
                MealTime = new TimeOnly(16, 30)
            });
            performancePlan.MealPlanItems.Add(new MealPlanItem
            {
                Id = Guid.Parse("01906000-0000-7000-8000-000000001007"),
                MealPlanId = performancePlan.Id,
                MealId = dinner.Id,
                Meal = dinner,
                DayOfWeek = DayOfWeek.Tuesday,
                MealTime = new TimeOnly(20, 0)
            });

            _context.Meals.AddRange(breakfast, lunch, dinner, snack);
            _context.MealPlans.AddRange(maintenancePlan, performancePlan);

            await _context.SaveChangesAsync();
        }
    }
}
