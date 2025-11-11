using Nizzola.ToonConvert;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class User2
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public List<string> Interests { get; set; } = new();

    [ToonIgnore]
    public string InternalField { get; set; } = string.Empty;
}
