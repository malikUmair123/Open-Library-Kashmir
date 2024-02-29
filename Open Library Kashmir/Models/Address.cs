using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Open_Library_Kashmir.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [MaxLength(100)]
        public string AddressLine1 { get; set; }

        [MaxLength(100)]
        public string AddressLine2 { get; set; }

        [MaxLength(50)] // Optional
        public string Locality { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Tehsil { get; set; }

        [MaxLength(50)]
        public string District { get; set; }

        [MaxLength(50)]
        public string State { get; set; } // Consider using an enum if you want to enforce a controlled list

        [RegularExpression(@"^\d{6}$")] // Regular expression for 6-digit pin code
        public string PinCode { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }
        public string UserId { get; set; } // Foreign key
        public virtual ApplicationUser User { get; set; }
    }
}