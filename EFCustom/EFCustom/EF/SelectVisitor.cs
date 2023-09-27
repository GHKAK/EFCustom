using System.Linq.Expressions;

namespace EFCustom.EF;

public class SelectVisitor : ExpressionVisitor {
    public string SelectRoll { get; private set; }

    protected override Expression VisitMemberInit(MemberInitExpression node) {
        var nodes = node.Bindings.Cast<MemberAssignment>()
            .Select(x => QueryBuilder.ToString(x.Expression));
        SelectRoll = string.Join(", ", nodes);
        return base.VisitMemberInit(node);
    }
}