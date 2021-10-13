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
            var shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");

            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Eggs");
            var eggs = _shopManager.RegisterProduct("Meat");

            var products = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
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
            var shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");

            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Eggs");
            var eggs = _shopManager.RegisterProduct("Meat");

            var products = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            shop.AddProducts(products);

            shop.ChangePrice("Meat", 4);
        }

        [Test]
        public void TheBestShopSearching()
        {
            var shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");
            var shop2 = _shopManager.CreateShop("Pyaterochka", "Udarnikov Street 14");

            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Eggs");
            var eggs = _shopManager.RegisterProduct("Meat");
            
            var products1 = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            shop.AddProducts(products1);

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

            Assert.AreEqual(_shopManager.TheBestShopSearching(products), shop);
        }

        [Test]
        public void BuyingOfProductsInTheShop()
        {
            var shop = _shopManager.CreateShop("Pyaterochka", "Industrial Avenue 18");
            
            var milk = _shopManager.RegisterProduct("Milk");
            var meat = _shopManager.RegisterProduct("Eggs");
            var eggs = _shopManager.RegisterProduct("Meat");
            
            var productsToDelivery = new List<ShopProduct>
            {
                new ShopProduct(milk, 3, 3),
                new ShopProduct(meat, 13, 33),
                new ShopProduct(eggs, 60, 5)
            };
            shop.AddProducts(productsToDelivery);

            var productsToBuy = new List<ProductToBuy>
            {
                new ProductToBuy(milk, 2),
                new ProductToBuy(meat, 12),
                new ProductToBuy(eggs, 59)
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