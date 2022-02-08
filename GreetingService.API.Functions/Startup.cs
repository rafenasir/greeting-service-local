using GreetingService.API.Functions.Authentication;
using GreetingService.Core;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GreetingService.API.Functions.Startup))]
namespace GreetingService.API.Functions
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();
            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();
            builder.Services.AddSingleton<IGreetingRepository, MemoryGreetingRepository>();

            //builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(c =>
            //{
            //    var config = c.GetService<IConfiguration>();
            //    return new FileGreetingRepository(config["FileRepositoryFilePath"]);
            //});

            builder.Services.AddScoped<IUserService, AppSettingsUserService>();


        }
    }
}
