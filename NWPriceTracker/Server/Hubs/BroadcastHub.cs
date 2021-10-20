public class BroadcastHub : Hub
{
    public async Task SendPriceUpdateAsync(PriceEntry pe)
    {
        await Clients.All.SendAsync("UpdatePriceEntry", pe);
    }
}