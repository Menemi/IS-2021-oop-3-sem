using Shops.Services;

namespace Shops
{
    public class ShopProduct
    {
        public ShopProduct(Product product, int amount, float price)
        {
            Product = product;
            Price = price;
            Amount = amount;
        }

        public Product Product { get; }

        public float Price { get; set; }

        public int Amount { get; set; }

        public new virtual bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            ShopProduct tempProduct = (ShopProduct)obj;
            return Product == tempProduct.Product;
        }
    }
}