using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Comments { get; set; }
        public int StatusId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
    }
}
