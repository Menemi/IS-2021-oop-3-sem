using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Products = new List<ShopProduct>();
        }

        public float Money { get; set; }

        private List<ShopProduct> Products { get; }

        private int Id { get; }

        private string Name { get; }

        private string Address { get; }

        public void AddProducts(List<ShopProduct> products)
        {
            foreach (var product in products)
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
        }

        public ReadOnlyCollection<ShopProduct> GetProducts()
        {
            return Products.AsReadOnly();
        }

        public void Buy(Dictionary<string, int> products, Customer customer)
        {
            foreach (var product in products)
            {
                foreach (var shopProduct in Products)
                {
                    if (product.Key == shopProduct.Name && shopProduct.Amount >= product.Value)
                    {
                        shopProduct.Amount -= product.Value;
                        customer.Money -= product.Value * shopProduct.Price;
                        Money += product.Value * shopProduct.Price;
                        break;
                    }
                }
            }
        }

        public void ChangePrice(string name, float price)
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