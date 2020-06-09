using System.Collections.Generic;

namespace Payment.Api.Models
{
    public class PaymentSummary
    {
        public PaymentSummary()
        {
            Transactions = new List<PaymentTransaction>();
        }

        public double TotalBalance { get; set; }
        public List<PaymentTransaction> Transactions { get; set; }
    }
}
