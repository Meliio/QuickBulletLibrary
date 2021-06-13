using Newtonsoft.Json.Linq;
using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ParseJson : IParse
    {
        public string Execute(string source, string firstInput, string secondInput)
        {
            return JObject.Parse(source).SelectToken(firstInput).ToString();
        }
    }
}