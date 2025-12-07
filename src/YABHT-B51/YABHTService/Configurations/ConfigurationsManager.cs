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
			var defaultConfigurationsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "B51/YABHT-B51/AppData/Configurations");
			var configurationsFolder = _configuration.GetSection("YABHT").GetSection("General").GetValue("ConfigurationFolder", defaultConfigurationsFolder);
			var path = new FileInfo(configurationsFolder);

			if (!Directory.Exists(path.FullName))
			{
				_logger.Warn($"Configurations folder '{path.FullName}' does not exist.");
				return new List<RepositoryConfiguration>();
			}

			var configFiles = Directory.GetFiles(path.FullName).Where(e => e.EndsWith(".yabht-b51-config")).ToArray();
			_logger.Info($"Found {configFiles.Count()} config files.");

			if (!configFiles.Any())
			{
				CreateExampleFile(path);
			}
			
			var configurations = new List<RepositoryConfiguration>();
			
			foreach (var configFile in configFiles)
			{
				try
				{
					_logger.Debug($"Reading configuration file '{configFile}'.");
					var repositoryConfiguration = JsonConvert.DeserializeObject<RepositoryConfiguration>(File.ReadAllText(configFile));
					configurations.Add(repositoryConfiguration);
				} catch (Exception ex)
				{
					_logger.Error($"Error while reading configuration file '{configFile}'.", ex);
				}
			}

			return configurations;
		}

		private void CreateExampleFile(FileInfo path)
		{
			var exampleConfiguration = new RepositoryConfiguration();
			var json = JsonConvert.SerializeObject(exampleConfiguration, Formatting.Indented);
			
			var file = Path.Combine(path.FullName, "example.yabht-b51-config");
			_logger.Info($"Creating example configuration file '{file}'.");
			File.WriteAllText(file, json);
		}
    }
}
