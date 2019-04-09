using System;
using ExpressionTreeVisitor;

namespace ClickHouseTableGenerator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IndexGranularity : Attribute, IHasKey<int>
    {
        private int _key = 8172;

        public int Key
        {
            get => _key;
            set => _key = value;
        }

        public IndexGranularity(int key)
        {
            _key = key;
        }

        public IndexGranularity()
        {
            
        }

        public object ObjectKey
        {
            get => _key;
            set => _key = (int) value;
        }
    }
}