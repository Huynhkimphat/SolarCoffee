using System;

namespace SolarCoffee.Data.Models
{
    public class ProductInventory
    {
        public int Id { get; set; }

        public DateTime CreateOn { get; set; }

        public DateTime UpdateOn { get; set; }

        public int QuantityOnHand { get; set; }

        public int IdealOnHand { get; set; }

        public Product Product { get; set; }
    }
}