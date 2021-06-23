using EasyLease.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Model.Data
{
    public class EasyLeaseDbContext : DbContext
    {
        public EasyLeaseDbContext(DbContextOptions<EasyLeaseDbContext> options): base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMortgageDetails> ProductMortgageDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationError> Errors { get; set; }
    }
}
