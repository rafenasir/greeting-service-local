using GreetingService.API.Functions.Authentication;
using GreetingService.Core;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using GreetingService.Infrastructure.UserService;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

[assembly: FunctionsStartup(typeof(GreetingService.API.Functions.Startup))]
namespace GreetingService.API.Functions
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            var config = builder.GetContext().Configuration;
            
            builder.Services.AddLogging(c => { 
                var connnectionString = config["LoggingStorageAccount"]; 
                if(string.IsNullOrWhiteSpace(connnectionString))
                    return;
                var logName = $"{Assembly.GetCallingAssembly().GetName().Name}.log";
                var logger = new LoggerConfiguration()
                                .WriteTo.AzureBlobStorage(connnectionString, 
                                                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                                            storageFileName: "{yyyy}/{MM}/{dd}/" + logName ,
                                                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
                                .CreateLogger();
                c.AddSerilog(logger, true);
                    });

            //test change
            //builder.Services.AddHttpClient();
            builder.Services.AddScoped<IGreetingRepository, SqlGreetingRepository>();

            //builder.Services.AddScoped<IGreetingRepository, FileGreetingRepository>(c =>
            //{
            //    var config = c.GetService<IConfiguration>();
            //    return new FileGreetingRepository(config["FileRepositoryFilePath"]);
            //});

            builder.Services.AddScoped<IUserService, SqlUserService>();

            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();

            builder.Services.AddDbContext<GreetingDbContext>(options =>
            {
                options.UseSqlServer(config["GreetingDbConnectionString"]);
            });



        }
    }
}
