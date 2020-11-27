using System;

namespace Barcode
{
    public class Product
    {
        private static uint id = 1;
        private string name;

        public Product(string name, decimal price) : this()
        {
            Name = name;
            Price = price;
        }

        public Product()
        {
            Id = id++;
        }
        
        public uint Id { get; set; }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Value cannot be null or empty.", nameof(value));
                name = value;
            }
        }

        public decimal Price { get; set; }
        public bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }

        public override string ToString()
        {
            return $"#{Id, 5} | {Name, 40} {Price, 10} credits";
        }
    }
}