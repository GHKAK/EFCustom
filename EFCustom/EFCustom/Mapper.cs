using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using EFCustom.Connection;

namespace EFCustom; 

static class Mapper {
    private static MethodInfo getValueMethodInfo = typeof(DataReaderExtensions).GetMethod("GetValue");
    public static async Task<IEnumerable<T>> QueryAsync<T>(this ICustomConnection connection, FormattableString sqlCommand) {
        await using var command = connection.CreateCommand(sqlCommand); 
        using var reader = await command.ExecuteReaderAsync();
        var list = new List<T>();
        Func<IDataReader, T> createItem = InitializerFunc<T>();
        while (reader.Read()) {
            list.Add(createItem(reader));
        }
        return list;
    }

    private static Func<IDataReader, T> InitializerFunc<T>() {
        var readerParam = Expression.Parameter(typeof(IDataReader));
        var body = Expression.MemberInit(Expression.New(typeof(T)),
            typeof(T).GetProperties().Select(x => Expression.Bind(x, BuildGetExpression(readerParam, x))));
        return Expression.Lambda<Func<IDataReader, T>>(body, readerParam).Compile();
    }

    private static Expression BuildGetExpression(Expression reader, PropertyInfo prop) {
        var propertyType = prop.PropertyType;

        return Expression.Call(null, getValueMethodInfo.MakeGenericMethod(propertyType), reader,
            Expression.Constant(prop.Name));
    }
    
}