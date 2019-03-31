using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using DbContext;

namespace ExpressionTreeVisitor
{
    public abstract class ForSqlRequestInfo : ICanBuildWhereInfo, ICanBuildSelectInfos
    {
        public Type SetType { get; set; }
        public List<SelectInfo> SelectInfo { get; set; }
        public List<WhereInfo> WhereInfo { get; set; }
    }

  
    public class AggregateLinqInfo : ForSqlRequestInfo
    {
        public AggregateLinqInfo()
        {
            SelectInfo = new List<SelectInfo>();
            WhereInfo = new List<WhereInfo>();
        }
    }

    
}