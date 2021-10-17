using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWPriceTracker.Shared.DbModels
{
    public class PriceEntry
    {
        public int Id { get; set; }
        public Item? TargetItem { get; set; }
        public int TargetItemId { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
