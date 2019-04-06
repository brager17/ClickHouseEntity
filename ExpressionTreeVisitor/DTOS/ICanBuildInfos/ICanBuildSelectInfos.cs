using System.Collections.Generic;

namespace ExpressionTreeVisitor
{
    public interface ICanBuildSelectInfos
    {
        List<SelectInfo> SelectInfo { get; set; }
    }
}