namespace EFCustom.Connection; 

public interface ICustomConnection {
    ICustomCommand CreateCommand(FormattableString sql);
}