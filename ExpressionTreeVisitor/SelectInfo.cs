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

        /// <summary>
        /// example: Select(x=>x.Id) - simple select
        /// </summary>
        public bool IsPrimitiveSelect
        {
            get
            {
                var fromTypeName = LambdaType.Name;
                if (_propertyExpressions.Count != 1)
                    return false;
                var singleTypeName = _propertyExpressions.Single().Key.DeclaringType.Name;
               
                return fromTypeName == singleTypeName;
            }
        }
    }

    public class WhereInfo : BaseLinqInfo, ICallExpression
    {
        public MethodCallExpression _callExpression { get; set; }
    }
}