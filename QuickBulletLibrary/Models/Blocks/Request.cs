using System;

namespace QuickBulletLibrary.Models.Blocks
{
    public class Request
    {
        public string Url { get; set; } = "https://google.com";
        public string Method { get; set; } = "get";
        public string Data { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        public string[] Headers { get; set; } = Array.Empty<string>();
        public string[] Cookies { get; set; } = Array.Empty<string>();
        public bool LoadContent { get; set; } = true;
        public bool IsDisable { get; set; } = false;
    }
}
