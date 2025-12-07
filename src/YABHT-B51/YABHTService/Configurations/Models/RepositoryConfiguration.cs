namespace YABHTService.Configurations.Models
{
    public record RepositoryConfiguration
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        /// <summary>
        /// This needs to be initialized as a Git repository
        /// </summary>
        public string RepositoryPath { get; set; }
        
        public bool SafeRepository { get; set; }
        
    }
}