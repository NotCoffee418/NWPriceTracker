using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWPriceTracker.Shared.DbModels
{
    public class Account
    {
        public int Id { get; set; }
        public string DiscordHandle { get; set; }
        public string ProfilePictureUrl { get; set; }

        public static bool IsAuthorized(string email)
        {
            throw new NotImplementedException();
        }
    }
}
