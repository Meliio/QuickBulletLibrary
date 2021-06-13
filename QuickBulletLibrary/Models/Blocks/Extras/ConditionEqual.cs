using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionEqual : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionEqual(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (source == key)
            {
                return true;
            }
            return false;
        }
    }
}
