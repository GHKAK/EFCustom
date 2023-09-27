namespace EFCustom.EF;

public interface ICustomContext {
    string GetTableName(Type setArgumentType);
    TResult QueryAsync<TResult>(FormattableString sql);
}