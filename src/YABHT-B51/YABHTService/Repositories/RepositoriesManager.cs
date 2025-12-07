using LibGit2Sharp;
using log4net;
using YABHTService.Configurations.Models;

namespace YABHTService.Repositories;

internal class RepositoriesManager
{
		private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public void BackupRepository(RepositoryConfiguration repositoryConfiguration)
    {
        _logger.Debug($"Analyze repository '{repositoryConfiguration.Name}'");
        using var repository = new Repository(repositoryConfiguration.RepositoryPath);

        Commands.Stage(repository, "*");
        
        var status = repository.RetrieveStatus();
        int fileCount = status.Count(s => s.State != FileStatus.Ignored && s.State != FileStatus.Unaltered);
        
        if(fileCount > 0) _logger.Info($"Backup repository '{repositoryConfiguration.Name}' with {fileCount} files.");
        var now = DateTimeOffset.Now;
        var author = new Signature("YABHT-B51", "YABHT-B51@no-reply.com", now);
        var commit = repository.Commit($"[YABHT] Automated backup at {now} ", author, author);
    }
}