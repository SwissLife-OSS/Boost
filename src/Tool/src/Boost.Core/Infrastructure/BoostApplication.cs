namespace Boost.Infrastructure
{
    public class BoostApplication
    {
        public string WorkingDirectory { get; set; } = default!;

        public bool ConfigurationRequired { get; set; }

        public string Version { get; set; } = default!;
    }
}
