namespace Payment.Api.Infrastructure.Exception
{
    public class PaymentDomainException : System.Exception
    {
        public PaymentDomainException()
        { }

        public PaymentDomainException(string message)
            : base(message)
        { }

        public PaymentDomainException(string message,
            System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
