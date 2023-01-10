using System.ComponentModel.DataAnnotations;
using WebApi.Common;

namespace WebApi.Models.Order
{
    public class CreateOrderModel
    {
        [Required]
        public string ShippingCountry { get; set; } = string.Empty;

        [Required]
        public string ShippingCity { get; set; } = string.Empty;

        [Required]
        public string ShippingAddressLine1 { get; set; } = string.Empty;

        public string? ShippingAddressLine2 { get; set; }

        [Required]
        public string ShippingPostalCode { get; set; } = string.Empty;

        public Enums.OrderStatus Status { get; set; }

        public List<CreateOrderProductModel> Products { get; set; } = new List<CreateOrderProductModel>();
    }
}
