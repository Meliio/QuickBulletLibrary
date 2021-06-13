using CommandLine;
using QuickBulletLibrary;
using System;
using System.Threading.Tasks;

namespace TestQuickBulletLibrary
{
    class Program
    {
        class Options
        {
            [Option('w', Required = true, HelpText = "Set wordlist path.")]
            public string WordlistPath { get; set; }

            [Option('c', Required = true, HelpText = "Set config path.")]
            public string ConfigPath { get; set; }

            [Option('p', Required = false, HelpText = "Set proxies path.")]
            public string ProxiesPath { get; set; }

            [Option('m', Required = false, HelpText = "Set maxDegreeOfParallelism.", Default = 1)]
            public int MaxDegreeOfParallelism { get; set; }
        }

        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(Run);
        }

        private static async Task Run(Options options)
        {
            var checkerBuilder = new CheckerBuilder(options.WordlistPath, options.ConfigPath)
            {
                MaxDegreeOfParallelism = options.MaxDegreeOfParallelism
            };

            if (options.ProxiesPath != null)
            {
                checkerBuilder.AddProxies(options.ProxiesPath);
            }

            var checker = checkerBuilder.Build();

            var consoleManager = new ConsoleManager(checker);

            _ = consoleManager.StartUpdatingTitleAsync();

            await checker.StartAsync();

            Console.WriteLine(checker.Status);
        }
    }
}
