public class ZoneInfo
{
    public ZoneInfo(int id, string shortName, string fullName)
    {
        Id = id;
        ShortName = shortName;
        FullName = fullName;
    }

    public int Id { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }

}