using System.Reflection;

public static class MigrationHelper
{
    public static async Task ApplyMigrations()
    {
        // prepare data
        int installedDbVer = await GetDatabaseVersionAsync();
        List<IMigration> allMigrations = GetAllMigrations();
        string finalQuery = "";

        // get migrations where version is higher than instaled version
        var newMigrations = allMigrations.Where(x => x.DbVersion > installedDbVer).ToList();
        foreach (var migration in newMigrations)
        {
            string alteredQuery = migration.UpgradeSql.Trim();

            // Add ;
            if (alteredQuery.Length > 0 && alteredQuery.Last() != ';')
                alteredQuery += ';';

            // add to finalquery
            finalQuery += alteredQuery + Environment.NewLine;
        }

        // Apply migrations
        if (newMigrations.Count() > 0)
        {
            // get latest migration version
            int latestMigrationVersion = newMigrations
                .OrderBy(x => x.DbVersion)
                .LastOrDefault().DbVersion;

            // updata database version
            finalQuery += $"UPDATE setting SET settingvalue = {latestMigrationVersion} WHERE settingkey = 'InstalledDatabaseVersion';" + Environment.NewLine;

            // turn into transaction
            finalQuery = $"BEGIN;" + Environment.NewLine + finalQuery + Environment.NewLine + "COMMIT;";

            // Run query
            using var db = NwptDb.GetConnection();
            await db.ExecuteAsync(finalQuery);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static async Task<int> GetDatabaseVersionAsync()
    {
        using var db = NwptDb.GetConnection();

        // Create settings table if needed
        bool doesSettingsTableExist = await db.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT * FROM information_schema.tables WHERE table_schema = 'public' AND table_name = 'setting')");
        if (!doesSettingsTableExist)
        {
            await db.ExecuteAsync("CREATE TABLE setting (" +
                "settingkey varchar(64) PRIMARY KEY," +
                "settingvalue varchar(2048))");
        }

        // get current database version
        SettingsHelper sh = new SettingsHelper();
        return sh.Get<int>("InstalledDatabaseVersion");
    }

    private static List<IMigration> GetAllMigrations()
    {
        List<IMigration> result = new();
        AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IMigration).IsAssignableFrom(p) && !p.Attributes.HasFlag(TypeAttributes.Abstract))
            .ToList()
            .ForEach(x => result.Add((IMigration)Activator.CreateInstance(x)));
        return result;
    }
}