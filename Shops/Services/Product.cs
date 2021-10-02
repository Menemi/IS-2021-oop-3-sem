namespace Shops.Services
{
    public class Product
    {
        public int Amount { get; set; }

        public float Price { get; set; }

        public string Name { get; }

        public Product(int amount, float price, string name)
        {
            Amount = amount;
            Price = price;
            Name = name;
        }
    }
}