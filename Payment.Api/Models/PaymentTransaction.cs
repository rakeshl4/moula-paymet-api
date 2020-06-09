using System;

namespace Payment.Api.Models
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; } 
        public double Amount { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
