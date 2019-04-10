using System;
using System.Linq.Expressions;

namespace EntityTracking
{
    public class DeleteInfo<T>
    {
        public Expression<Func<T, bool>> FilterExpression { get; set; }
        public Expression DbSetInitialExpression { get; set; }
    }
}