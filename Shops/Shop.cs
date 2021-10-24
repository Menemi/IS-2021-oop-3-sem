using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                var productInShop = _products.FirstOrDefault(p => product == p);
                if (productInShop == null)
                {
                    _products.Add(product);
                    continue;
                }

                productInShop.Amount += product.Amount;
            }
        }

        public ReadOnlyCollection<ShopProduct> GetProducts()
        {
            return _products.AsReadOnly();
        }

        public void Buy(List<ProductToBuy> products, Customer customer)
        {
            foreach (var product in products)
            {
                foreach (var shopProduct in _products
                    .Where(shopProduct => product.Product.Id == shopProduct.Product.Id))
                {
                    if (shopProduct.Amount < product.Amount)
                    {
                        throw new NotEnoughProductsAmount();
                    }

                    var enoughMoneyToBuy = product.Amount * shopProduct.GetPrice();

                    if (customer.Money < enoughMoneyToBuy)
                    {
                        throw new NotEnoughMoneyException();
                    }

                    shopProduct.Amount -= product.Amount;
                    customer.Money -= enoughMoneyToBuy;
                    Money += product.Amount * shopProduct.GetPrice();
                    break;
                }
            }
        }

        public void ChangePrice(string name, float price)
        {
            foreach (var product in _products.Where(product => product.Product.Name == name))
            {
                product.SetPrice(price);
                return;
            }

            throw new ProductIsNotRegistered();
        }
    }
}