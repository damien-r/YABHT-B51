namespace YABHTService
{


    public class YABHTWorker() : BackgroundService
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Debug("Worker running");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
