using System;

namespace Payment.Api.Models
{
    public class ApprovePaymentRequest
    {
        public Guid TransactionId { get; set; } 
    }
}
