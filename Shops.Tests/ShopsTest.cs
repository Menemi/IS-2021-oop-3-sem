using System.Collections.Generic;
using System.Linq;
using Shops.Services;
using Shops.Tools;
using NUnit.Framework;

namespace Shops.Tests
{
    public class Tests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void DeliveryOfProductsToTheShop()
        {
            Shop shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");

            _shopManager.RegisterProduct("Milk");
            _shopManager.RegisterProduct("Eggs");
            _shopManager.RegisterProduct("Meat");

            var products = new List<Product>
            {
                new Product(3, 3, "Milk"),
                new Product(13, 33, "Meat"),
                new Product(60, 5, "Eggs")
            };
            shop.Delivery(products, _shopManager.ProductsName);

            foreach (var product in products)
            {
                Assert.Contains(product, shop.Products);
            }
        }

        [Test]
        public void ChangingPriceForSomeProducts()
        {
            Assert.Catch<ShopsException>(() =>
            {
                Shop shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");

                _shopManager.RegisterProduct("Milk");
                _shopManager.RegisterProduct("Eggs");
                _shopManager.RegisterProduct("Meat");

                var products = new List<Product>
                {
                    new Product(3, 3, "Milk"),
                    new Product(13, 33, "Meat"),
                    new Product(60, 5, "Eggs")
                };
                shop.Delivery(products, _shopManager.ProductsName);

                shop.PriceChanging("Kilk", 4);
            });
        }

        [Test]
        public void TheBestShopSearching()
        {
            Shop shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");
            Shop shop2 = _shopManager.CreateShop("Pyaterochka", "Udarnikov Street 14");

            _shopManager.RegisterProduct("Milk");
            _shopManager.RegisterProduct("Eggs");
            _shopManager.RegisterProduct("Meat");

            var products1 = new List<Product>
            {
                new Product(3, 3, "Milk"),
                new Product(13, 33, "Meat"),
                new Product(60, 5, "Eggs")
            };
            shop.Delivery(products1, _shopManager.ProductsName);

            var products2 = new List<Product>
            {
                new Product(3, 4, "Milk"),
                new Product(13, 34, "Meat"),
                new Product(60, 5, "Eggs")
            };
            shop2.Delivery(products2, _shopManager.ProductsName);

            var products = new Dictionary<string, int>
            {
                {"Milk", 2},
                {"Meat", 13},
                {"Eggs", 60}
            };

            Assert.AreEqual(_shopManager.TheBestShopSearching(products), shop);
        }

        [Test]
        public void BuyingOfProductsInTheShop()
        {
            Shop shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");

            _shopManager.RegisterProduct("Milk");
            _shopManager.RegisterProduct("Eggs");
            _shopManager.RegisterProduct("Meat");

            var productsToDelivery = new List<Product>
            {
                new Product(3, 3, "Milk"),
                new Product(13, 33, "Meat"),
                new Product(60, 5, "Eggs")
            };
            shop.Delivery(productsToDelivery, _shopManager.ProductsName);

            var productsToBuy = new Dictionary<string, int>
            {
                {"Milk", 2},
                {"Meat", 12},
                {"Eggs", 59}
            };

            var shopsMoneyBeforePurchase = shop.Money;
            Customer customer = new Customer(1000000, "Taylor Swift");
            int shopsCountOfProductsBeforePurchase = 0, shopsCountOfProductsAfterPurchase = 0;

            shopsCountOfProductsBeforePurchase += shop.Products.Sum(product => product.Amount);
            Assert.True(_shopManager.AvailabilityOfProducts(productsToBuy, shop));
            Assert.Less(_shopManager.CostOfProductsInTheShop(productsToBuy, shop), customer.Money);
            shop.Buy(productsToBuy, _shopManager.ProductsName, customer);
            Assert.Less(shopsMoneyBeforePurchase, shop.Money);
            shopsCountOfProductsAfterPurchase += shop.Products.Sum(product => product.Amount);
            Assert.Less(shopsMoneyBeforePurchase, shopsCountOfProductsAfterPurchase);
        }
    }
}