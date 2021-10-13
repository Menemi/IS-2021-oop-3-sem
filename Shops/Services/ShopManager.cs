using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager
    {
        public ShopManager()
        {
            RegisteredProducts = new List<Product>();
            Shops = new List<Shop>();
        }

        public List<Product> RegisteredProducts { get; }

        private List<Shop> Shops { get; }

        public Shop CreateShop(string name, string address)
        {
            Shop shop = new Shop(name, address);
            Shops.Add(shop);
            return shop;
        }

        public void RegisterProduct(string name)
        {
            RegisteredProducts.Add(new Product(name));
        }

        public void RegistrationCheck(List<ShopProduct> products)
        {
            if (products.Any(product => !RegisteredProducts.Contains(product.Product)))
            {
                throw new ProductIsNotRegistered();
            }
        }

        public void AddProducts(List<ShopProduct> products, Shop shop)
        {
            RegistrationCheck(products);
            shop.AddProducts(products);
        }

        // do not use without checking the registration
        public float CostOfProductsInTheShop(List<ProductToBuy> products, Shop shop)
        {
            float resultPrice = 0;
            foreach (var product in products)
            {
                foreach (var shopProduct in shop.GetProducts())
                {
                    if (product.Product.Id == shopProduct.Product.Id)
                    {
                        resultPrice += product.Amount * shopProduct.Price;
                        break;
                    }
                }
            }

            return resultPrice;
        }

        public Shop TheBestShopSearching(List<ProductToBuy> products)
        {
            float minExpenses = int.MaxValue;

            Shop theBestShop = null;
            foreach (var shop in Shops)
            {
                var price = CostOfProductsInTheShop(products, shop);
                if (minExpenses > price)
                {
                    minExpenses = price;
                    theBestShop = shop;
                }
            }

            return theBestShop;
        }
    }
}