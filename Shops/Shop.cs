using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            _products = new List<ShopProduct>();
        }

        public float Money { get; set; }

        private List<ShopProduct> _products;

        private int Id { get; }

        private string Name { get; }

        private string Address { get; }

        public void AddProducts(List<ShopProduct> products)
        {
            if (products.First() == null)
            {
                foreach (var product in products)
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
            else
            {
                _products.Add(products.First());
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
                if (registeredProducts.Contains(product.Product))
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
                else
                {
                    throw new ProductIsNotRegistered();
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