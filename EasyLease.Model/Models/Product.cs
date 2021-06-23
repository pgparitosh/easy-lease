using EasyLease.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace EasyLease.Model.Models
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }
        
        [Required]
        public string ShortDescription { get; set; }
        
        [Required]
        public string FullDescription { get; set; }
        
        [Required]
        public ProductTypeValues ProductType { get; set; }

        [Required]
        public int Quantity { get; set; }

        public long ProductWeightInGrams { get; set; }

        public string ProductImageUrls { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
