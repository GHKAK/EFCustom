using System.Data;

namespace EFCustom.Connection; 

public interface ICustomCommand : IAsyncDisposable {
    Task<IDataReader> ExecuteReaderAsync();
}