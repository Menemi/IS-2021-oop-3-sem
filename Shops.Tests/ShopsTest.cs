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
                new ShopProduct(new Product("Milk"), 3, 3),
                new ShopProduct(new Product("Meat"), 13, 33),
                new ShopProduct(new Product("Eggs"), 60, 5)
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
                new ShopProduct(new Product("Milk"), 3, 3),
                new ShopProduct(new Product("Meat"), 13, 33),
                new ShopProduct(new Product("Eggs"), 60, 5)
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
                new ShopProduct(new Product("Milk"), 3, 3),
                new ShopProduct(new Product("Meat"), 13, 33),
                new ShopProduct(new Product("Eggs"), 60, 5)
            };
            shop.AddProducts(products1);

            var products2 = new List<ShopProduct>
            {
                new ShopProduct(new Product("Milk"), 3, 4),
                new ShopProduct(new Product("Meat"), 13, 34),
                new ShopProduct(new Product("Eggs"), 60, 5)
            };
            shop2.AddProducts(products2);

            var products = new List<ProductToBuy>
            {
                new ProductToBuy(new Product("Milk"), 2),
                new ProductToBuy(new Product("Meat"), 13),
                new ProductToBuy(new Product("Eggs"), 60)
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
                new ShopProduct(new Product("Milk"), 3, 3),
                new ShopProduct(new Product("Meat"), 13, 33),
                new ShopProduct(new Product("Eggs"), 60, 5)
            };
            shop.AddProducts(productsToDelivery);

            var productsToBuy = new List<ProductToBuy>
            {
                new ProductToBuy(new Product("Milk"), 2),
                new ProductToBuy(new Product("Meat"), 12),
                new ProductToBuy(new Product("Eggs"), 59)
            };

            var shopsMoneyBeforePurchase = shop.Money;
            var customer = new Customer(1000000, "Taylor Swift");
            var shopsCountOfProductsBeforePurchase = shop.GetProducts().Sum(product => product.Amount);

            foreach (var product in productsToBuy)
            {
                Assert.Contains(product.Product, _shopManager.RegisteredProducts);
                foreach (var shopProduct in shop.GetProducts())
                {
                    if (shopProduct.Product.Id == product.Product.Id)
                    {
                        Assert.True(product.Amount <= shopProduct.Amount);
                        break;
                    }
                }
            }

            Assert.Less(_shopManager.CostOfProductsInTheShop(productsToBuy, shop), customer.Money);
            shop.Buy(productsToBuy, customer, _shopManager.RegisteredProducts);
            Assert.Less(shopsMoneyBeforePurchase, shop.Money);
            Assert.Less(shop.GetProducts().Sum(product => product.Amount), shopsCountOfProductsBeforePurchase);
        }
    }
}