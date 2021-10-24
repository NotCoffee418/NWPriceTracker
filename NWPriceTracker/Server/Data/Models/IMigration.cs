namespace NWPriceTracker.Server.Data.Models
{
    public interface IMigration
    {
        public int DbVersion { get; }
        public string UpgradeSql { get; }
    }
}
