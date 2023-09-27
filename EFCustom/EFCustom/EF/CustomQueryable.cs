using System.Collections;
using System.Linq.Expressions;

namespace EFCustom.EF; 

public class CustomQueryable<T> : IQueryable<T> {
    public Type ElementType { get; }
    public Expression Expression { get; }
    public IQueryProvider Provider { get; }

    public CustomQueryable(IQueryProvider provider) {
        ElementType = typeof(T);
        Expression = Expression.Constant(this);
        Provider = provider;
    }
    public CustomQueryable(IQueryProvider provider,Expression expression) {
        ElementType = typeof(T);
        Expression = expression;
        Provider = provider;
    }
    public IEnumerator<T> GetEnumerator() {
        return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

}