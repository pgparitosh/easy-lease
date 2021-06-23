using EasyLease.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyLease.Web.BusinessModels
{
    public class ProductDetails
    {
        public Product ProductBaseDetails { get; set; }
        public ProductMortgageDetails ProductMortgageDetails { get; set; }
        public List<Transaction> ProductTransactions { get; set; }
    }
}
