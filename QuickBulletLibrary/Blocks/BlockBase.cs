using QuickBulletLibrary.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuickBulletLibrary.Blocks
{
    public abstract class BlockBase
    {
        private readonly Regex _regex;

        private const string PATTERN = @"{(\w+)\.(\w+)(?:\[(\w+)\])?}";

        public BlockBase()
        {
            _regex = new Regex(PATTERN, RegexOptions.Compiled);
        }

        public abstract Task Execute(BotData botData);

        protected string ReplaceValues(string input, BotData botData)
        {
            input = input.Trim().ToLower();

            var matches = _regex.Matches(input);

            foreach (Match match in matches)
            {
                switch (match.Groups[1].Value)
                {
                    case "input":
                        switch (match.Groups[2].Value)
                        {
                            case "raw":
                                input = input.Replace(match.Value, botData.Input.Raw);
                                break;
                            default:
                                if (botData.Input.TryGetValue(match.Groups[2].Value, out string inputValue))
                                {
                                    input = input.Replace(match.Value, inputValue);
                                }
                                break;
                        }
                        break;
                    case "request":
                        switch (match.Groups[2].Value)
                        {
                            case "statuscode":
                                input = input.Replace(match.Value, botData.Request.StatusCode);
                                break;
                            case "uri":
                                input = input.Replace(match.Value, botData.Request.Uri);
                                break;
                            case "content":
                                input = input.Replace(match.Value, botData.Request.Content);
                                break;
                            case "headers":
                                if (botData.Request.Headers.TryGetValue(match.Groups[3].Value, out string headerValue))
                                {
                                    input = input.Replace(match.Value, headerValue);
                                }
                                break;
                            case "cookies":
                                if (botData.Request.Cookies.TryGetValue(match.Groups[3].Value, out string cookieValue))
                                {
                                    input = input.Replace(match.Value, cookieValue);
                                }
                                break;
                        }
                        break;
                    case "bot":
                        switch (match.Groups[2].Value)
                        {
                            case "status":
                                input = input.Replace(match.Value, botData.Status.ToString());
                                break;
                            default:
                                if (botData.Variables.Any(v => v.Name.Equals(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase)))
                                {
                                    input = input.Replace(match.Value, botData.Variables.Find(v => v.Name.Equals(match.Groups[2].Value, StringComparison.OrdinalIgnoreCase)).Value);
                                }
                                break;
                        }
                        break;
                }
            }

            return input;
        }
    }
}
