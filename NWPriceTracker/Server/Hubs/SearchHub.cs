public class SearchHub : Hub
{
	/// <summary>
	/// Receive search request and sends response with items
	/// </summary>
	/// <param name="searchQuery"></param>
	/// <returns></returns>
	public async Task<List<ItemData>> RequestSearch(string searchQuery)
	{
		// generate result
		List<ItemData> results = new();
		var foundItems = await Queries.SearchItemsAsync(searchQuery);
		var discordUser = (ClaimsIdentity)Context.User.Identity;
		string discordHandle = discordUser.FindFirst(ClaimTypes.Name).Value + '#' +
				discordUser.Claims.Where(x => x.Type == "urn:discord:user:discriminator").First().Value;

		foreach (var item in foundItems)
		{
			bool isUserFavorite = await Queries.IsItemFavoritedByUserAsync(item.Id, discordHandle);
			var relatedPriceEntries = await Queries.GetPriceEntriesAsync(item.Id);
			results.Add(new()
			{
				Item = item,
				PriceEntries = relatedPriceEntries.ToList(),
				IsUserFavorite = isUserFavorite,
			});
		}
		return results;
	}

	public async Task<List<ItemData>> GetUserFavorites()
	{
		// generate result
		List<ItemData> results = new();
		var discordUser = (ClaimsIdentity)Context.User.Identity;
		string discordHandle = discordUser.FindFirst(ClaimTypes.Name).Value + '#' +
				discordUser.Claims.Where(x => x.Type == "urn:discord:user:discriminator").First().Value;
		var foundItems = await Queries.GetUserFavoritesAsync(discordHandle);

		foreach (var item in foundItems)
		{
			bool isUserFavorite = await Queries.IsItemFavoritedByUserAsync(item.Id, discordHandle);
			var relatedPriceEntries = await Queries.GetPriceEntriesAsync(item.Id);
			results.Add(new()
			{
				Item = item,
				PriceEntries = relatedPriceEntries.ToList(),
				IsUserFavorite = isUserFavorite,
			});
		}
		return results;
	}
}