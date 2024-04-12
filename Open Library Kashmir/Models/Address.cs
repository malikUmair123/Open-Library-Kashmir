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

        [DisplayName("Address Line 1 Other")]
        [StringLength(100)]
        public string AddressLine1Other { get; set; }


        [DisplayName("Tehsil")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [DisplayName("Tehsil Other")]
        [StringLength(100)]
        public string AddressLine2Other { get; set; }


        [StringLength(100)]
        public string Locality { get; set; }

        [DisplayName("District")]
        [StringLength(100)]
        public string City { get; set; }

        [DisplayName("District Other")]
        [StringLength(100)]
        public string CityOther { get; set; }


        [StringLength(100)]
        public string State { get; set; }

        [DisplayName("State Other")]
        [StringLength(100)]
        public string StateOther { get; set; }


        [DisplayName("Country")]
        [StringLength(100)]
        public string Country { get; set; }

        [DisplayName("Country Other")]
        [StringLength(100)]
        public string CountryOther { get; set; }


        [DisplayName("Pin Code")]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [DisplayName("Pin Code Other")]
        [StringLength(20)]
        public string PostalCodeOther { get; set; }
    }
}
