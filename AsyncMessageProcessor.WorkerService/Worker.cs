using AsyncMessageProcessor.Application.Interfaces;

namespace AsyncMessageProcessor.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISendEmailService _sendEmailService;
        public Worker(ILogger<Worker> logger, ISendEmailService sendEmailService)
        {
            _logger = logger;
            _sendEmailService = sendEmailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await _sendEmailService.SendEmailAsync(stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
