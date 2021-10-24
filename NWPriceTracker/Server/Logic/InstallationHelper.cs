namespace NWPriceTracker.Server.Logic
{
    public static class InstallationHelper
    {
        /// <summary>
        /// Gets items from NewWorldForge and puts them in the database, or updates them
        /// </summary>
        /// <returns></returns>
        public static async Task InstallUpdateItemsAsync(InstallerHub hub)
        {
            var items = await GetItemsFromForgeAsync(hub);
            await Queries.CreateUpdateItemsAsync(items);
        }

        private static async Task<List<Item>> GetItemsFromForgeAsync(InstallerHub hub)
        {
            bool hasData = false;
            List<Item> result = new();
            int pageNum = 1;
            var client = new HttpClient();

            // url will return html unless this is specified, other headers may be needed if they modify
            client.DefaultRequestHeaders.Add("accept", "application/json; charset=utf-8");

            do
            {
                // prepare url for this pagenum
                string url = $"https://newworldfans.com/db?page={pageNum}&category=Items&sort=name&dir=asc&locale=en";

                // get page from newworldforgeapi
                string json = await client.GetStringAsync(url);
                dynamic rows;
                try
                {
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                    rows = data.subjects.data;
                }
                catch
                {
                    throw new Exception("Item install returned non-json on " + url);
                    hasData = false;
                    break;
                }

                List<Item> newItems = new List<Item>();
                foreach (var row in rows)
                {
                    try
                    {
                        newItems.Add(new Item()
                        {
                            Id = row.id,
                            Name = row.attributes.name_with_affixes ?? row.attributes.name,
                            Alias = row.attributes.slug ?? ((string)row.attributes.name).ToLower().Replace(' ','-'),
                            Type = row.attributes.item_type,
                            Category = row.attributes.item_class_en,
                            Description = row.attributes.description,
                            Rarity = row.attributes.rarity,
                            Tier = row.attributes.tier,
                            Icon = row.attributes.cdn_asset_path,
                        });
                    }
                    catch (Exception ex)
                    {
                        return "fail on " + row.ToString() + "with error: " + ex.Message;
                    }
                }

                // increase pagenum and check if we had data
                hasData = newItems.Count > 0;
                result.AddRange(newItems);

                // Delay to prevent 429 (Too Many Requests)
                await hub.SendStatusUpdate("InstallItems", $"Installing: at page {pageNum}");
                await Task.Delay(100);
                pageNum++;
            } while (hasData);
            return result;
        }
    }
}
