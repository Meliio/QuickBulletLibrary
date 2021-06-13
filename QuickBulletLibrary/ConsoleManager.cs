using System;
using System.Text;
using System.Threading.Tasks;

namespace QuickBulletLibrary
{
    public class ConsoleManager
    {
        private readonly Checker _checker;

        public ConsoleManager(Checker checker)
        {
            _checker = checker;
        }

        public async Task StartUpdatingTitleAsync()
        {
            var title = new StringBuilder();

            while (true)
            {
                title.Append("Success: ");
                title.Append(_checker.Success);

                title.Append(" Free: ");
                title.Append(_checker.Free);

                title.Append(" Unknown: ");
                title.Append(_checker.Unknown);

                title.Append(" Bad: ");
                title.Append(_checker.Bad);

                title.Append(" Retry: ");
                title.Append(_checker.Retry);

                title.Append(" Ban: ");
                title.Append(_checker.Ban);

                title.Append(" Checked: ");
                title.Append(_checker.Checked);

                title.Append(" [ CPM ");
                title.Append(_checker.Cpm);
                title.Append(" ] ");

                Console.Title = title.ToString();

                title.Clear();

                await Task.Delay(100);
            }
        }
    }
}
