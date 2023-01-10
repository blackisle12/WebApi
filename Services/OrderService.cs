using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WebApi.Models.Order;
using WebApi.Repository;
using WebApi.Repository.Entity;
using WebApi.Resources;
using WebApi.Services.Interface;
using static WebApi.Common.Enums;

namespace WebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext context;

        public OrderService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CreateAsync(CreateOrderModel model)
        {
            if (model.Products == null || !model.Products.Any())
            {
                throw new ArgumentException("AtLeastOneProductIsRequired");
            }

            var productIds = model.Products.Select(p => p.Id);

            if (context.Products.Count(p => productIds.Contains(p.Id)) != productIds.Count())
            {
                throw new ArgumentException("OneOrMoreProductsIsInvalid");
            }

            var totalPrice = context.Products.Where(p => productIds.Contains(p.Id)).Sum(p => p.Price);

            if (totalPrice > 1000000)
            {
                throw new ArgumentException("OrderTotalPriceShouldNotExceed");
            }

            var order = new Order
            {
                ShippingCountry = model.ShippingCountry,
                ShippingCity = model.ShippingCity,
                ShippingPostalCode = model.ShippingPostalCode,
                ShippingAddressLine1 = model.ShippingAddressLine1,
                ShippingAddressLine2 = model.ShippingAddressLine2,
                Status = model.Status,
                Products = model.Products
                    .Select(p => new ProductOrder
                    {
                        ProductId = p.Id
                    })
                    .ToList()
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            return order.Id;
        }

        public async Task EditAsync(EditOrderModel model)
        {
            if (model.Products == null || !model.Products.Any())
            {
                throw new ArgumentException("AtLeastOneProductIsRequired");
            }

            var productIds = model.Products.Select(p => p.Id);

            if (context.Products.Count(p => productIds.Contains(p.Id)) != productIds.Count())
            {
                throw new ArgumentException("OneOrMoreProductsIsInvalid");
            }

            var existingOrder = await context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == model.Id);

            if (existingOrder == null)
            {
                throw new ArgumentException("OrderIdIsInvalid");
            }

            if (existingOrder.Status == Common.Enums.OrderStatus.Closed)
            {
                throw new ArgumentException("ClosedOrderCannotBeModified");
            }

            existingOrder.ShippingCountry = model.ShippingCountry;
            existingOrder.ShippingCity = model.ShippingCity;
            existingOrder.ShippingPostalCode = model.ShippingPostalCode;
            existingOrder.ShippingAddressLine1 = model.ShippingAddressLine1;
            existingOrder.ShippingAddressLine2 = model.ShippingAddressLine2;
            existingOrder.Status = model.Status;

            var existingProductIds = existingOrder.Products.Select(p => p.ProductId);
            var newProducts = model.Products
                .Where(p => !existingProductIds.Contains(p.Id))
                .Select(p => new ProductOrder
                {
                    OrderId = existingOrder.Id,
                    ProductId = p.Id
                })
                .ToList();

            existingOrder.Products.AddRange(newProducts);

            await context.SaveChangesAsync();
        }

        public async Task<List<GetOrderModel>> GetAllAsync(int? id = null, OrderStatus? status = null)
        {
            var orders = context.Orders.AsQueryable();

            if (id != null)
            {
                orders = orders.Where(o => o.Id == id);
            }

            if (status != null)
            {
                orders = orders.Where(o => o.Status == status);
            }

            var result = await orders
                .Select(o => new GetOrderModel
                {
                    Id = o.Id,
                    ShippingCountry = o.ShippingCountry,
                    ShippingCity = o.ShippingCity,
                    ShippingPostalCode = o.ShippingPostalCode,
                    ShippingAddressLine1 = o.ShippingAddressLine1,
                    ShippingAddressLine2 = o.ShippingAddressLine2,
                    Products = o.Products
                        .Select(p => new GetOrderProductModel
                        {
                            Id = p.ProductId,
                            Name = p.Product.Name,
                            Price = p.Product.Price
                        })
                        .ToList()
                })
                .ToListAsync();

            return result;
        }
    }
}
