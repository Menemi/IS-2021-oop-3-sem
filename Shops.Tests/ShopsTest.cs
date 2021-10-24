using System.Collections.Generic;
using System.Linq;
using Shops.Services;
using NUnit.Framework;

namespace Shops.Tests
{
    public class Tests
    {
        private ShopManager _shopManager;
        private Shop _shop;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
            _shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");
        }

        [Test]
        public void DeliveryOfProductsToTheShop()
        {
            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Meat");
            var eggs = _shopManager.RegisterProduct("Eggs");

            var products = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            _shop.AddProducts(products);

            CollectionAssert.AreEquivalent(products, _shop.GetProducts());
        }

        [Test]
        public void ChangingPriceForSomeProducts()
        {
            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Meat");
            var eggs = _shopManager.RegisterProduct("Eggs");

            var products = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            _shop.AddProducts(products);

            _shop.ChangePrice("Meat", 4);
            Assert.AreEqual(_shop.GetProducts()[1].GetPrice(), 4);
        }

        [Test]
        public void TheBestShopSearching()
        {
            var shop2 = _shopManager.CreateShop("Pyaterochka", "Udarnikov Street 14");

            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Meat");
            var eggs = _shopManager.RegisterProduct("Eggs");

            var products1 = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            _shop.AddProducts(products1);

            var products2 = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 4),
                new ShopProduct(meat, 13, 34),
                new ShopProduct(eggs, 60, 5)
            };
            shop2.AddProducts(products2);

            var products = new List<ProductToBuy>
            {
                new ProductToBuy(milk, 2),
                new ProductToBuy(meat, 13),
                new ProductToBuy(eggs, 60)
            };

            Assert.AreEqual(_shopManager.TheBestShopSearching(products), _shop);
        }

        [Test]
        public void BuyingOfProductsInTheShop()
        {
            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Meat");
            var eggs = _shopManager.RegisterProduct("Eggs");

            var productsToDelivery = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            _shop.AddProducts(productsToDelivery);

            var productsToBuy = new List<ProductToBuy>
            {
                new ProductToBuy(milk, 2),
                new ProductToBuy(meat, 12),
                new ProductToBuy(eggs, 59)
            };

            var shopsMoneyBeforePurchase = _shop.Money;
            var customer = new Customer(1000000, "Taylor Swift");
            var shopsCountOfProductsBeforePurchase = _shop.GetProducts().Sum(product => product.Amount);


            foreach (var product in productsToBuy)
            {
                // без foreach тут не обойтись, я просмотрел все методы CollectionAssert
                // и там нет нужного,который бы это делал без заданного мной цикла
                CollectionAssert.Contains(_shopManager.RegisteredProducts, product.Product);

                foreach (var shopProduct in _shop.GetProducts()
                    .Where(shopProduct => shopProduct.Product.Id == product.Product.Id))
                {
                    Assert.True(product.Amount <= shopProduct.Amount);
                    break;
                }
            }

            var customersMoneyBeforePurchase = customer.Money;
            _shopManager.Buy(productsToBuy, customer, _shop);
            
            Assert.Less(_shopManager.CostOfProductsInTheShop(productsToBuy, _shop), customersMoneyBeforePurchase);
            Assert.Less(shopsMoneyBeforePurchase, _shop.Money);
            Assert.Less(_shop.GetProducts().Sum(product => product.Amount), shopsCountOfProductsBeforePurchase);
        }
    }
}