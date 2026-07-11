namespace SmartCoachService.Clients.Progress
{
    public sealed record ProgressResponse(int CompletedWorkouts,
                                          decimal CaloriesConsumedToday,
                                          decimal CaloriesBurnedToday,
                                          int CurrentStreak);
}
