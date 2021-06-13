using System.Net.Http;

namespace QuickBulletLibrary.Models
{
    public class CustomHttpClient : HttpClient
    {
        public Proxy Proxy { get; }
        public bool IsBanned { get; set; }

        public CustomHttpClient()
        {

        }

        public CustomHttpClient(HttpClientHandler httpClientHandler) : base(httpClientHandler)
        {

        }
    }
}
