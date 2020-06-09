using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.Api.Repositories
{
    public interface IDataRepository
    {
        Task<Guid> CreatePayment(Models.Payment paymentRequest);
        Task<Models.Payment> GetTransaction(Guid transactionId);
        Task<List<Models.Payment>> GetAllPayments(Guid customerId);
        void UpdatePayment(Models.Payment transaction);
        Task<Models.Customer> GetCustomer(Guid customerId);
        void UpdateCustomer(Models.Customer customer);
    }
}
