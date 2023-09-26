using System.Text.RegularExpressions;
using EFCustom.Connection;
using Npgsql;

namespace EFCustom.Npgsql; 

public class CustomNpgsqlConnection : ICustomConnection {
    private NpgsqlConnection _connection;

    public CustomNpgsqlConnection(NpgsqlConnection connection) {
        _connection = connection;
    }

    public ICustomCommand CreateCommand(FormattableString sqlCommand) {
        var command = _connection.CreateCommand();
        command.CommandText = ConvertToParameterizedSql(sqlCommand.Format);
        for (int i = 0; i < sqlCommand.ArgumentCount; i++) {
            command.Parameters.AddWithValue($@"@p{i}",sqlCommand.GetArgument(i));
        }

        return new CustomNpgsqlCommand(command);
    }
    
    public static string ConvertToParameterizedSql(string sql) {
        int paramIndex = 0;

        string pattern = @"\{(\d+)\}";

        string result = Regex.Replace(sql, pattern, m => {
            string placeholder = $"@p{paramIndex}";
            paramIndex++;
            return placeholder;
        });
        return result;
    }
}