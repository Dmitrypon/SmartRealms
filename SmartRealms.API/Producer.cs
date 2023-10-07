﻿namespace SmartRealms.API
{
    public class Producer : BackgroundService
    {
        private readonly ILogger _logger;

        public Producer(ILogger<Producer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

            }
        }
    }
}