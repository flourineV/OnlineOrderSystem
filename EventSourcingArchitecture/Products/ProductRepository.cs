public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products = [
        new Product { Id = Guid.NewGuid(), Name = "Algebra 1" },
        new Product { Id = Guid.NewGuid(), Name = "Calculus 1" },
        new Product { Id = Guid.NewGuid(), Name = "C++ from zero to hero book (not scam)" },
        new Product { Id = Guid.NewGuid(), Name = "C# for beginner (not scam)" },
        new Product { Id = Guid.NewGuid(), Name = "Learn java at home" },
    ];

    public Product? GetById(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return product;
    }

    public IEnumerable<Product> GetAll()
    {
        return _products.AsReadOnly();
    }
}