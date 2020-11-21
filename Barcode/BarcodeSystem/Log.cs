using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Barcode
{
    public class Log
    {
        private readonly string _logPath = AppDomain.CurrentDomain.BaseDirectory;
        private const string LogFileName = "log.txt";
        private List<string> _logEntries;

        private static Lazy<Log> instance = new Lazy<Log>(() => new Log());
        public static Log Instance => instance.Value;

        public Log()
        {
            _logEntries = File.ReadAllLines(Path.Combine(_logPath, LogFileName)).ToList();
        }
        
        public void AddLog(object sender)
        {
            _logEntries.Add(sender.ToString());
            
            File.WriteAllLines(_logPath, _logEntries);
        }
    }
}