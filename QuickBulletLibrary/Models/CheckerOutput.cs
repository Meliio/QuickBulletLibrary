using QuickBulletLibrary.Enums;
using System;

namespace QuickBulletLibrary.Models
{
    public class CheckerOutput
    {
        public BotStatus Status { get; }
        public string Input { get; }
        public string Output { get; }
        public DateTime DateTime => new DateTime(1970, 1, 1).AddSeconds(_unixDate);

        private readonly int _unixDate;

        public CheckerOutput(BotStatus botStatus, string input, string output)
        {
            Status = botStatus;
            Input = input;
            Output = output;
            _unixDate = (int)Math.Round((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
        }
    }
}
