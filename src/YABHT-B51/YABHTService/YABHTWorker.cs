using log4net;
using YABHTService.Configurations;
using YABHTService.Repositories;

namespace YABHTService
{


    internal class YABHTWorker : BackgroundService
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly ConfigurationsManager _configurationsManager;
        private RepositoriesManager _repositoriesManager;

        public YABHTWorker(IConfiguration configuration, ConfigurationsManager configurationsManager, RepositoriesManager repositoriesManager)
        {
            _configuration = configuration;
            _configurationsManager = configurationsManager;
            _repositoriesManager = repositoriesManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info("================================================================================");
            _logger.Info("YABHT Service started");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.Debug("--------------------------------------------------------------------------------");
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
            _logger.Info("YABHT Service stopped");
        }

        private void Loop()
        {
            var repositoryConfigurations = _configurationsManager.GetRepositories();
            foreach (var repositoryConfiguration in repositoryConfigurations)
            {
                _repositoriesManager.BackupRepository(repositoryConfiguration);
            }
        }
    }
}
