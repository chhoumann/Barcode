using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcode
{
    public sealed class FileLog : ILog
    {
        private const string LogFileName = "log.txt";
        private readonly string _logDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
        private string _logFilePath;
        public List<string> LogEntries { get; private set; }

        public FileLog()
        {
            _logFilePath = Path.Combine(_logDirectoryPath, LogFileName);
            LogEntries = new List<string>();
            
            try
            {
                Task.Run(async () => LogEntries = await ReadLogs());
            }
            catch (FileNotFoundException e)
            {
                File.Create(_logFilePath);
            }
        }

        private async Task<List<string>> ReadLogs()
        {
            if(File.Exists(_logFilePath)) 
                return (await File.ReadAllLinesAsync(Path.Combine(_logDirectoryPath, LogFileName))).ToList();
            
            throw new FileNotFoundException("Log file not found");
        }

        public void AddLogEntry(object sender)
        {
            LogEntries.Add(sender.ToString());
            
            File.AppendAllLines(_logFilePath, new string[1] {sender.ToString()});
        }
    }
}