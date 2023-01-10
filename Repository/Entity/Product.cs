namespace WebApi.Repository.Entity
{
    public class Product : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
