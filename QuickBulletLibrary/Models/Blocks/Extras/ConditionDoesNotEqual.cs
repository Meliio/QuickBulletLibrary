using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionDoesNotEqual : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionDoesNotEqual(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (source != key)
            {
                return true;
            }
            return false;
        }
    }
}
