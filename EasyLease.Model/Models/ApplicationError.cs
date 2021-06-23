using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLease.Model.Models
{
    public class ApplicationError
    {
        [Key]
        public int ErrorId { get; set; }
        public long UserId { get; set; }
        public string Message  { get; set; }
        public string InnerException { get; set; }
        public DateTime LoggedAt { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string InputDataString { get; set; }
    }
}
