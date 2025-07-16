using Microsoft.Extensions.DependencyInjection;

namespace CodingGiantsRecruitmentTask.Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly));
            return services;
        }
    }
}
