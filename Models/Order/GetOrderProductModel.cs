using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Order
{
    public class GetOrderProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal? Price { get; set; }
    }
}
