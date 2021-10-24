public class SettingInitializer
{
    public struct KeyData
    {
        public KeyData(string name, int confVersion, string defaultValue, string details = null)
        {
            Name = name;
            ConfigVersion = confVersion;
            Details = details;
            DefaultValue = defaultValue;
        }
        public string Name;
        public int ConfigVersion;
        public string Details;
        public string DefaultValue;
    }

    /// <summary>
    /// These values are placed in the database.
    /// If DbVersion of a KeyData > InstalledConfigVersion, any new values should be installed
    /// </summary>
    public static readonly List<KeyData> Keys = new List<KeyData>()
    {
        new KeyData("InstalledConfigVersion", 1, "0"),
        new KeyData("InstalledDatabaseVersion", 1, "0"),
        new KeyData("DiscordClientId", 1, ""),
        new KeyData("DiscordClientSecret", 1, ""),
    };

    public static string GetDescription(string confKey)
    {
        KeyData? kd = Keys.Where(x => x.Name == confKey).FirstOrDefault();
        return kd == null ? "" : kd.Value.Details;
    }
}