using System;

namespace Barcode
{
    public class Product
    {
        private static uint _id = 1;
        private string _name;
        public uint Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Value cannot be null or empty.", nameof(value));
                _name = value;
            }
        }

        public decimal Price { get; set; }
        public bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }

        public Product(string name, decimal price)
        {
            Id = _id++;
            Name = name;
            Price = price;
        }

        public Product()
        {
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Price)}: {Price}";
        }
    }
}