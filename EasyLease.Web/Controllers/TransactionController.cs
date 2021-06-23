using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using EasyLease.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private const string CONTROLLER_NAME = "Transaction";
        readonly ITransactionService _transactionService;
        readonly IErrorService _errorService;
        public TransactionController(ITransactionService transactionService, IErrorService errorService)
        {
            _transactionService = transactionService;
            _errorService = errorService;
        }

        [HttpPost]
        [Route("AddTransaction")]
        public long AddTransaction(Transaction transaction)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _transactionService.AddTransaction(transaction, orgId, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "AddProduct", userId, JsonConvert.SerializeObject(transaction), ex);
                return -1;
            }
        }

        [HttpPost]
        [Route("UpdateTransaction")]
        public Transaction UpdateTransaction(Transaction transaction)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _transactionService.UpdateTransaction(transaction, userId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "UpdateTransaction", userId, JsonConvert.SerializeObject(transaction), ex);
                return new Transaction();
            }
        }

        [HttpGet]
        [Route("GetTransactionsForCustomer/{id}")]
        public IEnumerable<Transaction> GetTransactionsForCustomer(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _transactionService.GetAllTransactionsByCustomerId(orgId, id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetTransactionsForCustomer", userId, id.ToString(), ex);
                return new List<Transaction>();
            }
        }

        [HttpGet]
        [Route("GetTransactionsForProduct/{id}")]
        public IEnumerable<Transaction> GetTransactionsForProduct(long id)
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _transactionService.GetAllTransactionsByProductId(id);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetTransactionsForProduct", userId, id.ToString(), ex);
                return new List<Transaction>();
            }
        }

        [HttpGet]
        [Route("GetAllTransactions")]
        public IEnumerable<Transaction> GetAllTransactions()
        {
            var loggedInUserDetails = HttpContext.User.LoggedInUserDetails();
            var orgId = loggedInUserDetails.Item1;
            var userId = loggedInUserDetails.Item2;
            try
            {
                return _transactionService.GetAllTransactions(orgId);
            }
            catch (Exception ex)
            {
                _errorService.LogError(CONTROLLER_NAME, "GetAllTransactions", userId, "", ex);
                return new List<Transaction>();
            }
        }
    }
}
