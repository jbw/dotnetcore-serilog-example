using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WelcomeToTheLogFactoryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Using Serilog and ILogger implementation with exceptions serilog package
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new JsonFormatter())
                .CreateLogger();

            try
            {
                new HostBuilder()
                    .ConfigureServices((hostContext, services) => services.AddHostedService<LoggingService>())
                    .ConfigureLogging((hostContext, logging) => logging.AddSerilog(Log.Logger))
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }


    public class LoggingService : IHostedService
    {
        private ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (_logger.BeginScope("LoggerService"))
            using (_logger.BeginScope(new { ImportantId = 1, AnotherOne = "Key" }))
            {

                _logger.LogInformation("Starting to do some logging");

                throw new System.Exception("Halp!");

            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }
    }
}
