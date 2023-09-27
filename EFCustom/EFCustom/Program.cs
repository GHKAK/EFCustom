using EFCustom;
using EFCustom.EF;
using EFCustom.Npgsql;
using Npgsql;

var connection = new NpgsqlConnection("Host=localhost;Database=cities;User Id=postgres;Password=postgres");
await connection.OpenAsync();
var npgConnection = new CustomNpgsqlConnection(connection);
var context = new GeoContext(npgConnection);
var result = context.Cities.Where(x => x.Id >-1 ).Select(x => new City(){Name = x.Name}).Take(2);

foreach (var item in  result) {
    Console.WriteLine(item.Name);
}