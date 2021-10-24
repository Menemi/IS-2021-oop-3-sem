using System;
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

        public override int GetHashCode()
        {
            return HashCode.Combine(_price, Product, Amount);
        }

        protected bool Equals(ShopProduct other)
        {
            return _price.Equals(other._price) && Equals(Product, other.Product) && Amount == other.Amount;
        }
    }
}