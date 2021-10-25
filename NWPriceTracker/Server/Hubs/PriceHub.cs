public class PriceHub : Hub
{
    public async Task SendPriceUpdateAsync(PriceEntry pe)
    {
        await Clients.All.SendAsync("PriceEntryUpdated", pe);
    }

    public async Task UpdatePrice(PriceEntry pe)
    {
        // Create or update price entry
        if (pe.Id == 0)
            pe = await Queries.CreatePriceEntryAsync(pe);
        else
            await Queries.UpdatePriceAsync(pe);

        // Broadcast update
        await SendPriceUpdateAsync(pe);
    }
    public async Task ChangeItemFavoriteState(int itemId, bool isFavorite)
    {
        var discordUser = (ClaimsIdentity)Context.User.Identity;
        string discordHandle = discordUser.FindFirst(ClaimTypes.Name).Value + '#' +
                discordUser.Claims.Where(x => x.Type == "urn:discord:user:discriminator").First().Value;
        await Queries.ChangeItemFavoriteState(itemId, discordHandle, isFavorite);
    }
}