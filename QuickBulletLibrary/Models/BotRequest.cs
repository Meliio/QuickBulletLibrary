using System;
using System.Collections.Generic;

namespace QuickBulletLibrary.Models
{
    public class BotRequest
    {
        public string StatusCode { get; set; }
        public string Uri { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> Headers { get; }
        public Dictionary<string, string> Cookies { get; }

        public BotRequest()
        {
            StatusCode = "0";
            Uri = string.Empty;
            Content = string.Empty;
            Cookies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
