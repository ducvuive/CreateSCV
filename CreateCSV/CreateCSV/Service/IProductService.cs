using CreateCSV.Model;

namespace CreateCSV.Service
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsAsync();
    }
}
