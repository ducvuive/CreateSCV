using System.ComponentModel.DataAnnotations;

namespace CreateCSV.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string NameProduct { get; set; }
    }
}
