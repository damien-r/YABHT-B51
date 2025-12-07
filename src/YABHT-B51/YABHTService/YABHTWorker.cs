using log4net;
using YABHTService.Configurations;

namespace YABHTService
{


    internal class YABHTWorker : BackgroundService
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly ConfigurationsManager _configurationsManager;

        public YABHTWorker(IConfiguration configuration, ConfigurationsManager configurationsManager)
        {
            _configuration = configuration;
            _configurationsManager = configurationsManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Debug("Worker running");
                try
                {
                    Loop();
                }
                catch (Exception ex)
                {
                    _logger.Error("An error occurred during main loop", ex);
                }
                var runningRate = _configuration.GetSection("YABHT").GetSection("General").GetValue("RunningRate", TimeSpan.FromMinutes(10));
                _logger.Debug($"Job done. Waiting for {runningRate}");
                await Task.Delay(runningRate, stoppingToken);
            }
        }

        private void Loop()
        {
            _configurationsManager.GetRepositories();

            throw new NotImplementedException();
        }
    }
}
