using QuickBulletLibrary.Blocks;
using QuickBulletLibrary.Models;
using QuickBulletLibrary.Models.Blocks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace QuickBulletLibrary
{
    public class CheckerBuilder
    {
        public int MaxDegreeOfParallelism { get; set; } = 1;

        private readonly string[] _wordlist;
        private readonly ConfigSettings _configSettings;
        private readonly BlockBase[] _blockBases;
        private readonly CheckerSettings _checkerSettings;
        private readonly HttpClientManager _httpClientManager;

        private const string CHECKER_SETTINGS_FILE = "settings.json";

        public CheckerBuilder(string wordlistPath, string configPath, string proxiesPath = null)
        {
            _wordlist = File.ReadAllLines(wordlistPath);

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _configSettings = JsonSerializer.Deserialize<ConfigSettings>(JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(configPath)).GetProperty("settings").GetRawText(), jsonSerializerOptions);

            if (string.IsNullOrEmpty(_configSettings.Name))
            {
                _configSettings.Name = Path.GetFileNameWithoutExtension(configPath);
            }

            var jsonElements = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(configPath), jsonSerializerOptions).GetProperty("blocks").EnumerateArray();

            var blockBases = new List<BlockBase>();

            foreach (var jsonElement in jsonElements)
            {
                switch (jsonElement.GetProperty("block").GetString().ToLower())
                {
                    case "request":
                        blockBases.Add(new BlockRequest(JsonSerializer.Deserialize<Request>(jsonElement.GetRawText(), jsonSerializerOptions)));
                        break;
                    case "parse":
                        blockBases.Add(new BlockParse(JsonSerializer.Deserialize<Parse>(jsonElement.GetRawText(), jsonSerializerOptions)));
                        break;
                    case "condition":
                        blockBases.Add(new BlockCondition(JsonSerializer.Deserialize<Condition>(jsonElement.GetRawText(), jsonSerializerOptions)));
                        break;
                }
            }

            _blockBases = blockBases.ToArray();
            _checkerSettings = File.Exists(CHECKER_SETTINGS_FILE) ? JsonSerializer.Deserialize<CheckerSettings>(File.ReadAllText(CHECKER_SETTINGS_FILE), jsonSerializerOptions) : new CheckerSettings();         
            _httpClientManager = new HttpClientManager();
        }

        public void AddProxies(string proxiesPath)
        {
            var proxies = File.ReadAllLines(proxiesPath);
            _httpClientManager.AddCustomHttpClient(proxies.Select(p => new Proxy(p)).Select(p => new CustomHttpClient(new HttpClientHandler() { Proxy = p.WebProxy })).ToArray());
            _httpClientManager.UseProxy = true;
        }

        public Checker Build()
        {
            Directory.CreateDirectory(Path.Combine(_checkerSettings.OutputDirectory, _configSettings.Name));
            return new Checker(_checkerSettings, _wordlist, _configSettings, _blockBases, _httpClientManager, MaxDegreeOfParallelism);
        }
    }
}