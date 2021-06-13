using QuickBulletLibrary.Enums;
using QuickBulletLibrary.Interfaces;
using QuickBulletLibrary.Models;
using QuickBulletLibrary.Models.Blocks;
using QuickBulletLibrary.Models.Blocks.Extras;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickBulletLibrary.Blocks
{
    public class BlockCondition : BlockBase
    {
        private readonly Condition _condition;
        private readonly ICondition[] _conditionsProcess;

        public BlockCondition(Condition condition)
        {
            _condition = condition;

            var conditionsProcess = new List<ICondition>();

            foreach (var pattern in condition.Patterns)
            {
                switch (pattern.Condition.ToLower())
                {
                    case "equal":
                        conditionsProcess.Add(new ConditionEqual(pattern));
                        break;
                    case "contains":
                        conditionsProcess.Add(new ConditionContains(pattern));
                        break;
                    case "smaller":
                        conditionsProcess.Add(new ConditionSmaller(pattern));
                        break;
                    case "greater":
                        conditionsProcess.Add(new ConditionGreater(pattern));
                        break;
                    case "regexmatch":
                        conditionsProcess.Add(new ConditionRegexMatch(pattern));
                        break;
                }
            }

            _conditionsProcess = conditionsProcess.ToArray();
        }

        public override Task Execute(BotData botData)
        {
            if (_condition.IsDisable)
            {
                return Task.CompletedTask;
            }

            foreach (var condition in _conditionsProcess)
            {
                string source = ReplaceValues(condition.ConditionPattern.Source, botData);
                string key = ReplaceValues(condition.ConditionPattern.Key, botData);

                if (condition.Execute(source, key))
                {
                    botData.Status = Enum.Parse<BotStatus>(condition.ConditionPattern.Status, true);
                    break;
                }
                botData.Status = BotStatus.Ban;
            }

            return Task.CompletedTask;
        }
    }
}
