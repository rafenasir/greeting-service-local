using Azure.Messaging.ServiceBus;
using GreetingService.API.Functions.Authentication;
using GreetingService.Core;
using GreetingService.Core.Interfaces;
using GreetingService.Infrastructure;
using GreetingService.Infrastructure.MessagingService;
using GreetingService.Infrastructure.TeamApproval;
using GreetingService.Infrastructure.UserService;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(GreetingService.API.Functions.Startup))]
namespace GreetingService.API.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var config = builder.GetContext().Configuration;

            builder.Services.AddLogging(c =>
            {
                var connnectionString = config["LoggingStorageAccount"];
                if (string.IsNullOrWhiteSpace(connnectionString))
                    return;
                var logName = $"{Assembly.GetCallingAssembly().GetName().Name}.log";
                var logger = new LoggerConfiguration()
                                .WriteTo.AzureBlobStorage(connnectionString,
                                                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                                            storageFileName: "{yyyy}/{MM}/{dd}/" + logName,
                                                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
                                .CreateLogger();
                c.AddSerilog(logger, true);
            });

            //builder.Services.AddScoped<IGreetingRepository, SqlGreetingRepository>();
            builder.Services.AddScoped<IGreetingRepository, CosmosDbGreetingRepository>();
            builder.Services.AddScoped<IUserService, SqlUserService>();
            builder.Services.AddScoped<IInvoiceService, SqlInvoiceService>();
            builder.Services.AddScoped<IAuthHandler, BasicAuthHandler>();
            builder.Services.AddScoped<IMessagingService, ServiceBusMessagingService>();
            builder.Services.AddScoped<IApprovalService, TeamsApprovalService>();



            builder.Services.AddDbContext<GreetingDbContext>(options =>
            {
                options.UseSqlServer(config["GreetingDbConnectionString"]);

            });

            builder.Services.AddSingleton(c =>
            {
                var serviceBusClient = new ServiceBusClient(config["ServiceBusConnectionString"]);      //remember to add this connection to the application configuration
                return serviceBusClient.CreateSender("main");
            });

            builder.Services.AddSingleton(c =>
            {
                var CosmosdbClient = new CosmosClient(config["CosmosConnectionString"]); 
                return CosmosdbClient;
            });
        }

            public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
            {
                builder.ConfigurationBuilder.AddAzureKeyVault(Environment.GetEnvironmentVariable("KeyVaultUri"));

            }
    }
}
