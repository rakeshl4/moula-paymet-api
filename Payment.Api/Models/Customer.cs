using System;

namespace Payment.Api.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double TotalBalance { get; set; }
    }
}
