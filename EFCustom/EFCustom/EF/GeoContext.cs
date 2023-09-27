using EFCustom.Connection;

namespace EFCustom.EF;

public class GeoContext : CustomContext {
    public GeoContext(ICustomConnection connection) : base(connection) {
    }

    public CustomSet<City> Cities { get; set; }
}