namespace Barcode.Product
{
    public class Product
    {
        private static uint _id = 1;
        public uint Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }

        public Product(string name, decimal price)
        {
            Id = _id++;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Price)}: {Price}";
        }
    }
}