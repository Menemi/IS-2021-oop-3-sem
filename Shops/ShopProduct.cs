using Shops.Services;

namespace Shops
{
    public class ShopProduct : Product
    {
        private static int _idCounter = 1;

        public ShopProduct(int amount, float price, string name)
            : base(name)
        {
            Price = price;
            Amount = amount;
            Id = _idCounter++;
        }

        public float Price { get; set; }

        public int Amount { get; set; }

        public int Id { get; set; }

        public new virtual bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            ShopProduct product = (ShopProduct)obj;
            return Id == product.Id;
        }
    }
}