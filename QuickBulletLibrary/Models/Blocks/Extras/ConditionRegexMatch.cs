using QuickBulletLibrary.Interfaces;
using System.Text.RegularExpressions;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionRegexMatch : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionRegexMatch(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (Regex.IsMatch(source, key))
            {
                return true;
            }
            return false;
        }
    }
}
