using Payment.Api.Repositories;
using Payment.Api.Models;
using System;
using System.Threading.Tasks;
using Payment.Api.Infrastructure.Exception;
using System.Linq;

namespace Payment.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataRepository _dataRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IDataRepository dataRepository, IUnitOfWork unitOfWork)
        {
            _dataRepository = dataRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreatePayment(Models.Payment paymentRequest)
        {
            var customer = await _dataRepository.GetCustomer(paymentRequest.CustomerId);
            if (customer == null)
                throw new PaymentDomainException("Invalid Customer Id");
            if (customer.TotalBalance < paymentRequest.Amount)
                throw new PaymentDomainException("Not enough funds");
            paymentRequest.StatusId = (int)TransactionStatus.Pending;
            var result = await _dataRepository.CreatePayment(paymentRequest);
            await _unitOfWork.Commit();
            return result;
        }

        public async Task<Models.Payment> CancelPayment
            (Guid transactionId, string comments)
        {
            var transaction = await _dataRepository.GetTransaction(transactionId);
            if (transaction == null)
                return null;

            if (transaction.StatusId == (int)TransactionStatus.Closed)
                throw new PaymentDomainException("The payment is already closed");

            transaction.StatusId = (int)TransactionStatus.Closed;
            transaction.Comments = comments;
            _dataRepository.UpdatePayment(transaction);
            await _unitOfWork.Commit();
            return transaction;
        }

        public async Task<Models.Payment> ApprovePayment(Guid transactionId)
        {
            var transaction = await _dataRepository.GetTransaction(transactionId);
            if (transaction == null)
                return null;

            if (transaction.StatusId == (int)TransactionStatus.Closed)
                throw new PaymentDomainException("The payment is already closed");

            var customer = await _dataRepository.GetCustomer(transaction.CustomerId);
            if (customer.TotalBalance < transaction.Amount)
                throw new PaymentDomainException("Not enough funds");

            customer.TotalBalance = customer.TotalBalance - transaction.Amount;
            transaction.StatusId = (int)TransactionStatus.Closed;
            transaction.Comments = "Processed";

            _dataRepository.UpdatePayment(transaction);
            _dataRepository.UpdateCustomer(customer);
            await _unitOfWork.Commit();
            return transaction;
        }

        public async Task<PaymentSummary> GetAllPayments(Guid customerId)
        {
            var customer = await _dataRepository.GetCustomer(customerId);
            if (customer == null)
                throw new PaymentDomainException("Invalid Customer Id");
            var summary = new PaymentSummary();
            summary.TotalBalance = customer.TotalBalance;
            var collection = await _dataRepository.GetAllPayments(customerId);
            foreach (var item in collection.OrderBy(a => a.TransactionDate))
            {
                summary.Transactions.Add(new PaymentTransaction()
                {
                    Amount = item.Amount,
                    Id = item.Id,
                    Status = (item.StatusId == (int)TransactionStatus.Pending) ? "Pending" : "Closed",
                    Comments = item.Comments,
                    TransactionDate = item.TransactionDate
                });
            }

            return summary;
        }
    }
}
