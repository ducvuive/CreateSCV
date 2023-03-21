using CreateCSV.Model;
using Microsoft.EntityFrameworkCore;

namespace CreateCSV.Service
{
    public class ProductService : IProductService
    {
        //private readonly string _connectionString;

        /*        public ProductService(string connectionString)
                {
                    _connectionString = connectionString;
                }*/
        private readonly ProductContext _context;
        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            /*using (IDbConnection db = new SqlConnection(_connectionString))
            {
                //return await db.QueryAsync<Product>("GetAllProducts", commandType: CommandType.StoredProcedure);
            }*/
            var sql = "SELECT ProductId,NameProduct FROM Product";


            var products = await _context.Products.FromSqlRaw(sql).ToListAsync();
            return products;
        }
    }
}
