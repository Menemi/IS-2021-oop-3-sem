using System;
using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Services
{
    public class Shop
    {
        private static int _idCounter = 1;

        public Shop(string name, string address)
        {
            Id = _idCounter++;
            Name = name;
            Address = address;
            Money = 0;
            Products = new List<Product>();
        }

        public float Money { get; set; }

        public List<Product> Products { get; }

        private int Id { get; }

        private string Name { get; }

        private string Address { get; }

        public void Delivery(List<Product> products, List<string> registeredProducts)
        {
            foreach (var product in products)
            {
                if (registeredProducts.Contains(product.Name))
                {
                    if (!Products.Contains(product))
                    {
                        Products.Add(product);
                    }
                    else
                    {
                        foreach (var shopProduct in Products)
                        {
                            if (product.Name == shopProduct.Name)
                            {
                                shopProduct.Amount += product.Amount;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    throw new ProductIsNotRegistered();
                }
            }
        }

        public void Buy(Dictionary<string, int> products, List<string> registeredProducts, Customer customer)
        {
            foreach (var product in products)
            {
                if (registeredProducts.Contains(product.Key))
                {
                    foreach (var shopProduct in Products)
                    {
                        if (product.Key == shopProduct.Name)
                        {
                            shopProduct.Amount -= product.Value;
                            customer.Money -= product.Value * shopProduct.Price;
                            Money += product.Value * shopProduct.Price;
                            break;
                        }
                    }
                }
                else
                {
                    throw new ProductIsNotRegistered();
                }
            }
        }

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

            throw new ProductIsNotRegistered();
        }
    }
}