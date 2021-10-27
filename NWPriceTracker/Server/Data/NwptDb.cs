public static class NwptDb
{
    static NwptDb()
    {
        // -- CONFIG
        var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: false)
                 .Build();
        config.GetSection("Database").Bind(DatabaseConfig);
    }

    private static DatabaseConfig DatabaseConfig { get; set; } = new();

    public static NpgsqlConnection GetConnection()
        => new NpgsqlConnection(DatabaseConfig.ConnectionString);
}