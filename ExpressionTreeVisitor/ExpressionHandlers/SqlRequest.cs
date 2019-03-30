using System.Collections.Generic;

namespace DbContext
{
    public class SqlRequest
    {
        public string TableName { get; set; }
        public string Select { get; set; }
        public string Where { get; set; }
        public string OrderBy { get; set; }
    }
}