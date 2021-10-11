using System.Collections.Generic;
using System.Linq;
using Shops.Services;
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

            var products = new List<ShopProduct>
            {
                new ShopProduct(3, 3, "Milk"),
                new ShopProduct(13, 33, "Meat"),
                new ShopProduct(60, 5, "Eggs")
            };
            shop.AddProducts(products);

            foreach (var product in products)
            {
                Assert.Contains(product, shop.GetProducts());
            }
        }

        [Test]
        public void ChangingPriceForSomeProducts()
        {
            Shop shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");

            _shopManager.RegisterProduct("Milk");
            _shopManager.RegisterProduct("Eggs");
            _shopManager.RegisterProduct("Meat");

            var products = new List<ShopProduct>
            {
                new ShopProduct(3, 3, "Milk"),
                new ShopProduct(13, 33, "Meat"),
                new ShopProduct(60, 5, "Eggs")
            };
            shop.AddProducts(products);

            shop.ChangePrice("Meat", 4);
        }

        [Test]
        public void TheBestShopSearching()
        {
            Shop shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");
            Shop shop2 = _shopManager.CreateShop("Pyaterochka", "Udarnikov Street 14");

            _shopManager.RegisterProduct("Milk");
            _shopManager.RegisterProduct("Eggs");
            _shopManager.RegisterProduct("Meat");

            var products1 = new List<ShopProduct>
            {
                new ShopProduct(3, 3, "Milk"),
                new ShopProduct(13, 33, "Meat"),
                new ShopProduct(60, 5, "Eggs")
            };
            shop.AddProducts(products1);

            var products2 = new List<ShopProduct>
            {
                new ShopProduct(3, 4, "Milk"),
                new ShopProduct(13, 34, "Meat"),
                new ShopProduct(60, 5, "Eggs")
            };
            shop2.AddProducts(products2);

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

            var productsToDelivery = new List<ShopProduct>
            {
                new ShopProduct(3, 3, "Milk"),
                new ShopProduct(13, 33, "Meat"),
                new ShopProduct(60, 5, "Eggs")
            };
            shop.AddProducts(productsToDelivery);

            var productsToBuy = new Dictionary<string, int>
            {
                {"Milk", 2},
                {"Meat", 12},
                {"Eggs", 59}
            };

            var shopsMoneyBeforePurchase = shop.Money;
            var customer = new Customer(1000000, "Taylor Swift");
            var shopsCountOfProductsBeforePurchase = shop.GetProducts().Sum(product => product.Amount);

            foreach (var product in productsToBuy)
            {
                Assert.Contains(product.Key, _shopManager.RegisteredProducts);
                foreach (var shopProduct in shop.GetProducts())
                {
                    if (shopProduct.Name == product.Key)
                    {
                        Assert.True(product.Value <= shopProduct.Amount);
                        break;
                    }
                }
            }

            Assert.Less(_shopManager.CostOfProductsInTheShop(productsToBuy, shop), customer.Money);
            shop.Buy(productsToBuy, customer);
            Assert.Less(shopsMoneyBeforePurchase, shop.Money);
            Assert.Less(shop.GetProducts().Sum(product => product.Amount), shopsCountOfProductsBeforePurchase);
        }
    }
}