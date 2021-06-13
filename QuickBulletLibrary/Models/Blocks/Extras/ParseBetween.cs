using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ParseBetween : IParse
    {
        public string Execute(string source, string firstInput, string secondInput)
        {
            int indexOfBegin = source.IndexOf(firstInput);

            if (indexOfBegin != -1)
            {
                source = source.Substring(indexOfBegin + firstInput.Length);

                int indexOfEnd = source.IndexOf(secondInput);

                if (indexOfEnd != -1)
                {
                    return source.Substring(0, indexOfEnd);
                }
            }

            return null;
        }
    }
}
