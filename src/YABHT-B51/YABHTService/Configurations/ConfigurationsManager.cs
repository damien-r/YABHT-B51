using log4net;
using YABHTService.Configurations.Models;

namespace YABHTService.Configurations
{
    internal class ConfigurationsManager
    {

		private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IConfiguration _configuration;

        public ConfigurationsManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<RepositoryConfiguration> GetRepositories()
		{
			var configurationsFolder = _configuration.GetSection("YABHT").GetSection("General").GetValue("ConfigurationFolder", "Configs");

			if (!Directory.Exists(configurationsFolder))
			{
				_logger.Warn($"Configurations folder '{configurationsFolder}' does not exist.");
				return new List<RepositoryConfiguration>();
			}

			throw new NotImplementedException();
		}
	}
}
