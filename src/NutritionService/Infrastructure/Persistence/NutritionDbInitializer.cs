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

        }
    }
}
