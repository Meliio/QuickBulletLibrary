using QuickBulletLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickBulletLibrary
{
    public class HttpClientManager
    {
        public bool UseProxy { get; set; }

        private readonly CustomHttpClient _customHttpClient;
        private readonly List<CustomHttpClient> _customHttpClients;
        private readonly Random _random;

        public HttpClientManager()
        {
            _customHttpClient = new CustomHttpClient();
            _customHttpClients = new List<CustomHttpClient>();
            _random = new Random();
        }

        public void AddCustomHttpClient(CustomHttpClient[] customHttpClients)
        {
            _customHttpClients.AddRange(customHttpClients);
        }

        public CustomHttpClient GetCustomHttpClient()
        {
            if (UseProxy)
            {
                lock (_customHttpClients)
                {
                    if (_customHttpClients.All(h => h.IsBanned))
                    {
                        foreach (var httpClient in _customHttpClients)
                        {
                            httpClient.IsBanned = false;
                        }
                    }
                    var customHttpClients = _customHttpClients.Where(h => !h.IsBanned);
                    return customHttpClients.ElementAt(_random.Next(customHttpClients.Count()));
                }
            }
            return _customHttpClient;
        }
    }
}
