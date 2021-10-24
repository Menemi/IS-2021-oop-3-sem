using Shops.Services;

namespace Shops
{
    public class ShopProduct
    {
        private float _price;

        public ShopProduct(Product product, int amount, float price)
        {
            Product = product;
            _price = price;
            Amount = amount;
        }

        public Product Product { get; }

        public int Amount { get; set; }

        public float GetPrice()
        {
            return _price;
        }

        public void SetPrice(float price)
        {
            _price = price;
        }

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