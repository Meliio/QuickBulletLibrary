namespace QuickBulletLibrary.Models
{
    public class BotVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool AddToOutput { get; set; }

        public BotVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
