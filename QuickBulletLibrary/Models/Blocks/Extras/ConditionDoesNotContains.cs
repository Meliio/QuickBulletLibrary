using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionDoesNotContains : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionDoesNotContains(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (!source.Contains(key))
            {
                return true;
            }
            return false;
        }
    }
}
