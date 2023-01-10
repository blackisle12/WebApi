using WebApi.Common;

namespace WebApi.Repository.Entity
{
    public class Order : EntityBase
    {
        public string ShippingCountry { get; set; } = string.Empty;
        public string ShippingCity { get; set; } = string.Empty;
        public string ShippingAddressLine1 { get; set; } = string.Empty;
        public string? ShippingAddressLine2 { get; set; }
        public string ShippingPostalCode { get; set; } = string.Empty;
        public Enums.OrderStatus Status { get; set; }
        public List<ProductOrder> Products { get; set; } = new List<ProductOrder>();
    }
}
