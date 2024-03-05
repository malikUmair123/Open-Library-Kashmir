namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonorBook")]
    public partial class DonorBook
    {
        [Key]
        [Column(Order = 0)]
        public string DonorId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateDonated { get; set; }

        public virtual Book Book { get; set; }

        public virtual Donor Donor { get; set; }
    }
}
