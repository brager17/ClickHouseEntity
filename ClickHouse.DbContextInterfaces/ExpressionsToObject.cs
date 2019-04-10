using System.Linq.Expressions;

namespace DbContext
{
    public interface IExpressionsToObject
    {
        TResult Handle<TResult>(Expression expression);
    }
}