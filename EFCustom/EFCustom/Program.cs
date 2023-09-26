using System.Collections.Concurrent;
using EFCustom.Npgsql;
using Npgsql;

var connection = new NpgsqlConnection("Host=localhost;Database=cities;User Id=postgres;Password=postgres");
await connection.OpenAsync();
var npgConnection = new CustomNpgsqlConnection(connection);
const int id=-1;
FormattableString sql = $"""
                   SELECT "Id","Name" FROM "Cities" WHERE "Id" > {id};
                   """;
foreach (var item in await npgConnection.QueryAsync<City>(sql)) {
    Console.WriteLine(item.Id);
}