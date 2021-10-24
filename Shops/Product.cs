using System;

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

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id);
        }

        protected bool Equals(Product other)
        {
            return Name == other.Name && Id == other.Id;
        }
    }
}