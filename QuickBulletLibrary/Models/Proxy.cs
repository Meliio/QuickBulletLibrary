using System;
using System.Net;

namespace QuickBulletLibrary.Models
{
    public class Proxy
    {
        public string Host { get; }
        public string Port { get; }
        public string Username { get; }
        public string Password { get; }
        public WebProxy WebProxy { get; }

        private const char SPLIT_SEPARATOR = ':';
        private const int SPLIT_COUNT_WITH_CREDENTIALS = 4;

        public Proxy(string proxy)
        {
            var proxySplit = proxy.Split(SPLIT_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

            Host = proxySplit[0];
            Port = proxySplit[1];

            WebProxy = new WebProxy(new Uri($"http://{Host}:{Port}"));

            if (proxySplit.Length == SPLIT_COUNT_WITH_CREDENTIALS)
            {
                Username = proxySplit[2];
                Password = proxySplit[3];
                WebProxy.Credentials = new NetworkCredential(Username, Password);
            }
        }


    }
}
