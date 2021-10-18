namespace NWPriceTracker.Server.Data
{
    public static class Queries
    {
        public static List<Item> SearchItem(string searchQuery)
        {
            throw new NotImplementedException();
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