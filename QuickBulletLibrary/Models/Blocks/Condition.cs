using QuickBulletLibrary.Models.Blocks.Extras;
using System;

namespace QuickBulletLibrary.Models.Blocks
{
    public class Condition
    {
        public ConditionPattern[] Patterns { get; set; } = Array.Empty<ConditionPattern>();
        public bool IsDisable { get; set; } = false;
    }
}
