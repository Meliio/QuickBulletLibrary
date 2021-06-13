namespace QuickBulletLibrary.Models.Blocks
{
    public class Parse
    {
        public string Name { get; set; } = "parseName";
        public string Prefix { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public string Source { get; set; } = "{request.content}";
        public string Methode { get; set; } = "between";
        public string FirstInput { get; set; } = string.Empty;
        public string SecondInput { get; set; } = string.Empty;
        public bool AddToOutput { get; set; } = false;
        public bool IsDisable { get; set; } = false;
    }
}
