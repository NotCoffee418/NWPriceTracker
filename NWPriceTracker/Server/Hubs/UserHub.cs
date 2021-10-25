using System.Text.RegularExpressions;

public class UserHub : Hub
{
    public async Task<List<Account>> GetAllAccounts()
        => await Queries.GetAllAccounts();

    public async Task AccountIsAuthorizedAsync(string discordHandle)
        => await Queries.AccountIsAuthorizedAsync(discordHandle);

    public async Task UpdateAccountInfoAsync(string discordHandle, string pfpUrl)
        => await Queries.UpdateAccountInfoAsync(discordHandle, pfpUrl);

    public async Task GiveAccountAccess(string discordHandle)
    {
        Regex r = new Regex(@"[a-zA-Z0-9_ ]*#[0-9]{4}");
        if (r.IsMatch(discordHandle))
            await Queries.GiveAccountAccess(discordHandle);
        else 
            throw new ArgumentException($"{discordHandle} does not appear to be a valid discord handle.");
    }
}