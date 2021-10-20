namespace NWPriceTracker.Server.Data
{
    public static class Queries
    {
        public static async Task<IEnumerable<Item>> SearchItem(string searchQuery, int linit = 20)
        {
            using var db = NwptDb.GetConnection();

            // morph search text
            searchQuery = searchQuery
                .Trim()
                .ToLowerInvariant()
                .Replace(' ', '+');

            // run query
            return await db.QueryAsync<Item>(
                $"SELECT * FROM item WHERE to_tsvector(name) @@ to_tsquery(@SearchQuery) LIMIT {linit}",
                new
                {
                    SearchQuery = searchQuery,
                });
        }

        /// <summary>
        /// Creates or updates the items table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task CreateUpdateItems(List<Item> data)
        {
            using var db = NwptDb.GetConnection();
            foreach (var item in data)
                await db.QueryAsync("CALL insert_update_item(@Id, @Name, @Alias, @Type, @Category, @Description, @Rarity, @Weight, @Icon)", item);
        }
    }
}