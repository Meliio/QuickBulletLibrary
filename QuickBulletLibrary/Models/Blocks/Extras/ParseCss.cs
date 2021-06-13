using AngleSharp.Html.Parser;
using QuickBulletLibrary.Interfaces;

namespace QuickBulletLibrary.Models.Blocks.Extras
{
    public class ParseCss : IParse
    {
        public string Execute(string source, string firstInput, string secondInput)
        {
            var htmlParser = new HtmlParser();
            var document = htmlParser.ParseDocument(source);

            return (secondInput.ToLower()) switch
            {
                "innerhtml" => document.QuerySelector(firstInput).InnerHtml,
                "outerhtml" => document.QuerySelector(firstInput).OuterHtml,
                "textcontent" => document.QuerySelector(firstInput).TextContent,
                _ => document.QuerySelector(firstInput).GetAttribute(secondInput)
            };
        }
    }
}
