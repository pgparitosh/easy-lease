using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.Services.Interface
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomers();
        long AddCustomer(Customer customer, long userId);
        Customer GetCustomerById(long customerId);
        bool InActivateCustomer(long customerId);
        Customer UpdateCustomer(Customer customer, long userId);
        IEnumerable<Customer> GetAccessibleCustomers(long orgId);
    }
}
