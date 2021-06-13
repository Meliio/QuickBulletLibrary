using QuickBulletLibrary.Enums;
using System.Collections.Generic;
using System.Net.Http;

namespace QuickBulletLibrary.Models
{
    public class BotData
    {
        public HttpClient HttpClient { get; }
        public BotInput Input { get; }
        public BotRequest Request { get; }
        public BotStatus Status { get; set; }
        public List<BotVariable> Variables { get; }

        public BotData(HttpClient httpClient, BotInput input)
        {
            HttpClient = httpClient;
            Input = input;
            Request = new BotRequest();
            Status = BotStatus.None;
            Variables = new List<BotVariable>();
        }
    }
}