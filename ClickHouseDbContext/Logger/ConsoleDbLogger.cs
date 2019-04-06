using System;

namespace Context
{
    public class ConsoleDbLogger : IDbLogger
    {
        public void WriteLog(LogLevel logLevel, string log)
        {
            Console.WriteLine(log);
        }
    }

    public class StubConsoleLogger : IDbLogger
    {
        public void WriteLog(LogLevel logLevel, string log)
        {
        }
    }
}