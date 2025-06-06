using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AlertNotificationService.Data;
using AlertNotificationService.Service;
using AlertNotificationService.Services;

namespace AlertNotificationService
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                try
                {
                    var dbContext = services.GetRequiredService<AlertDbContext>();
                    Console.WriteLine("Database connection established successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while connecting to the database: {ex.Message}");
                }
            }
            
            host.Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    
                    services.AddDbContext<AlertDbContext>(options =>
                        options.UseOracle(configuration.GetConnectionString("DefaultConnection")));
              
                    services.AddSingleton<AzureEmailService>();
                    services.AddScoped<AlertProcessor>();
                    services.AddHostedService<RabbitMQConsumer>();
                });
    }
}