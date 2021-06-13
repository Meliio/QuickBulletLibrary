using QuickBulletLibrary.Interfaces;
using System.Text.RegularExpressions;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ParseRegex : IParse
    {
        public string Execute(string source, string firstInput, string secondInput)
        {
            var matche = Regex.Match(source, firstInput);

            if (matche.Success)
            {
                return matche.Groups[secondInput].Value;
            }

            return null;
        }
    }
}
