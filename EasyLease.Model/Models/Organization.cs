using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Model.Models
{
    public class Organization
    {
        [Key]
        public long OrganizationId { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int MaxAllowedUsers { get; set; }

        [Required]
        public int MaxLoginRetrtyAttempts { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
