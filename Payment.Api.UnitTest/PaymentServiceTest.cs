using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Payment.Api.Infrastructure.Exception;
using Payment.Api.Models;
using Payment.Api.Repositories;
using Payment.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Api.UnitTest
{
    [TestClass]
    public class PaymentServiceTest
    {
        [TestMethod]
        public async Task CreatePayment_ForInvalidCustomerId_ThrowsError()
        {
            Models.Customer customer = null;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var paymentRequest = new Models.Payment()
            {
                CustomerId = Guid.NewGuid(),
                Amount = 1000,
                TransactionDate = DateTime.Now
            };

            PaymentDomainException error = null;
            try
            {
                var result = await service.CreatePayment(paymentRequest);
            }
            catch (PaymentDomainException ex)
            {
                error = ex;
            }

            Assert.IsTrue(error != null);
            Assert.IsTrue(error.Message == "Invalid Customer Id");
        }

        [TestMethod]
        public async Task CreatePayment_ForInsufficient_ThrowsError()
        {
            Models.Customer customer = new Models.Customer()
            {
                TotalBalance = 1000
            };
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var paymentRequest = new Models.Payment()
            {
                CustomerId = Guid.NewGuid(),
                Amount = 2000,
                TransactionDate = DateTime.Now
            };

            PaymentDomainException error = null;
            try
            {
                var result = await service.CreatePayment(paymentRequest);
            }
            catch (PaymentDomainException ex)
            {
                error = ex;
            }

            Assert.IsTrue(error != null);
            Assert.IsTrue(error.Message == "Not enough funds");
        }

        [TestMethod]
        public async Task CreatePayment_ForValidData_ShouldUpdateStatusToPending()
        {
            Models.Payment inputData = null;
            Models.Customer customer = new Models.Customer()
            {
                TotalBalance = 10000
            };
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            mockRepo.Setup(service => service.CreatePayment(It.IsAny<Models.Payment>()))
              .Callback<Models.Payment>((obj) => inputData = obj)
              .ReturnsAsync(Guid.NewGuid());
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var paymentRequest = new Models.Payment()
            {
                CustomerId = Guid.NewGuid(),
                Amount = 2000,
                TransactionDate = DateTime.Now
            };

            var result = await service.CreatePayment(paymentRequest);
            Assert.IsTrue(inputData != null);
            Assert.IsTrue(inputData.StatusId == (int)TransactionStatus.Pending);
        }

        [TestMethod]
        public async Task CancelPayment_ForClosedTransaction_ThrowsError()
        {
            Models.Payment payment = new Models.Payment()
            {
                StatusId = (int)TransactionStatus.Closed
            };

            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetTransaction(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            PaymentDomainException error = null;
            try
            {
                var result = await service.CancelPayment(Guid.NewGuid(), string.Empty);
            }
            catch (PaymentDomainException ex)
            {
                error = ex;
            }

            Assert.IsTrue(error != null);
            Assert.IsTrue(error.Message == "The payment is already closed");
        }

        [TestMethod]
        public async Task CancelPayment_WithValidData_ShouldPass()
        {
            Models.Payment inputData = null;
            Models.Payment payment = new Models.Payment()
            {
                StatusId = (int)TransactionStatus.Pending
            };

            Models.Customer customer = new Models.Customer()
            {
                TotalBalance = 10000
            };
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            mockRepo.Setup(service => service.GetTransaction(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
            mockRepo.Setup(service => service.UpdatePayment(It.IsAny<Models.Payment>()))
             .Callback<Models.Payment>((obj) => inputData = obj);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var result = await service.CancelPayment(Guid.NewGuid(), string.Empty);
            Assert.IsTrue(inputData != null);
            Assert.IsTrue(inputData.StatusId == (int)TransactionStatus.Closed);
        }

        [TestMethod]
        public async Task ApprovePayment_ForClosedTransaction_ThrowsError()
        {
            Models.Payment payment = new Models.Payment()
            {
                StatusId = (int)TransactionStatus.Closed
            };

            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetTransaction(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            PaymentDomainException error = null;
            try
            {
                var result = await service.ApprovePayment(Guid.NewGuid());
            }
            catch (PaymentDomainException ex)
            {
                error = ex;
            }

            Assert.IsTrue(error != null);
            Assert.IsTrue(error.Message == "The payment is already closed");
        }

        [TestMethod]
        public async Task ApprovePayment_ForInsufficient_ThrowsError()
        {
            Models.Payment payment = new Models.Payment()
            {
                StatusId = (int)TransactionStatus.Pending,
                Amount = 10000
            };

            Models.Customer customer = new Models.Customer()
            {
                TotalBalance = 1000
            };
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            mockRepo.Setup(service => service.GetTransaction(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            PaymentDomainException error = null;
            try
            {
                var result = await service.ApprovePayment(Guid.NewGuid());
            }
            catch (PaymentDomainException ex)
            {
                error = ex;
            }

            Assert.IsTrue(error != null);
            Assert.IsTrue(error.Message == "Not enough funds");
        }

        [TestMethod]
        public async Task ApprovePayment_WithValidData_ShouldPass()
        {
            Models.Payment inputData = null;
            Models.Payment payment = new Models.Payment()
            {
                StatusId = (int)TransactionStatus.Pending,
                Amount = 1000
            };

            Models.Customer customer = new Models.Customer()
            {
                TotalBalance = 10000
            };
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            mockRepo.Setup(service => service.GetTransaction(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
            mockRepo.Setup(service => service.UpdatePayment(It.IsAny<Models.Payment>()))
             .Callback<Models.Payment>((obj) => inputData = obj);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var result = await service.ApprovePayment(Guid.NewGuid());
            Assert.IsTrue(inputData != null);
            Assert.IsTrue(inputData.Comments == "Processed");
            Assert.IsTrue(inputData.StatusId == (int)TransactionStatus.Closed);
        }

        [TestMethod]
        public async Task GetAllPayments_ForInvalidCustomerId_ThrowsError()
        {
            Models.Customer customer = null;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var paymentRequest = new Models.Payment()
            {
                CustomerId = Guid.NewGuid(),
                Amount = 1000,
                TransactionDate = DateTime.Now
            };

            PaymentDomainException error = null;
            try
            {
                var result = await service.GetAllPayments(Guid.NewGuid());
            }
            catch (PaymentDomainException ex)
            {
                error = ex;
            }

            Assert.IsTrue(error != null);
            Assert.IsTrue(error.Message == "Invalid Customer Id");
        }

        [TestMethod]
        public async Task GetAllPayments_ForValidRequest_ReturnsData()
        {
            Models.Customer customer = new Models.Customer()
            {
                TotalBalance = 10000
            };

            var paymentCollection = new List<Models.Payment>() {
            new  Models.Payment(){
                  Amount = 100,
                  Id = Guid.NewGuid(),
                  StatusId = (int)TransactionStatus.Closed,
                  Comments = "Processed",
                  TransactionDate = DateTime.Now
              }
            };
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(service => service.GetCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(customer);
            mockRepo.Setup(service => service.GetAllPayments(It.IsAny<Guid>()))
                .ReturnsAsync(paymentCollection);
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.Commit());

            var service = new PaymentService(mockRepo.Object, mockUnitOfWork.Object);
            var paymentRequest = new Models.Payment()
            {
                CustomerId = Guid.NewGuid(),
                Amount = 1000,
                TransactionDate = DateTime.Now
            };

            var result = await service.GetAllPayments(Guid.NewGuid());
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.TotalBalance == 10000);
            Assert.IsTrue(result.Transactions.Count == 1);
            Assert.IsTrue(result.Transactions.First().Amount == 100);
        }
    }
}
