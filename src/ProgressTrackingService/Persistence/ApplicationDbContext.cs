using Microsoft.EntityFrameworkCore;
using ProgressTrackingService.Entities;

namespace ProgressTrackingService.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();
    public DbSet<WorkoutLogExercise> WorkoutLogExercises => Set<WorkoutLogExercise>();
    public DbSet<WeightHistory> WeightHistories => Set<WeightHistory>();
    public DbSet<BodyMeasurement> BodyMeasurements => Set<BodyMeasurement>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<Streak> Streaks => Set<Streak>();
    public DbSet<UserStatistic> UserStatistics => Set<UserStatistic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}