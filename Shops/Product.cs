namespace Shops.Services
{
    public class Product
    {
        public Product(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public new virtual bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Product product = (Product)obj;
            return Name == product.Name;
        }
    }
}