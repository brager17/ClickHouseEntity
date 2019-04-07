using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeVisitor
{
    public class SelectInfo : BaseLinqInfo
    {
        public Dictionary<PropertyInfo, InPropertySelectExpression> _propertyExpressions { get; }

        public SelectInfo()
        {
            _propertyExpressions = new Dictionary<PropertyInfo, InPropertySelectExpression>();
        }
    }

    public class WhereInfo
    {
        public WhereStr _WhereInfo { get; set; }
    }
}