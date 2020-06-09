using Payment.Api.Infrastructure;
using System;
using System.Threading.Tasks;
using Payment.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Payment.Api.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly PaymentDbContext _context;

        public DataRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreatePayment(Models.Payment paymentRequest)
        {
            await _context.PaymentCollection.AddAsync(paymentRequest); 
            return paymentRequest.Id;
        }

        public async Task<Models.Payment> GetTransaction(Guid transactionId)
        {
            return await _context.PaymentCollection.FirstOrDefaultAsync
                (p => p.Id == transactionId);
        }

        public async Task<List<Models.Payment>> GetAllPayments(Guid customerId)
        {
            return await _context.PaymentCollection
           .Where(a => a.CustomerId == customerId).ToListAsync();
        }

        public void UpdatePayment(Models.Payment transaction)
        {
            _context.PaymentCollection.Update(transaction);
        }

        public async Task<Customer> GetCustomer(Guid customerId)
        {
            return await _context.CustomerCollection.FirstOrDefaultAsync
                (p => p.Id == customerId);
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.CustomerCollection.Update(customer);
        }
    }
}
