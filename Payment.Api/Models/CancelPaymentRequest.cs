using System;

namespace Payment.Api.Models
{
    public class CancelPaymentRequest
    {
        public Guid TransactionId { get; set; }
        public string Comments { get; set; }
    }
}
