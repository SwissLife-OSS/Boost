namespace Boost;

public class ToolInfo
{
    public string Name { get; set; }

    public string Path { get; set; }

    public ToolType Type { get; set; }

    public bool IsDefault { get; set; }
}

public enum ToolType
{
    Shell,
    IDE,
    Other
}
