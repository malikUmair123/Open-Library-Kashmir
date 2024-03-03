namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Donation")]
    public partial class Donation
    {
        [Key]
        public int Donation_ID { get; set; }

        public int Book_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Donor_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_Donated { get; set; }

        public virtual Book Book { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
