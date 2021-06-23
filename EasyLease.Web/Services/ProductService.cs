using EasyLease.Model.Data;
using EasyLease.Model.Enums;
using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.BusinessModels;
using EasyLease.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services
{
    public class ProductService : Interface.IProductService
    {
        readonly EasyLeaseDbContext _dbContext;
        readonly ITransactionService _transactionService;
        readonly ICustomerService _customerService;
        public ProductService(EasyLeaseDbContext dbContext, ITransactionService transactionService, ICustomerService customerService)
        {
            _dbContext = dbContext;
            _transactionService = transactionService;
            _customerService = customerService;
        }

        public long AddProductDetails(ProductDetails productDetails, long orgId, long userId)
        {
            var accessibleCustomerIds = _customerService.GetAccessibleCustomers(orgId).Select(x => x.CustomerId).ToList();
            if (!accessibleCustomerIds.Contains(productDetails.ProductMortgageDetails.CustomerId)) return -2;
            var currentTimestamp = DateTime.Now;
            productDetails.ProductBaseDetails.IsActive = true;
            productDetails.ProductBaseDetails.CreatedBy = userId;
            productDetails.ProductBaseDetails.CreatedTime = currentTimestamp;
            productDetails.ProductBaseDetails.UpdatedBy = userId;
            productDetails.ProductBaseDetails.UpdatedTime = currentTimestamp;
            productDetails.ProductMortgageDetails.ApproximateProfit = GetApproximateProfit(productDetails.ProductMortgageDetails);
            productDetails.ProductMortgageDetails.ApproximateIncome = productDetails.ProductMortgageDetails.LendedAmount + productDetails.ProductMortgageDetails.ApproximateProfit;
            productDetails.ProductMortgageDetails.CurrentStatus = ProductStatusTypeValues.Mortgaged;
            productDetails.ProductMortgageDetails.PaymentStatus = productDetails.ProductTransactions.Count > 0 ? ProductPaymentStatusTypeValues.PartiallyPaid : ProductPaymentStatusTypeValues.FullyDue;
            productDetails.ProductMortgageDetails.PendingAmount = GetPendingAmount(productDetails);
            productDetails.ProductMortgageDetails.ProductMortgageEndDate = productDetails.ProductMortgageDetails.ProductMortgageDate.AddMonths(productDetails.ProductMortgageDetails.RepaymentTenureInMonths);
            productDetails.ProductMortgageDetails.CreatedBy = userId;
            productDetails.ProductMortgageDetails.CreatedTime = currentTimestamp;
            productDetails.ProductMortgageDetails.UpdatedBy = userId;
            productDetails.ProductMortgageDetails.UpdatedTime = currentTimestamp;
            _dbContext.Products.Add(productDetails.ProductBaseDetails);
            _dbContext.SaveChanges();
            productDetails.ProductMortgageDetails.ProductId = productDetails.ProductBaseDetails.ProductId;
            _dbContext.ProductMortgageDetails.Add(productDetails.ProductMortgageDetails);
            productDetails.ProductTransactions.ForEach(x =>
            {
                x.ProductId = productDetails.ProductBaseDetails.ProductId;
                x.CustomerId = productDetails.ProductMortgageDetails.CustomerId;
                x.CreatedBy = userId;
                x.UpdatedBy = userId;
                x.CreatedTime = currentTimestamp;
                x.UpdatedTime = currentTimestamp;
            });
            _dbContext.Transactions.AddRange(productDetails.ProductTransactions);
            _dbContext.SaveChanges();
            return productDetails.ProductBaseDetails.ProductId;
        }

        private static decimal GetPendingAmount(ProductDetails productDetails)
        {
            decimal allTransactionSum = 0.0M;
            productDetails.ProductTransactions.ForEach(x => allTransactionSum += x.TransactionAmount);
            return productDetails.ProductMortgageDetails.LendedAmount - allTransactionSum;
        }

        private static decimal GetApproximateProfit(ProductMortgageDetails productMortgageDetails)
        {
            return (productMortgageDetails.LendedAmount * productMortgageDetails.InterestRate * 12) / (productMortgageDetails.RepaymentTenureInMonths * 100);
        }

        public IEnumerable<ProductDetails> GetAllProducts(long orgId)
        {
            return (from user in _dbContext.Users
                    join org in _dbContext.Organizations
                    on user.OrganizationId equals org.OrganizationId
                    join product in _dbContext.Products
                    on user.UserId equals product.CreatedBy
                    join productMortgageDetails in _dbContext.ProductMortgageDetails
                    on product.ProductId equals productMortgageDetails.ProductId
                    where org.OrganizationId == orgId
                    select new ProductDetails
                    {
                        ProductBaseDetails = product,
                        ProductMortgageDetails = productMortgageDetails,
                        ProductTransactions = _transactionService.GetAllTransactionsByProductId(product.ProductId).ToList()
                    }).ToList();
        }

        public IEnumerable<ProductDetails> GetAllProductsByCustomerId(long customerId)
        {
            return (from product in _dbContext.Products
                    join productMortgageDetails in _dbContext.ProductMortgageDetails
                    on product.ProductId equals productMortgageDetails.ProductId
                    where productMortgageDetails.CustomerId == customerId
                    select new ProductDetails
                    {
                        ProductBaseDetails = product,
                        ProductMortgageDetails = productMortgageDetails,
                        ProductTransactions = _transactionService.GetAllTransactionsByProductId(product.ProductId).ToList()
                    }).ToList();
        }

        public ProductDetails GetProductById(long productId)
        {
            return (from product in _dbContext.Products
                    join productMortgageDetails in _dbContext.ProductMortgageDetails
                    on product.ProductId equals productMortgageDetails.ProductId
                    where product.ProductId == productId
                    select new ProductDetails
                    {
                        ProductBaseDetails = product,
                        ProductMortgageDetails = productMortgageDetails,
                        ProductTransactions = _transactionService.GetAllTransactionsByProductId(product.ProductId).ToList()
                    }).FirstOrDefault();
        }

        public bool InActivateProduct(long productId, long userId)
        {
            var dbProduct = _dbContext.Products.Where(x => x.ProductId == productId).FirstOrDefault();
            if (dbProduct == null) return false;
            dbProduct.IsActive = false;
            dbProduct.UpdatedBy = userId;
            dbProduct.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return true;
        }

        public ProductDetails UpdateProduct(ProductDetails productDetails, long updatedBy)
        {
            var dbProduct = _dbContext.Products.Where(x => x.ProductId == productDetails.ProductBaseDetails.ProductId).FirstOrDefault();
            if (dbProduct == null) return new ProductDetails();
            var dbProductMortgageDetails = _dbContext.ProductMortgageDetails.Where(x => x.ProductId == productDetails.ProductBaseDetails.ProductId).FirstOrDefault();
            dbProduct = dbProduct.CopyProductDetails(productDetails.ProductBaseDetails, updatedBy);
            dbProductMortgageDetails = dbProductMortgageDetails.CopyProductMortgageDetails(productDetails.ProductMortgageDetails, updatedBy);
            _dbContext.SaveChanges();
            return new ProductDetails { ProductBaseDetails = dbProduct, ProductMortgageDetails = dbProductMortgageDetails, ProductTransactions = productDetails.ProductTransactions };
        }

        public ProductMortgageDetails UpdateProductMortgageDetails(long productId, ProductPaymentStatusTypeValues paymentStatusType, decimal pendingAmount, long userId)
        {
            var dbProductMortgageDetails = _dbContext.ProductMortgageDetails.Where(x => x.ProductId == productId).FirstOrDefault();
            if (dbProductMortgageDetails == null) return new ProductMortgageDetails();
            dbProductMortgageDetails.PendingAmount = pendingAmount;
            dbProductMortgageDetails.PaymentStatus = paymentStatusType;
            dbProductMortgageDetails.UpdatedBy = userId;
            dbProductMortgageDetails.UpdatedTime = DateTime.Now;
            _dbContext.SaveChanges();
            return dbProductMortgageDetails;
        }
    }
}
