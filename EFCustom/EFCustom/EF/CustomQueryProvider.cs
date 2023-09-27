using System.Linq.Expressions;

namespace EFCustom.EF;

public class CustomQueryProvider : IQueryProvider {
    private ICustomContext _context;

    public CustomQueryProvider(ICustomContext context) {
        _context = context;
    }

    public IQueryable CreateQuery(Expression expression) => CreateQuery<object>(expression);
    public object? Execute(Expression expression) => Execute<object>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) {
        return new CustomQueryable<TElement>(this, expression);
    }

    public TResult Execute<TResult>(Expression expression) {
        var queryBuilder = new QueryBuilder(_context);
        FormattableString sql = queryBuilder.Compile(expression);
        return _context.QueryAsync<TResult>(sql);
    }
}