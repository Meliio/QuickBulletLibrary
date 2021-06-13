using QuickBulletLibrary.Interfaces;
using QuickBulletLibrary.Models;
using QuickBulletLibrary.Models.Blocks;
using QuickBulletLibrary.Models.Blocks.Extras;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuickBulletLibrary.Blocks
{
    public class BlockParse : BlockBase
    {
        private readonly Parse _parse;
        private readonly IParse _parseProcess;

        public BlockParse(Parse parse)
        {
            _parse = parse;

            switch (_parse.Methode.ToLower())
            {
                case "between":
                    _parseProcess = new ParseBetween();
                    break;
                case "css":
                    _parseProcess = new ParseCss();
                    break;
                case "json":
                    _parseProcess = new ParseJson();
                    break;
                case "regex":
                    _parseProcess = new ParseRegex();
                    break;
            }
        }

        public override Task Execute(BotData botData)
        {
            if (_parse.IsDisable)
            {
                return Task.CompletedTask;
            }

            string source = ReplaceValues(_parse.Source, botData);
            string firstInput = ReplaceValues(_parse.FirstInput, botData);
            string secondInput = ReplaceValues(_parse.SecondInput, botData);

            string parseResult = _parseProcess.Execute(source, firstInput, secondInput);

            if (string.IsNullOrEmpty(parseResult))
            {
                return Task.CompletedTask;
            }

            parseResult = _parse.Prefix + parseResult.Trim() + _parse.Suffix;

            if (botData.Variables.Any(v => v.Name.Equals(parseResult, StringComparison.OrdinalIgnoreCase)))
            {
                botData.Variables.Find(v => v.Name.Equals(parseResult, StringComparison.OrdinalIgnoreCase)).Value = parseResult;
            }
            else
            {
                botData.Variables.Add(new BotVariable(_parse.Name, parseResult)
                {
                    AddToOutput = _parse.AddToOutput
                });
            }

            return Task.CompletedTask;
        }
    }
}
