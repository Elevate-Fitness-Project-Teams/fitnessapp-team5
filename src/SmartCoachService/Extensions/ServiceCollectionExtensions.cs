using Microsoft.EntityFrameworkCore;
using SmartCoachService.Clients.FCE;
using SmartCoachService.Clients.Progress;
using SmartCoachService.Common;
using SmartCoachService.Persistence;
using SmartCoachService.Persistence.Repositories.ChatMessageRepository;
using SmartCoachService.Persistence.Repositories.ChatSessionRepository;
using SmartCoachService.Persistence.Repositories.RecommendationCache;
using SmartCoachService.Persistence.Repositories.UnitOfWork;
using SmartCoachService.Services.AI;
using SmartCoachService.Services.AI.Providers;
using SmartCoachService.Services.Cache;
using SmartCoachService.Services.ChatRateLimit;
using SmartCoachService.Services.CurrentUser;
using SmartCoachService.Services.Prompt;

namespace SmartCoachService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            #region OpenAPI + Scalar (.NET 10)

            services.AddOpenApi();

            #endregion

            #region DbContext

            services.AddDbContext<SmartCoachDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            #endregion

            #region Repositories

            services.AddScoped<IChatSessionRepository, ChatSessionRepository>();

            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

            services.AddScoped<IRecommendationCacheRepository, RecommendationCacheRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Http Clients

            services.AddHttpClient<IFceClient, FceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:FCE"]!);
            });

            //services.AddHttpClient<IProgressClient, ProgressClient>(client =>
            //{
            //    client.BaseAddress = new Uri(configuration["Services:Progress"]!);
            //});

            #endregion

            #region AI

            services.Configure<AIProviderSettings>(
                configuration.GetSection("AIProviders"));

            services.AddScoped<IAIProvider, OpenAIProvider>();

            services.AddScoped<IAIProvider, ClaudeProvider>();

            services.AddScoped<IAIService, AIService>();

            #endregion

            #region Smart Coach Services

            services.AddScoped<IPromptBuilder, PromptBuilder>();

            services.AddScoped<IRecommendationCacheService, RecommendationCacheService>();

            services.AddScoped<IChatRateLimitService, ChatRateLimitService>();

            #endregion

            #region Current User

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            #endregion

            #region Distributed Cache

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration.GetConnectionString("Redis");
            });

            #endregion

            return services;
        }
    }
}
