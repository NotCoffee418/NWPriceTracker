namespace NWPriceTracker.Server.Data
{
    public static class Queries
    {
        public static async Task<IEnumerable<Item>> SearchItemsAsync(string searchQuery, int limit = 20)
        {
            using var db = NwptDb.GetConnection();

            // morph search text
            searchQuery = searchQuery
                .Trim()
                .ToLowerInvariant();
            searchQuery = $"%{searchQuery}%";

            // run query
            var result = await db.QueryAsync<Item>(
                $"SELECT * FROM item WHERE LOWER(name) LIKE LOWER(@SearchQuery) LIMIT {limit}",
                new
                {
                    SearchQuery = searchQuery,
                });
            return result;
        }

        public static async Task<IEnumerable<PriceEntry>> GetPriceEntriesAsync(int itemId)
		{
            using var db = NwptDb.GetConnection();
            return await db.QueryAsync<PriceEntry>(
                $"SELECT * FROM priceentry WHERE targetitemid = @ItemId",
                new
                {
                    ItemId = itemId,
                });
        }
        
        /// <summary>
        /// Creates or updates the items table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task CreateUpdateItemsAsync(List<Item> data)
        {
            using var db = NwptDb.GetConnection();
            foreach (var item in data)
                await db.QueryAsync("CALL insert_update_item(@Id, @Name, @Alias, @Type, @Category, @Description, @Rarity, @Tier, @Icon)", item);
        }

        internal static async Task UpdatePriceAsync(PriceEntry pe)
        {
            using var db = NwptDb.GetConnection();
            pe.UpdatedTime = DateTime.UtcNow;
            await db.QueryAsync("UPDATE priceentry SET price = @Price, UpdatedTime = @UpdatedTime WHERE id = @Id", pe);
        }

        internal static async Task<PriceEntry> CreatePriceEntryAsync(PriceEntry pe)
        {
            using var db = NwptDb.GetConnection();
            pe.UpdatedTime = DateTime.UtcNow;
            return await db.QueryFirstAsync<PriceEntry>("INSERT INTO priceentry (targetitemid, targetarea, price, updatedtime) " +
                "VALUES (@TargetItemId, @TargetArea, @Price, @UpdatedTime) RETURNING *", pe);
        }
    }
}