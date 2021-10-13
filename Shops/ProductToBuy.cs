using Shops.Services;

namespace Shops
{
    public class ProductToBuy
    {
        public ProductToBuy(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public Product Product { get; }

        public int Amount { get; }
    }
}