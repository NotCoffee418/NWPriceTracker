using Npgsql;

namespace Caleb.Shared.Helpers;
public static class NwptDb
{
    // consts
    private const string connStr = "Server=db;Port=5432;Database=Caleb.Database;Userid=Caleb;Password=kXakVYj7WEZYQgfH;Include Error Detail=true";

    public static NpgsqlConnection GetConnection()
        => new NpgsqlConnection(connStr);

    public static bool IsUnitTesting { get; set; } = false;
}