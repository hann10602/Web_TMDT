using System.Data.SqlClient;

namespace ShoeStore.DB
{
    public static class ShopDBContext
    {
        static IConfigurationRoot _config;
        static ShopDBContext()
        {
            _config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }
}
