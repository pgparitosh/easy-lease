using EasyLease.Model.Models;
using System.Collections.Generic;

namespace EasyLease.Web.Services.Interface
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetAllTransactions(long orgId);
        IEnumerable<Transaction> GetAllTransactionsByCustomerId(long orgId, long customerId);
        IEnumerable<Transaction> GetAllTransactionsByProductId(long productId);
        long AddTransaction(Transaction transaction, long orgId, long userId);
        Transaction UpdateTransaction(Transaction transaction, long userId);
    }
}
