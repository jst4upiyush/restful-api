namespace ProductCatalog.Api.Infrastructure.Data
{
    // This can easily be modified to be IEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity
    public interface IEntity
    {
        int Id { get; }
    }
}
