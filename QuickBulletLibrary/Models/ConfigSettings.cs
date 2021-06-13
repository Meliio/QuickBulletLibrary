using System;

namespace QuickBulletLibrary.Models
{
    public class ConfigSettings
    {
        public string Name { get; set; } = string.Empty;
        public string InputSeparator { get; set; } = string.Empty;
        public string[] InputNames { get; set; } = Array.Empty<string>();
    }
}
