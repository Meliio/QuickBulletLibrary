using System;
using System.Collections.Generic;

namespace QuickBulletLibrary.Models
{
    public class BotInput : Dictionary<string, string>
    {
        public string Raw { get; }
        public bool IsValid { get; }

        public BotInput(string input, string inputSeparator, string[] inputNames) : base(StringComparer.OrdinalIgnoreCase)
        {
            Raw = input;

            var inputSplit = input.Split(inputSeparator, inputNames.Length);

            if (inputSplit.Length != inputNames.Length)
            {
                return;
            }

            for (int i = 0; i < inputSplit.Length; i++)
            {
                Add(inputNames[i], inputSplit[i]);
            }

            IsValid = true;
        }
    }
}
