public class ItemData
{
	public Item Item { get; set; }
	public List<PriceEntry> PriceEntries { get; set; }
	public bool IsEditingEnabled { get; set; } = false;
	public bool IsUserFavorite { get; set; } = false;
}