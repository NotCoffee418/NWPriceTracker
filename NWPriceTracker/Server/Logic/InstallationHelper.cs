﻿namespace NWPriceTracker.Server.Logic
{
    public static class InstallationHelper
    {
        /// <summary>
        /// Gets items from NewWorldForge and puts them in the database, or updates them
        /// </summary>
        /// <returns></returns>
        public static async Task InstallUpdateItemsAsync()
        {
            var items = await GetItemsFromForgeAsync();
            await Queries.CreateUpdateItemsAsync(items);
        }

        private static async Task<List<Item>> GetItemsFromForgeAsync()
        {
            bool hasData = false;
            List<Item> result = new();
            int pageNum = 1;
            var client = new HttpClient();
            do
            {
                // prepare url for this pagenum
                string url = $"https://api.newworldforge.com/api/web/items?path=%2Fdatabase%2Fitems&sort=id&dir=desc&page={pageNum}&limit=1000&search_query=";

                // get page from newworldforgeapi
                string json = await client.GetStringAsync(url);
                dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                List<Item> newItems = new List<Item>();
                foreach (var row in data.rows)
                {
                    try
                    {
                        newItems.Add(new Item()
                        {
                            Id = row.id,
                            Name = row.title,
                            Alias = row.alias,
                            Type = row.type,
                            Category = row.category,
                            Description = row.description,
                            Rarity = row.rarity,
                            Weight = row.weight,
                            Icon = row.icon,
                        });
                    }
                    catch (Exception ex)
                    {
                        return "fail on " + row.ToString() + "with error: " + ex.Message;
                    }
                }

                // increase pagenum and check if we had data
                pageNum++;
                hasData = newItems.Count > 0;
                result.AddRange(newItems);

            } while (hasData);
            return result;
        }
    }
}
