namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ConditionPattern
    {
        public string Status { get; set; } = "bad";
        public string Source { get; set; } = "{request.content}";
        public string Condition { get; set; } = "contains";
        public string Key { get; set; } = string.Empty;
    }
}
