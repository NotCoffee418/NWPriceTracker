using System.ComponentModel;

public class SettingsHelper
{
    public SettingsHelper()
    {
        CheckUpdateConfigVersion();
    }

    private bool IsUpdateCheckDone { get; set; } = false;

    /// <summary>
    /// Return convertedvalue attached to confkey
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="confKey"></param>
    /// <returns>default(T) or value</returns>
    public T Get<T>(string confKey)
    {
        // Get Config row from DB
        using var db = NwptDb.GetConnection();
        Setting s = db.QuerySingleOrDefault<Setting>(
            "SELECT * FROM setting WHERE settingkey = @SettingKey",
            new {SettingKey = confKey});

        // Null check
        if (s == null)
            throw new Exception($"Config.Get() failed because the key '{confKey}' does not exist in the database.");

        // Return null or value
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFrom(s.SettingValue);
    }

    public void Set<T>(string confKey, T value)
    {
        // Update the value
        using var db = NwptDb.GetConnection();
        db.Execute("INSERT INTO setting (settingkey, settingvalue) VALUES (@SettingKey, @SettingValue) " +
            "ON CONFLICT (settingkey) DO " +
            "UPDATE SET settingvalue = EXCLUDED.settingvalue",
            new
            {
                SettingKey = confKey,
                SettingValue = value.ToString()
            });
    }

    /// <summary>
    /// Should be run when opening the controller for config key/values
    /// Will add new values if there are any & set DB version to current
    /// </summary>
    public void CheckUpdateConfigVersion()
    {
        // Check version only once per application lifetime
        if (IsUpdateCheckDone)
            return;

        // Get application's config version
        int applicationVersion = SettingInitializer.Keys
            .OrderByDescending(x => x.ConfigVersion)
            .First().ConfigVersion;

        // Manually find database version
        using var db = NwptDb.GetConnection();
        int databaseVersion = 0;
        var row = db.QuerySingleOrDefault<Setting>(
            "SELECT * FROM setting WHERE settingkey = @Key",
            new { Key = "InstalledConfigVersion" });
        if (row != null)
            databaseVersion = Convert.ToInt32(row.SettingValue);

        // Install new keys if needed
        if (applicationVersion > databaseVersion)
        {
            var keysToAdd = SettingInitializer.Keys.Where(x => x.ConfigVersion > databaseVersion);

            // Add values
            foreach (var key in keysToAdd)
                db.Execute("INSERT INTO setting (settingkey, settingvalue) VALUES (@SettingKey, @SettingValue)",
                new { 
                    SettingKey = key.Name, 
                    SettingValue = key.DefaultValue 
                });

            // Update database version
            Set<int>("InstalledConfigVersion", applicationVersion);
        }

        // Set update check done to true
        IsUpdateCheckDone = true;
    }
}