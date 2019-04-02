using System.Collections.Generic;

namespace ExpressionTreeVisitor
{
    public class LinqInfoPropertiesMap
    {
        public List<SelectInfo> SelectInfos { get; set; }
        public List<LinqInfo> LinqInfoStack { get; set; }
        public TakeInfo TakeInfo { get; set; }

        public LinqInfoPropertiesMap()
        {
            SelectInfos = new List<SelectInfo>();
            LinqInfoStack = new List<LinqInfo>();
        }
    }
}