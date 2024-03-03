namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Distribution")]
    public partial class Distribution
    {
        [Key]
        public int Distribution_ID { get; set; }

        public int Book_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Recipient_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_Distributed { get; set; }

        public virtual Book Book { get; set; }

        public virtual Recipient Recipient { get; set; }
    }
}
