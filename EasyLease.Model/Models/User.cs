using System;
using System.ComponentModel.DataAnnotations;

namespace EasyLease.Model.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }

        [Required]
        public long OrganizationId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string PreviousPassword { get; set; }

        [Required]
        public DateTime LastPasswordChangeDate { get; set; }

        public DateTime LastLoginAttemptDate { get; set; }

        public DateTime LastSuccessfulLogin { get; set; }

        [Required]
        public int FailedAttemptsCount { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public long UpdatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
