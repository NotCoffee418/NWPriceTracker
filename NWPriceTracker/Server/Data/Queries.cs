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

        internal static async Task<bool> IsItemFavoritedByUserAsync(int itemId, string discordHandle)
        {
            using var db = NwptDb.GetConnection();
            return await db.ExecuteScalarAsync<bool>("SELECT EXISTS (" +
                "SELECT * FROM favorite WHERE itemid = @ItemId AND userid = (" +
                    "SELECT id FROM account WHERE LOWER(discordhandle) = LOWER(@DiscordHandle)" +
                "))", new
                {
                    DiscordHandle = discordHandle,
                    ItemId = itemId
                });
        }

        internal static async Task ChangeItemFavoriteState(int itemId, string discordHandle, bool isFavorite)
        {
            using var db = NwptDb.GetConnection();
            int userId = await db.ExecuteScalarAsync<int>(
                "SELECT id FROM account WHERE LOWER(discordhandle) = LOWER(@DiscordHandle)",
                new { DiscordHandle = discordHandle });

            if (isFavorite)
            {
                await db.ExecuteAsync(
                    "INSERT INTO favorite (itemid, userid) VALUES (@ItemId, @UserId) ON CONFLICT DO NOTHING",
                    new
                    {
                        UserId = userId,
                        ItemId = itemId
                    });
            }
            else // unfavorite
            {
                await db.ExecuteAsync(
                    "DELETE FROM favorite WHERE itemid = @ItemId AND userid = @UserId",
                    new
                    {
                        UserId = userId,
                        ItemId = itemId
                    });
            }
        }

        internal static async Task<List<Item>> GetUserFavoritesAsync(string discordHandle)
        {
            using var db = NwptDb.GetConnection();
            return (await db.QueryAsync<Item>(
                "select item.* from item " +
                "JOIN favorite ON favorite.itemid = item.id " +
                "JOIN account ON account.id = favorite.userid " +
                "WHERE LOWER(account.discordhandle) = LOWER(@DiscordHandle)",
                new { DiscordHandle = discordHandle })).ToList();
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

        internal static async Task<IEnumerable<Setting>> GetAllSettingsAsync()
        {
            using var db = NwptDb.GetConnection();
            return await db.QueryAsync<Setting>("SELECT * FROM setting");
        }

        internal static async Task<Setting> GetSettingAsync(string settingKey)
        {
            using var db = NwptDb.GetConnection();
            return await db.QueryFirstOrDefaultAsync<Setting>(
                "SELECT * FROM setting WHERE settingkey = @SettingKey",
                new { SettingKey = settingKey });
        }

        internal static async Task UpdateSettingAsync(string settingKey, string settingValue)
        {
            using var db = NwptDb.GetConnection();
            await db.ExecuteAsync(
                "UPDATE setting SET settingvalue = @SettingValue WHERE settingkey = @SettingKey",
                new
                {
                    SettingKey = settingKey,
                    SettingValue = settingValue
                });
        }

        internal static async Task<List<Account>> GetAllAccounts()
        {
            using var db = NwptDb.GetConnection();
            return (await db.QueryAsync<Account>("SELECT * FROM account")).ToList();
        }

        internal static async Task<bool> AccountIsAuthorizedAsync(string discordHandle)
        {
            using var db = NwptDb.GetConnection();
            return await db.ExecuteScalarAsync<bool>(
                "SELECT EXISTS(SELECT id FROM account WHERE LOWER(discordhandle) = LOWER(@DiscordHandle))",
                new { DiscordHandle = discordHandle });
        }

        internal static async Task UpdateAccountInfoAsync(string discordHandle, string pfpUrl)
        {
            using var db = NwptDb.GetConnection();
            await db.ExecuteAsync(
                "UPDATE account SET profilepictureurl = @PfpUrl WHERE discordhandle = @DiscordHandle",
                new { 
                    DiscordHandle = discordHandle,
                    PfpUrl = pfpUrl
                });
        }

        internal static async Task GiveAccountAccess(string discordHandle)
        {
            using var db = NwptDb.GetConnection();
            await db.ExecuteAsync(
                "INSERT INTO account (discordhandle, profilepictureurl) VALUES (@DiscordHandle, @PfpUrl)",
                new { 
                    DiscordHandle = discordHandle,
                    PfpUrl = "/img/unknown-user.png"
                });
        }
    }
}