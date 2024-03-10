namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        public int AddressId { get; set; }

        [DisplayName("Address Line 1")]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [DisplayName("Tehsil")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(100)]
        public string Locality { get; set; }

        [DisplayName("District")]
        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [DisplayName("Country")]
        [StringLength(2)]
        public string CountryCode { get; set; }

        [DisplayName("Pin Code")]
        [StringLength(20)]
        public string PostalCode { get; set; }
    }
}
