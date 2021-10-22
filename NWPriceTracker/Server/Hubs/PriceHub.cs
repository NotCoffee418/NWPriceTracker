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
}