public class InstallerHub : Hub
{
    /// <summary>
    /// Installs or updates items database from an external API
    /// </summary>
    /// <returns></returns>
    public async Task<(bool, string)> InstallItems()
	{
        await InstallationHelper.InstallUpdateItemsAsync(this);
        return (true, "Successfully installed items");
    }

    public async Task SendStatusUpdate(string installerName, string statusMsg)
        => await Clients.All.SendAsync("OnStatusUpdate", installerName, statusMsg);
}