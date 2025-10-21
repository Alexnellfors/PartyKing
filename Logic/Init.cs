using Logic.DatabaseContexts;
using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poke_Connector;
using Poke_Connector.Services;

namespace Logic
{
    public static class Init
    {
        public static IServiceCollection AddLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPokeConnector(configuration);

            services.AddScoped<LogicService>();
            services.AddScoped<DatabaseService>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration["Database:Connectionstring"]));


            return services;
        }
    }
}
