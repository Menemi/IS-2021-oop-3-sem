namespace Shops.Services
{
    public class Customer
    {
        public Customer(float money, string name)
        {
            Name = name;
            Money = money;
        }

        public float Money { get; set; }

        private string Name { get; }
    }
}