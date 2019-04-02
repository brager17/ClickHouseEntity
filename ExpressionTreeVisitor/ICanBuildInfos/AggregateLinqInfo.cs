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

        public TakeInfo TakeInfo { get; set; }
    }

    public enum SkipTakeType
    {
        Skip,
        Take
    }

    public class TakeInfo
    {
        public TakeInfo(long? take)
        {
            Take = take;
        }

       

        public long? Take { get; }
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