using EasyLease.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Model.Models
{
    public class Customer
    {
        [Key]
        public long CustomerId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string AddressLine1 { get; set; }
        
        [Required]
        public string AddressLine2 { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string State { get; set; }
        
        [Required]
        [MaxLength(6, ErrorMessage ="Invalid Pincode")]
        public string Pincode { get; set; }
        
        [Required]
        public CustomerIdentificationTypeValues CustomerIdentificationType { get; set; }
        
        [Required]
        public string CustomerIdentificationNumber { get; set; }
        
        [Required]
        public string PrimaryPhone { get; set; }
        
        public string SecondaryPhone { get; set; }
        
        public string CustomerImageUrl { get; set; }

        [Required]
        public bool IsActive { get; set; }
        
        public long CreatedBy { get; set; }
        
        public DateTime CreatedTime { get; set; }
        
        public long UpdatedBy { get; set; }
        
        public DateTime UpdatedTime { get; set; }
    }
}
