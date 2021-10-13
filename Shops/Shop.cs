using System.Collections.Generic;
using System.Collections.ObjectModel;
using Shops.Tools;

namespace Shops.Services
{
    public class Shop
    {
        private static int _idCounter = 1;

        private List<ShopProduct> _products;

        public Shop(string name, string address)
        {
            Id = _idCounter++;
            Name = name;
            Address = address;
            Money = 0;
            _products = new List<ShopProduct>();
        }

        public float Money { get; set; }

        private int Id { get; }

        private string Name { get; }

        private string Address { get; }

        public void AddProducts(List<ShopProduct> products)
        {
            foreach (var product in products)
            {
                if (!_products.Contains(product))
                {
                    _products.Add(product);
                }
                else
                {
                    foreach (var shopProduct in _products)
                    {
                        if (product.Product.Name == shopProduct.Product.Name)
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
            return _products.AsReadOnly();
        }

        public void Buy(List<ProductToBuy> products, Customer customer, List<Product> registeredProducts)
        {
            foreach (var product in products)
            {
                foreach (var shopProduct in _products)
                {
                    if (product.Product.Id == shopProduct.Product.Id && shopProduct.Amount >= product.Amount)
                    {
                        shopProduct.Amount -= product.Amount;
                        customer.Money -= product.Amount * shopProduct.Price;
                        Money += product.Amount * shopProduct.Price;
                        break;
                    }
                }
            }
        }

        public void ChangePrice(string name, float price)
        {
            foreach (var product in _products)
            {
                if (product.Product.Name == name)
                {
                    product.Price = price;
                    return;
                }
            }

            throw new ProductIsNotRegistered();
        }
    }
}