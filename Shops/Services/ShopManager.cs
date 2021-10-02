using System;
using System.Collections.Generic;

namespace Shops.Services
{
    public class ShopManager
    {
        public List<string> Products;
        public List<Shop> Shops;

        public void CreateShop(string name, string address, int id)
        {
            Shops.Add(new Shop(id, name, address));
        }

        public void RegisterProduct(string name)
        {
            Products.Add(name);
        }

        // TODO exception
        public bool AvailabilityOfProducts(List<Product> products, Shop shop)
        {
            foreach (var product in products)
            {
                bool availability = false;
                foreach (var shopProduct in shop.Products)
                {
                    availability = product.Name == shopProduct.Name && product.Amount >= shopProduct.Amount;
                }

                if (!availability)
                {
                    throw new Exception();
                }
            }

            return true;
        }

        public float CostOfProductsInTheShop(List<Product> products, Shop shop)
        {
            float resultPrice = 0;
            foreach (var product in products)
            {
                foreach (var shopProduct in shop.Products)
                {
                    if (product.Name == shopProduct.Name)
                    {
                        resultPrice += shopProduct.Price;
                        break;
                    }
                }
            }

            return resultPrice;
        }

        public Shop TheBestShopSearching(List<Product> products)
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