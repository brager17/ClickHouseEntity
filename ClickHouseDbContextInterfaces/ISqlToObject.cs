using System;
using System.Linq.Expressions;

namespace DbContext
{
    public interface ISqlToObject
    {
        TResult Handle<TResult>(Expression expression);
    }
}