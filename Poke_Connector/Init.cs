using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Poke_Connector.Config;
using Poke_Connector.Services;

namespace Poke_Connector
{
    public static class Init
    {
        public static IServiceCollection AddPokeConnector(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<PokeConfiguration>()
                .Bind(configuration.GetSection("PokeConfig"))
                .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl), "PokeApi:BaseUrl must be set")
                .Validate(o => Uri.TryCreate(o.BaseUrl, UriKind.Absolute, out _), "PokeApi:BaseUrl must be an absolute URL")
                .ValidateOnStart();

            services.AddHttpClient(Constants.DefaultClientName, (sp, client) =>
            {
                var opts = sp.GetRequiredService<IOptions<PokeConfiguration>>().Value;
                client.BaseAddress = new Uri(opts.BaseUrl!);
                client.Timeout = TimeSpan.FromSeconds(opts.TimeoutSeconds);
            });

            services.AddScoped<PokeService>();

            return services;
        }
    }
}
