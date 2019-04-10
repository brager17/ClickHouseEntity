using System;
using System.IO;

namespace Context
{
    public class ConsoleDbLogger : IDbLogger
    {
        public void WriteLog(LogLevel logLevel, string log)
        {
            Console.WriteLine(log);
        }
    }

    public class StubDBLogger : IDbLogger
    {
        public void WriteLog(LogLevel logLevel, string log)
        {
        }
    }

    public class FileDbLogger : IDbLogger
    {
        private readonly string _path;

        public FileDbLogger(string path)
        {
            _path = path;
        }

        public void WriteLog(LogLevel logLevel, string log)
        {
            File.AppendAllText(_path, log);
        }
    }
}