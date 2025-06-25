public interface IProductRepository
{
    Product? GetById(Guid id);
    IEnumerable<Product> GetAll();
}