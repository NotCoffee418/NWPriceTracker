public static class NwptDb
{
    // consts
    private const string connStr = "Server=db;Port=5432;Database=NWPriceTracker.Database;Userid=NWPriceTracker;Password=kXakVYj7WEZYQgfH;Include Error Detail=true";

    public static NpgsqlConnection GetConnection()
        => new NpgsqlConnection(connStr);
}