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
		foreach (var item in foundItems)
		{
			var relatedPriceEntries = await Queries.GetPriceEntriesAsync(item.Id);
			results.Add(new()
			{
				Item = item,
				PriceEntries = relatedPriceEntries.ToList(),
			});
		}
		return results;
	}
}