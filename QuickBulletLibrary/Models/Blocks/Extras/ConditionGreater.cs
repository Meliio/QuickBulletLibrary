using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionGreater : ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public ConditionGreater(ConditionPattern conditionPattern)
        {
            ConditionPattern = conditionPattern;
        }

        public bool Execute(string source, string key)
        {
            if (int.TryParse(source, out int sourceToInteger) && int.TryParse(key, out int keyToInteger))
            {
                if (sourceToInteger > keyToInteger)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
