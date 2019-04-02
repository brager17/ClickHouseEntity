namespace Context
{
    public interface IDbLogger
    {
        void WriteLog(LogLevel logLevel,string log);
    }
    
}