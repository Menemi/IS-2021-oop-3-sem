namespace Shops.Services
{
    public class Product
    {
        private static int _idCounter = 1;

        public Product(string name)
        {
            Name = name;
            Id = _idCounter++;
        }

        public string Name { get; }

        public int Id { get; }

        public new virtual bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Product product = (Product)obj;
            return Id == product.Id;
        }
    }
}