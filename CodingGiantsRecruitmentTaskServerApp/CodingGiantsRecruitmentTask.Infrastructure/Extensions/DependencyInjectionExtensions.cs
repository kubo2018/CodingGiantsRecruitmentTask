using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Services;
using CodingGiantsRecruitmentTask.Infrastructure.Repositories;
using CodingGiantsRecruitmentTask.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodingGiantsRecruitmentTask.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Default"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), null);
                    }));

            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatMessageRatingRepository, ChatMessageRatingRepository>();
            services.AddScoped<IChatMessageRatingTypeRepository, ChatMessageRatingTypeRepository>();

            services.AddScoped<IChatGeneratorService, ChatGeneratorService>();

            return services;
        }
    }
}
