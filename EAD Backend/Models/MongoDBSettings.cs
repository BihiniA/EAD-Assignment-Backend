namespace MongoDBExample.Models
{
    public class MongoDBSettings //mongodb class to create connnection strings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;

    }
}