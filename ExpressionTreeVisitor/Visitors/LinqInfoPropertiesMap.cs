using System.Collections.Generic;

namespace ExpressionTreeVisitor
{
    public class LinqInfoPropertiesMap
    {
        public List<SelectInfo> SelectInfos { get; private set; }
        public List<LinqInfo> LinqInfoStack { get; private set; }
        public TakeInfo TakeInfo { get; set; }

        public LinqInfoPropertiesMap()
        {
            SelectInfos = new List<SelectInfo>();
            LinqInfoStack = new List<LinqInfo>();
        }
    }
}