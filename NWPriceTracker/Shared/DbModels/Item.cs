using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWPriceTracker.Shared.DbModels
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Rarity { get; set; }
        public decimal? Weight { get; set; }
        public string Icon { get; set; }


    }
}
