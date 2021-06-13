using QuickBulletLibrary.Models.Blocks.Extras;

namespace QuickBulletLibrary.Interfaces
{
    public interface ICondition
    {
        public ConditionPattern ConditionPattern { get; }

        public bool Execute(string source, string key);
    }
}