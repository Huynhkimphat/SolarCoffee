using System;
using System.ComponentModel.DataAnnotations;

namespace SolarCoffee.Web.ViewModels
{
    public class CustomerModel
    {
        public int Id { get; set; }

        public DateTime CreateOn { get; set; }

        public DateTime UpdateOn { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public CustomerAddressModel PrimaryAddress { get; set; }
    }

    public class CustomerAddressModel
    {
        public int Id { get; set; }

        public DateTime CreateOn { get; set; }

        public DateTime UpdateOn { get; set; }

        [MaxLength(100)]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [MaxLength(100)]
        public string AddressLine3 { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }
    }
}
