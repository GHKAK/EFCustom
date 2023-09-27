using System.Reflection;
using EFCustom.Connection;

namespace EFCustom.EF;

public class CustomContext : ICustomContext {
    private ICustomConnection _connection;

    public CustomContext(ICustomConnection connection) {
        _connection = connection;
        foreach (var set in FindSets(GetType())) {
            set.SetValue(this, CreateSet(set.PropertyType.GetGenericArguments()[0]));
        }
    }

    private static PropertyInfo[] FindSets(Type contextType) {
        return contextType.GetRuntimeProperties()
            .Where(p => p.PropertyType.GetGenericTypeDefinition() == typeof(CustomSet<>))
            .ToArray();
    }

    private object? CreateSet(Type pType) {
        return typeof(CustomContext).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(x => x.Name == "CreateSetGeneric")
            .MakeGenericMethod(pType).Invoke(this, Array.Empty<object>());
    }

    private CustomSet<T> CreateSetGeneric<T>() => new CustomSet<T>(new CustomQueryProvider(this));
    
    public string GetTableName(Type setArgumentType) {
        var sets =FindSets(GetType());
        string tableName;
        foreach (var set in sets) {
            if (setArgumentType == set.PropertyType.GetGenericArguments()[0]) {
                return set.Name;
            }
        }
        throw new InvalidOperationException();
    }

    public TResult QueryAsync<TResult>(FormattableString sql) {
        var entityType = typeof(TResult).GetGenericArguments()[0];
        var task = (Task<TResult>) typeof(Mapper).GetMethod("QueryAsync").MakeGenericMethod(entityType)
            .Invoke(null, new object[] { _connection, sql });
        return task.Result;
    }
}