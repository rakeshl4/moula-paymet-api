using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.Api.Services
{
    public interface IPaymentService
    {
        Task<Guid> CreatePayment(Models.Payment paymentRequest);
        Task<Models.Payment> CancelPayment(Guid transactionId, string comments);
        Task<Models.Payment> ApprovePayment(Guid transactionId);
        Task<Models.PaymentSummary> GetAllPayments(Guid customerId);
    }
}
