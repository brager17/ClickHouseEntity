using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public List<OrderInfo> OrderInfo { get; set; }
    }

    public class TakeInfo
    {
        public TakeInfo(long? take)
        {
            Take = take;
        }

        public long? Take { get; }
    }

    public enum OrderType
    {
        [Name("ASC")]
        OrderBy,
        [Name("DESC")]
        OrderByDescending,
        [InfoAttribute("NotImpl")] ThenBy,
        [InfoAttribute("NotImpl")] ThenByDescending,
    }

    public class OrderInfo
    {
        public OrderType OrderType { get; set; }

        [Info("orderBy(x=>{x.Id,x.Name}) // Id,Name")]
        public string OrderString { get; set; }
    }

    public class AggregateLinqInfo : ForSqlRequestInfo
    {
        public AggregateLinqInfo()
        {
            SelectInfo = new List<SelectInfo>();
            WhereInfo = new List<WhereInfo>();
            OrderInfo = new List<OrderInfo>();
        }
    }
}