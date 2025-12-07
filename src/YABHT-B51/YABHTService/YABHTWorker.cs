namespace YABHTService
{


    public class YABHTWorker : BackgroundService
    {
		private readonly IConfiguration _configuration;

		public YABHTWorker(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Debug("Worker running");

                var runningRate = _configuration.GetSection("YABHT").GetSection("General").GetValue("RunningRate", TimeSpan.FromMinutes(10));
                _logger.Debug($"Job done. Waiting for {runningRate}");
                await Task.Delay(runningRate, stoppingToken);
			}
        }
    }
}
