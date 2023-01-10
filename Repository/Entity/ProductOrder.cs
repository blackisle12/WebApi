namespace WebApi.Repository.Entity
{
    public class ProductOrder : EntityBase
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
