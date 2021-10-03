using System;
using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager
    {
        public ShopManager()
        {
            ProductsName = new List<string>();
            Shops = new List<Shop>();
        }

        public List<string> ProductsName { get; }

        public List<Shop> Shops { get; }

        public Shop CreateShop(string name, string address)
        {
            Shop shop = new Shop(name, address);
            Shops.Add(shop);
            return shop;
        }

        public void RegisterProduct(string name)
        {
            ProductsName.Add(name);
        }

        public bool AvailabilityOfProducts(Dictionary<string, int> products, Shop shop)
        {
            foreach (var product in products)
            {
                if (ProductsName.Contains(product.Key))
                {
                    foreach (var shopProduct in shop.Products)
                    {
                        if (shopProduct.Name == product.Key && product.Value > shopProduct.Amount)
                        {
                            throw new NotEnoughProductsAmount();
                        }
                    }
                }
                else
                {
                    throw new ProductIsNotRegistered();
                }
            }

            return true;
        }

        public float CostOfProductsInTheShop(Dictionary<string, int> products, Shop shop)
        {
            float resultPrice = 0;
            foreach (var product in products)
            {
                foreach (var shopProduct in shop.Products)
                {
                    if (product.Key == shopProduct.Name)
                    {
                        resultPrice += product.Value * shopProduct.Price;
                        break;
                    }
                }
            }

            return resultPrice;
        }

        public Shop TheBestShopSearching(Dictionary<string, int> products)
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