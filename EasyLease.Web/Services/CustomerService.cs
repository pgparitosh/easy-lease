using EasyLease.Model.Data;
using EasyLease.Model.Extensions;
using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyLease.Web.Services
{
    public class CustomerService : Interface.ICustomerService
    {
        readonly EasyLeaseDbContext _dbContext;
        public CustomerService(EasyLeaseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddCustomer(Customer customer, long userId)
        {
            customer.CreatedBy = userId;
            customer.CreatedTime = DateTime.Now;
            customer.UpdatedBy = userId;
            customer.UpdatedTime = DateTime.Now;
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();
            return customer.CustomerId;
        }

        public IEnumerable<Customer> GetAccessibleCustomers(long orgId)
        {
            return (from org in _dbContext.Organizations
                    join user in _dbContext.Users on org.OrganizationId equals user.UserId
                    join customer in _dbContext.Customers on user.UserId equals customer.CreatedBy
                    where org.OrganizationId == orgId
                    select customer).ToList();
        }

        public Customer GetCustomerById(long customerId)
        {
            return _dbContext.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault() ?? new Customer();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _dbContext.Customers;
        }

        public bool InActivateCustomer(long customerId)
        {
            var customer = _dbContext.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault();
            if (customer == null) return false;
            customer.IsActive = false;
            _dbContext.SaveChanges();
            return true;
        }

        public Customer UpdateCustomer(Customer customer, long userId)
        {
            var dbCustomer = GetCustomerById(customer.CustomerId);
            if (dbCustomer.CustomerId == 0) return new Customer();
            dbCustomer = dbCustomer.CopyCustomerDetails(customer, userId);
            _dbContext.SaveChanges();
            return dbCustomer;
        }
    }
}
