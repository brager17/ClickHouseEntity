using System.Collections.Generic;

namespace ExpressionTreeVisitor
{
    public interface ICanBuildWhereInfo
    {
        List<WhereInfo> WhereInfo { get; set; }
    }
}