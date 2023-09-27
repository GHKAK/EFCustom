namespace EFCustom.EF; 

public class CustomSet<T> : CustomQueryable<T> {
    public CustomSet(IQueryProvider provider) : base(provider) {
    }
}