using System.Reflection;

namespace DbContext
{
    public class NameValue
    {
        public object Value { get; set; }
    }

    public class PropertiesNameValues
    {
        public PropertyInfo[] Properties { get; set; }
        public object[] Values { get; set; }
    }
}