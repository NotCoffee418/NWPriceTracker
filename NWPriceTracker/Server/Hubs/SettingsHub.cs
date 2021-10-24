public class SettingsHub : Hub
{
    public async Task<List<Setting>> GetAllSettings()
        => (await Queries.GetAllSettingsAsync()).ToList();

    public async Task<Setting> GetSetting(string settingKey)
        => await Queries.GetSettingAsync(settingKey);

    public async Task UpdateSetting(string settingKey, string settingValue)
        => await Queries.UpdateSettingAsync(settingKey, settingValue);

}