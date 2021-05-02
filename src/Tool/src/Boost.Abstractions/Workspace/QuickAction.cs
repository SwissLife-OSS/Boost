namespace Boost.Workspace
{
    public class QuickAction
    {
        public string Type { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public string Value { get; set; } = default!;

        public override string ToString()
        {
            switch (Type)
            {
                case QuickActionTypes.OpenVisualStudioSolution:
                    return $"Open {Value}";
                case QuickActionTypes.OpenDirectoryInExplorer:
                    return "Open in explorer";
                case QuickActionTypes.OpenDirectoryInCode:
                    return "Open in Code";
                case QuickActionTypes.OpenDirectoryInTerminal:
                    return "Open in Terminal";
                case QuickActionTypes.RunSuperBoost:
                    return $"SuperBoost: {Title}";
                default:
                    return Title;
            }
        }
    }
}
