using System.Linq.Expressions;

namespace EFCustom.EF; 

public class WhereVisitor : ExpressionVisitor {
    public string WhereCondition { get; private set; }

    protected override Expression VisitBinary(BinaryExpression node) {
        var condition = node.NodeType switch {
            ExpressionType.Equal => "=",
            ExpressionType.AndAlso => "AND",
            ExpressionType.OrElse => "OR",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.GreaterThan => ">",
        };
        WhereCondition = $"{QueryBuilder.ToString(node.Left)} {condition} {QueryBuilder.ToString(node.Right)}";
        return base.VisitBinary(node);
    }

    
}