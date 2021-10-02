namespace Shops.Services
{
    public class Customer
    {
        private string Name { get; }

        public float Money { get; set; }

        public Customer(float money, string name)
        {
            Name = name;
            Money = money;
        }
    }
}