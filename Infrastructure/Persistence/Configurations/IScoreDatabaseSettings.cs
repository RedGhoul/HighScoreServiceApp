using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Configurations
{
    public interface IScoreDatabaseSettings
    {
        string ScoreCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class ScoreDatabaseSettings : IScoreDatabaseSettings
    {
        public string ScoreCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public ScoreDatabaseSettings(IConfiguration configuration)
        {
            ScoreCollectionName = Secrets.GetSectionValue(configuration, "ScoreDatabaseSettings", "CollectionName");
            ConnectionString = Secrets.GetSectionValue(configuration, "ScoreDatabaseSettings", "ConnectionString");
            DatabaseName = Secrets.GetSectionValue(configuration, "ScoreDatabaseSettings", "DatabaseName");
        }

    }
}
