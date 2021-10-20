public class InstallerHub : Hub
{
    /// <summary>
    /// Installs or updates items database from an external API
    /// </summary>
    /// <returns></returns>
    public async Task<(bool, string)> InstallItems()
	{
        await InstallationHelper.InstallUpdateItemsAsync();
        return (true, "Successfully installed items");
    }
}