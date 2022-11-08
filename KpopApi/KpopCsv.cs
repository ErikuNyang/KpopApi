namespace KpopApi.Data;
// ["song","release","summary","artist","debut"]
public class KpopCsv
{
    public string song { get; set; } = null!;
    public DateTime release { get; set; }
    public string? summary { get; set; }
    public string artist { get; set; } = null!;
    public int debut { get; set; }
}

