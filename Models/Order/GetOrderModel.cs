using WebApi.Common;

namespace WebApi.Models.Order
{
    public class GetOrderModel
    {
        public int Id { get; set; }
        public string ShippingCountry { get; set; } = string.Empty;

        public string ShippingCity { get; set; } = string.Empty;

        public string ShippingAddressLine1 { get; set; } = string.Empty;

        public string? ShippingAddressLine2 { get; set; }

        public string ShippingPostalCode { get; set; } = string.Empty;

        public Enums.OrderStatus Status { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Products.Sum(p => p.Price) ?? 0;
            }
        }

        public List<GetOrderProductModel> Products { get; set; } = new List<GetOrderProductModel>();
    }
}
