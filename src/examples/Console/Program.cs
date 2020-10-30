using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WelcomeToTheLogFactoryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // using .NET ILogger<T> implementation and console output with scopes
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => services.AddHostedService<LoggingService>())
                .ConfigureLogging((hostContext, logging) => logging.AddConsole(options => options.IncludeScopes = true));
          
            builder.Build() .Run();
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
