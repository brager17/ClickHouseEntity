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

    public class WhereInfo : BaseLinqInfo
    {
        public UnaryLinqInfo FirstUnary { get; private set; }

        /// <summary>
        /// / добавление первого элемента цепочки в Where
        /// </summary>
        public WhereInfo(UnaryOperand leftInfo, UnaryOperand rigthInfo, BetweenUnaryOperandOperator @operator)
        {
            FirstUnary = new UnaryLinqInfo {Operator = @operator, LeftOperand = leftInfo, RightOperand = rigthInfo};
        }

        private List<UnaryChainMember> _unaryChainMembers { get; set; }

        public IEnumerable<UnaryChainMember> UnaryChainMembers => _unaryChainMembers;

        /// <summary>
        /// добавление всех элементов цепочки Where кроме первого
        /// </summary>
        public void Add(
            BetweenUnaryOperationOperator operationBetweenPrevAndCurrentUnaryOperandItem,
            UnaryOperand leftInfo, UnaryOperand rigthInfo, BetweenUnaryOperandOperator @operator)
        {
            _unaryChainMembers.Add(new UnaryChainMember
            {
                UnaryLinqInfo = new UnaryLinqInfo
                    {Operator = @operator, LeftOperand = leftInfo, RightOperand = rigthInfo},
                Operator = operationBetweenPrevAndCurrentUnaryOperandItem
            });
        }
    }

    public interface IHasName
    {
        string Name { get; }
    }

    public class ExpressionTypeAttribute : Attribute, IHasName
    {
        public ExpressionTypeAttribute(string binaryExpressionTypeName)
        {
            Name = binaryExpressionTypeName;
        }

        public string Name { get; }
    }

    public class SqlStringAttribute : Attribute, IHasName
    {
        public string Name { get; }

        public SqlStringAttribute(string sqlString)
        {
            Name = sqlString;
        }
    }

    public enum BetweenUnaryOperandOperator
    {
        [SqlString("="), ExpressionType("Equal")]
        Equal,

        [SqlString(">"), ExpressionType("Greater")]
        Greater,

        [SqlString("<"), ExpressionType("Less")]
        Less,

        [SqlString(">="), ExpressionType("GreaterOrEqual")]
        GreaterOrEqual,

        [SqlString("<="), ExpressionType("LessOrEqual")]
        LessOrEqual,

        [SqlString("!="), ExpressionType("NoEqual")]
        NoEqual
    }

    public static class BetweenUnaryOperandOperatorExtensions
    {
        public static BetweenUnaryOperandOperator GetBetweenUnaryOperandOperator(this ExpressionType nodeType)
        {
            var result = typeof(BetweenUnaryOperandOperator).GetMembers().Single(x => x
                .GetCustomAttributes<ExpressionTypeAttribute>(false)
                .Any(xx => xx.Name == nodeType.ToString()));
            return Enum.Parse<BetweenUnaryOperandOperator>(result.Name);
        }

        public static string GetSql(this Attribute attribute)
        {
            var sqlStringAttribute = attribute.GetType().GetCustomAttribute<SqlStringAttribute>();
            if (sqlStringAttribute == null)
                throw new ArgumentException("На экземпляре enum'a нет атрибута " + nameof(SqlStringAttribute));
            return sqlStringAttribute.Name;
        }
    }

    public enum BetweenUnaryOperationOperator
    {
        [SqlString("Or"), ExpressionType("||")]
        Or,

        [SqlString("And"), ExpressionType("&&")]
        And
    }

    public class UnaryChainMember
    {
        public UnaryLinqInfo UnaryLinqInfo { get; set; }
        public BetweenUnaryOperationOperator Operator { get; set; }
    }

    public class UnaryLinqInfo
    {
        public UnaryOperand LeftOperand { get; set; }
        public UnaryOperand RightOperand { get; set; }
        public BetweenUnaryOperandOperator Operator { get; set; }
    }

    public class UnaryOperand : ICallExpression, IMemberExpression, IConstantExpression
    {
        /// <summary>
        /// Этот тип может обработать только MethodCallExpression,MemberExpression и ConstantExpression
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="ArgumentException"></exception>
        public UnaryOperand(Expression expression)
        {
            switch (expression)
            {
                case MethodCallExpression methodCallExpression:
                    _callExpression = methodCallExpression;
                    break;
                case MemberExpression memberExpression:
                    _memberExpression = memberExpression;
                    break;
                case ConstantExpression constantExpression:
                    _contantExpression = constantExpression;
                    break;
                default:
                    throw new ArgumentException("Expression данного типа не допустим");
            }
        }

        public UnaryOperand(MethodCallExpression callExpression)
        {
            _callExpression = callExpression;
        }

        public UnaryOperand(ConstantExpression contantExpression)
        {
            _contantExpression = contantExpression;
        }

        public UnaryOperand(MemberExpression memberExpression)
        {
            _memberExpression = memberExpression;
        }

        public MethodCallExpression _callExpression { get; }
        public MemberExpression _memberExpression { get; }
        public ConstantExpression _contantExpression { get; }
    }
}