public class ItemData
{
	public Item Item { get; set; }
	public List<PriceEntry> PriceEntries { get; set; }
	public bool IsEditingEnabled { get; set; } = true;
}