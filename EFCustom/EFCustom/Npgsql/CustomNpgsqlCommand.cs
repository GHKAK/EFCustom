using System.Data;
using EFCustom.Connection;
using Npgsql;

namespace EFCustom.Npgsql; 

public class CustomNpgsqlCommand : ICustomCommand, IAsyncDisposable {
    private NpgsqlCommand _command;

    public CustomNpgsqlCommand(NpgsqlCommand command) {
        _command = command;
    }
    public async Task<IDataReader> ExecuteReaderAsync() {
        return await _command.ExecuteReaderAsync(CancellationToken.None);
    }

    public async ValueTask DisposeAsync() {
        await _command.DisposeAsync();
    }
}