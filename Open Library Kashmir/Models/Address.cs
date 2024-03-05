namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        public int AddressId { get; set; }

        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(100)]
        public string Locality { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }
    }
}
