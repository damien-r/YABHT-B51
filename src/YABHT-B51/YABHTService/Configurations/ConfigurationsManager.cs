using log4net;
using Newtonsoft.Json;
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
			var configurationsFolder = _configuration.GetSection("YABHT").GetSection("General").GetValue("ConfigurationFolder", "AppData/Configurations");
			var path = new FileInfo(configurationsFolder);

			if (!Directory.Exists(path.FullName))
			{
				_logger.Warn($"Configurations folder '{path.FullName}' does not exist.");
				return new List<RepositoryConfiguration>();
			}

			var configFiles = Directory.GetFiles(path.FullName).Where(e => e.EndsWith(".yabht-b51-config"));
			_logger.Info($"Found {configFiles.Count()} config files.");
			
			
			var configurations = new List<RepositoryConfiguration>();
			
			foreach (var configFile in configFiles)
			{
				var repositoryConfiguration = JsonConvert.DeserializeObject<RepositoryConfiguration>(File.ReadAllText(configFile));
				configurations.Add(repositoryConfiguration);
			}

			throw new NotImplementedException();
		}
	}
}
