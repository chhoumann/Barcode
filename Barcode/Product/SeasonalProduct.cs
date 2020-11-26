using System;

namespace Barcode
{
    public class SeasonalProduct : Product
    {
        public SeasonalProduct(string name, decimal price) : base(name, price) { }
        
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonEndDate { get; set; }
    }
}