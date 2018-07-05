namespace Shared.Persistency
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        private static string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public static string GetConnectionString()
        {
            //this is such a hacky way of doing this but I structured the context a little 
            //wrong, and this fixes dependency injection and migrations with the smallest effort
            return _connectionString;
        }
    }
}