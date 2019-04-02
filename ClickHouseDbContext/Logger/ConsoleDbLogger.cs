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

    internal class StubConsoleLogger : IDbLogger
    {
        public void WriteLog(LogLevel logLevel, string log)
        {
        }
    }
}