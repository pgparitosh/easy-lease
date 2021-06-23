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
    public class Transaction
    {
        [Key]
        public long TransactionId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public long CustomerId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TransactionAmount { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
