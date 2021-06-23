using EasyLease.Model.Data;
using EasyLease.Model.Enums;
using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services
{
    public class TransactionService : Interface.ITransactionService
    {
        readonly EasyLeaseDbContext _dbContext;
        readonly ICustomerService _customerService;
        readonly IProductService _productService;

        public TransactionService(EasyLeaseDbContext dbContext, ICustomerService customerService, IProductService productService)
        {
            _dbContext = dbContext;
            _customerService = customerService;
            _productService = productService;
        }

        public long AddTransaction(Transaction transaction, long orgId, long userId)
        {
            if (transaction.TransactionAmount <= 0) return -3;
            var accessibleCustomerIds = _customerService.GetAccessibleCustomers(orgId).Select(x => x.CustomerId).ToList();
            if(!accessibleCustomerIds.Contains(transaction.CustomerId)) return -2;
            var product = _productService.GetProductById(transaction.ProductId);
            if (product == null) return -2;
            var paymentStatusType = product.ProductMortgageDetails.PaymentStatus;
            if(product.ProductMortgageDetails.PendingAmount <= transaction.TransactionAmount)
                paymentStatusType = ProductPaymentStatusTypeValues.FullyPaid;
            else
                paymentStatusType = ProductPaymentStatusTypeValues.PartiallyPaid;
            var updatedPendingAmount = product.ProductMortgageDetails.PendingAmount - transaction.TransactionAmount;
            if (updatedPendingAmount < 0) updatedPendingAmount = 0;
            transaction.CreatedBy = userId;
            transaction.UpdatedBy = userId;
            transaction.CreatedTime = DateTime.Now;
            transaction.UpdatedTime = DateTime.Now;
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            _productService.UpdateProductMortgageDetails(product.ProductBaseDetails.ProductId, paymentStatusType, updatedPendingAmount, userId);
            return transaction.TransactionId;
        }

        public IEnumerable<Transaction> GetAllTransactions(long orgId)
        {
            var accessibleCustomerIds = _customerService.GetAccessibleCustomers(orgId).Select(x => x.CustomerId).ToList();
            return _dbContext.Transactions.Where(x => accessibleCustomerIds.Contains(x.CustomerId)).ToList();
        }

        public IEnumerable<Transaction> GetAllTransactionsByCustomerId(long orgId, long customerId)
        {
            var accessibleCustomerIds = _customerService.GetAccessibleCustomers(orgId).Select(x => x.CustomerId).ToList();
            if (!accessibleCustomerIds.Contains(customerId)) return new List<Transaction>();
            return _dbContext.Transactions.Where(x => x.CustomerId == customerId).ToList();
        }

        public IEnumerable<Transaction> GetAllTransactionsByProductId(long productId)
        {
            return (from transaction in _dbContext.Transactions
                   where transaction.ProductId == productId
                   select transaction).ToList();
        }

        public Transaction UpdateTransaction(Transaction transaction, long userId)
        {
            var dbTransaction = _dbContext.Transactions.Where(x => x.TransactionId == transaction.TransactionId).FirstOrDefault();
            if (dbTransaction == null) return new Transaction();
            dbTransaction = dbTransaction.CoptTransactionDetails(transaction, userId);
            return dbTransaction;
        }
    }
}
