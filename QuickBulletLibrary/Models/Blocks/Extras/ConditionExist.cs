using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionExist : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionExist(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (source != ConditionPattern.Source)
            {
                return true;
            }
            return false;
        }
    }
}

