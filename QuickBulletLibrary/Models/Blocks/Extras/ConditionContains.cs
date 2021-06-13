using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionContains : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionContains(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (source.Contains(key))
            {
                return true;
            }
            return false;
        }
    }
}
