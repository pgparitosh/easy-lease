using EasyLease.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Model.Models
{
    public class ProductMortgageDetails
    {
        [Key]
        public long MortgageId { get; set; }
        
        [Required]
        public long ProductId { get; set; }

        [Required]
        public long CustomerId { get; set; }

        [Required]
        public ProductPaymentStatusTypeValues PaymentStatus { get; set; }

        [Required]
        public ProductStatusTypeValues CurrentStatus { get; set; }

        [Required]
        public DateTime ProductMortgageDate { get; set; }

        [Required]
        public DateTime ProductMortgageEndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ApproximateValueAsOfCurrentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal InterestRate { get; set; }

        [Required]
        public ProductRepaymentTypeValues RepaymentType { get; set; }

        [Required]
        public int RepaymentTenureInMonths { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ApproximateIncome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ApproximateProfit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal LendedAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PendingAmount { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
