using System;
using System.Collections.Generic;

namespace Shops.Services
{
    public class Shop
    {
        public int Id { get; }

        public string Name { get; }

        private string Address { get; }

        public float Money = 0;
        public List<Product> Products;

        // TODO auto id creater
        public Shop(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }

        // shouldDo == true == Buy
        // TODO exception
        public float BuyOrDelivery(List<Product> products, bool shouldDo)
        {
            float costOfProducts = 0;
            foreach (var product in products)
            {
                bool registered = false;
                foreach (var shopProduct in Products)
                {
                    if (product.Name == shopProduct.Name)
                    {
                        registered = true;
                        if (shouldDo)
                        {
                            shopProduct.Amount -= product.Amount;
                            costOfProducts += product.Amount * shopProduct.Price;
                        }
                        else
                        {
                            shopProduct.Amount += product.Amount;
                        }
                    }
                }

                if (!registered)
                {
                    throw new Exception();
                }
            }

            return costOfProducts;
        }

        public void Delivery(List<Product> products)
        {
            BuyOrDelivery(products, false);
        }

        public void Buy(List<Product> products, Customer customer)
        {
            float costOfProducts = BuyOrDelivery(products, true);
            customer.Money -= costOfProducts;
            Money += costOfProducts;
        }

        // TODO exception
        public void PriceChanging(string name, float price)
        {
            foreach (var product in Products)
            {
                if (product.Name == name)
                {
                    product.Price = price;
                    return;
                }
            }

            throw new Exception();
        }
    }
}