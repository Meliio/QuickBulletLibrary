using QuickBulletLibrary.Blocks;
using QuickBulletLibrary.Enums;
using QuickBulletLibrary.Extentions;
using QuickBulletLibrary.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickBulletLibrary
{
    public class Checker
    {
        public CheckerStatus Status { get; private set; }

        public int Bad { get => _bad; }
        public int Success { get => _success; }
        public int Free { get => _free; }
        public int Unknown { get => _unknown; }
        public int Retry { get => _retry; }
        public int Ban { get => _ban; }
        public int Checked { get => _checked; }
        public int Cpm { get => _cpm; }

        private int _bad;
        private int _success;
        private int _free;
        private int _unknown;
        private int _retry;
        private int _ban;
        private int _checked;
        private int _cpm;

        private readonly CheckerSettings _checkerSettings;
        private readonly string[] _wordlist;
        private readonly ConfigSettings _configSettings;
        private readonly BlockBase[] _blockBases;
        private readonly HttpClientManager _httpClientManager;
        private readonly int _maxDegreeOfParallelism;
        private readonly BotStatus[] _validStatuses;
        private readonly ReaderWriterLock _readerWriterLock;

        public Checker(CheckerSettings checkerSettings, string[] wordlist, ConfigSettings configSettings, BlockBase[] blockBases, HttpClientManager httpClientManager, int maxDegreeOfParallelism)
        {
            Status = CheckerStatus.Idle;
            _checkerSettings = checkerSettings;
            _wordlist = wordlist;
            _configSettings = configSettings;
            _blockBases = blockBases;
            _httpClientManager = httpClientManager;
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
            _validStatuses = new BotStatus[] { BotStatus.Success, BotStatus.Free, BotStatus.Unknown };
            _readerWriterLock = new ReaderWriterLock();
        }

        public async Task StartAsync()
        {
            _ = StartCpmCalculator();

            Status = CheckerStatus.Running;

            await _wordlist.AsyncParallelForEach(async input =>
            {
                var botInput = new BotInput(input, _configSettings.InputSeparator, _configSettings.InputNames);

                if (botInput.IsValid)
                {
                    BotData botData;

                    while (true)
                    {
                        while (Status == CheckerStatus.Paused)
                        {
                            await Task.Delay(1000);
                        }

                        var customHttpClient = _httpClientManager.GetCustomHttpClient();

                        botData = new BotData(customHttpClient, botInput);

                        foreach (var block in _blockBases)
                        {
                            try
                            {
                                await block.Execute(botData);
                            }
                            catch
                            {
                                botData.Status = BotStatus.Retry;
                            }
                            if (botData.Status != BotStatus.None)
                            {
                                break;
                            }
                        }

                        if (botData.Status == BotStatus.Retry)
                        {
                            Interlocked.Increment(ref _retry);
                            continue;
                        }
                        else if (botData.Status == BotStatus.Ban)
                        {
                            customHttpClient.IsBanned = true;
                            Interlocked.Increment(ref _ban);
                            continue;
                        }

                        break;
                    }

                    if (botData.Status == BotStatus.Bad)
                    {
                        Interlocked.Increment(ref _bad);
                    }
                    else if (_validStatuses.Contains(botData.Status))
                    {
                        string outputPath = PathBuilder(new string[] { _checkerSettings.OutputDirectory, _configSettings.Name, botData.Status.ToString() }, "txt");
                        string output = OutputBuilder(botData);

                        //while (!await AppendToFile(outputPath, output))
                        //{
                        //    await Task.Delay(100);
                        //}

                        await AppendToFile(outputPath, output);

                        switch (botData.Status)
                        {
                            case BotStatus.Success:
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine(output);
                                Interlocked.Increment(ref _success);
                                break;
                            case BotStatus.Free:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine(output);
                                Interlocked.Increment(ref _free);
                                break;
                            case BotStatus.Unknown:
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine(output);
                                Interlocked.Increment(ref _unknown);
                                break;
                        }
                    }

                    Interlocked.Increment(ref _checked);
                }
            }, _maxDegreeOfParallelism);

            Status = CheckerStatus.Done;
        }

        public void Pause()
        {
            Status = CheckerStatus.Paused;
        }

        public void Resume()
        {
            Status = CheckerStatus.Running;
        }

        private string PathBuilder(string[] paths, string extension)
        {
            string fullPath = Path.Combine(paths);
            fullPath = Path.ChangeExtension(fullPath, extension);
            return fullPath;
        }

        private string OutputBuilder(BotData botData)
        {
            var output = new StringBuilder()
                .Append("Input = ")
                .Append(botData.Input.Raw);

            var botVariables = botData.Variables.Where(v => v.AddToOutput);

            if (botVariables.Count() == 0)
            {
                return output.ToString();
            }

            output.Append(_checkerSettings.OutputSeparator)
                .AppendJoin(_checkerSettings.OutputSeparator, botVariables.Select(c => $"{c.Name} = {c.Value}"));

            return output.ToString();
        }

        private async Task AppendToFile(string filePath, string content)
        {
            try
            {
                _readerWriterLock.AcquireWriterLock(int.MaxValue);
                using var streamWriter = File.AppendText(filePath);
                await streamWriter.WriteLineAsync(content);
            }
            finally
            {
                _readerWriterLock.ReleaseWriterLock();
            }
        }

        private async Task StartCpmCalculator()
        {
            while (true)
            {
                int checkedBefore = _checked;
                await Task.Delay(3000);
                int checkedAfter = _checked;
                _cpm = (checkedAfter - checkedBefore) * 20;
            }
        }
    }
}
