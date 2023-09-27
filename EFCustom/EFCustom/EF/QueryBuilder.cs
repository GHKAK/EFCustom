using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EFCustom.EF;

public class QueryBuilder : ExpressionVisitor {
    private ICustomContext _context;

    private Expression selectExpression,
        whereExpression;

    private string _whereCondition, _selectRoll, _limitCount, _offsetCount;

    public QueryBuilder(ICustomContext context) {
        _context = context;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node) {
        var method = node.Method.GetGenericMethodDefinition();
        if (method == QueryableMethods.Where) {
            VisitWhere(node);
        } else if (method == QueryableMethods.Select) {
            VisitSelect(node);
        } else if (method == QueryableMethods.Take) {
            VisitTake(node);
        } else if (method == QueryableMethods.Skip) {
            VisitSkip(node);
        } else {
            throw new NotImplementedException($"Method {node.Method.Name} is not supported.");
        }

        return base.VisitMethodCall(node);
    }

    private void VisitSelect(MethodCallExpression node) {
        selectExpression = node.Arguments[1];
    }

    private void VisitWhere(MethodCallExpression node) {
        whereExpression = node.Arguments[1];
    }

    private void VisitTake(MethodCallExpression node) {
        _limitCount = node.Arguments[1].ToString();
    }

    private void VisitSkip(MethodCallExpression node) {
        _offsetCount = node.Arguments[1].ToString();
    }

    public static string ToString(Expression expression) {
        if (expression is MemberExpression p) {
            return $"\"{p.Member.Name}\"";
        }

        return ((ConstantExpression)expression).Value.ToString();
    }

    public FormattableString Compile(Expression expression) {
        Visit(expression);
        var whereVisitor = new WhereVisitor();
        whereVisitor.Visit(whereExpression);
        _whereCondition = whereVisitor.WhereCondition;
        var selectVisitor = new SelectVisitor();
        selectVisitor.Visit(((UnaryExpression)selectExpression).Operand);
        _selectRoll = selectVisitor.SelectRoll;
        var operandExpression = ((UnaryExpression)selectExpression).Operand;
        var lambdaExpression = (LambdaExpression)operandExpression;
        var entityType = lambdaExpression.ReturnType; 
        var sql =
            $"""
             SELECT {_selectRoll}
             FROM {$"\"{_context.GetTableName(entityType)}\""}
             WHERE {_whereCondition}
             {(string.IsNullOrEmpty(_limitCount) ? "" : $"LIMIT {_limitCount}")}
             {(string.IsNullOrEmpty(_offsetCount) ? "" : $"OFFSET {_offsetCount}")}
             """;
        return FormattableStringFactory.Create(sql);
    }
}