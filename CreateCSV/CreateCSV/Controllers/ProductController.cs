using CreateCSV.Service;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CreateCSV.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        // Use dependency injection to get an instance of the service class 
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // Define a GET action method that returns a FileResult with the CSV content 
        [HttpGet]
        [Route("export")]
        public async Task<IActionResult> Export()
        {
            //var products = await _productService.GetAllProductsAsync();

            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };
            /*give me a list example about database product*/
            //List<Product> products = new List<Product>();
            var products = await _productService.GetAllProductsAsync();
            //products.Add(new Product { Collection_ID = "1", Collection_Name = "Product 1" });
            //products.Add(new Product { Collection_ID = "2", Collection_Name = "Product 2" });
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Collection_ID,Collection_Name");
            foreach (var prod in products)
            {
                stringBuilder.AppendLine($"{prod.ProductId},{prod.NameProduct}");
            }
            return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", "products.csv");
        }
    }
}