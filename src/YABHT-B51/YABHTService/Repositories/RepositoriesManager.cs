using LibGit2Sharp;
using log4net;
using YABHTService.Configurations.Models;

namespace YABHTService.Repositories;

internal class RepositoriesManager
{
    private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IConfiguration _configuration;

    public RepositoriesManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void BackupRepository(RepositoryConfiguration repositoryConfiguration)
    {
        if (_configuration.GetValue<bool>("YABHT:General:AllowUnsafeRepositories"))
        {
            // Avoid problems with a different owner of the repository when running as a service
            GlobalSettings.SetOwnerValidation(false);
        }

        _logger.Debug($"Analyze repository '{repositoryConfiguration.Name}'");
        using var repository = new Repository(repositoryConfiguration.RepositoryPath);

        Commands.Stage(repository, "*");

        var status = repository.RetrieveStatus();
        int fileCount = status.Count(s => s.State != FileStatus.Ignored && s.State != FileStatus.Unaltered);

        if (fileCount > 0)
        {
            _logger.Info($"Backup repository '{repositoryConfiguration.Name}' with {fileCount} files.");
            var now = DateTimeOffset.Now;
            var author = new Signature("YABHT-B51", "YABHT-B51@no-reply.com", now);
            var commit = repository.Commit($"[YABHT] Automated backup at {now} ", author, author);

            Push(repositoryConfiguration, repository);
        }
    }

    private void Push(RepositoryConfiguration repositoryConfiguration, Repository repository)
    {
        foreach (var pushConfiguration in repositoryConfiguration.PushConfigurations)
        {
            _logger.Info($"Push to remote '{pushConfiguration.RemoteName}'");
            var remote = repository.Network.Remotes[pushConfiguration.RemoteName];
            if (remote == null)
            {
                _logger.Error($"Remote '{pushConfiguration.RemoteName}' not found in repository '{repositoryConfiguration.Name}'");
                continue;
            }

            var options = new PushOptions();
            if (!string.IsNullOrEmpty(pushConfiguration.Username))
            {
                options.CredentialsProvider = (_, _, _) => new UsernamePasswordCredentials
                {
                    Username = pushConfiguration.Username,
                    Password = pushConfiguration.Password ?? string.Empty
                };
            }

            try
            {
                var pushRefSpec = $"refs/heads/{repository.Head.FriendlyName}:refs/heads/{repository.Head.FriendlyName}";
                repository.Network.Push(remote, pushRefSpec, options);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to push to remote '{pushConfiguration.RemoteName}'", ex);
            }
        }
    }
}