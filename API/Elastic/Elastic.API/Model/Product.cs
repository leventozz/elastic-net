namespace Elastic.API.Model
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ProductFeature Feature { get; set; }
    }
}
