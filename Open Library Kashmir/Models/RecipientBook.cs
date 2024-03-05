namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RecipientBook")]
    public partial class RecipientBook
    {
        [Key]
        [Column(Order = 0)]
        public string RecipientId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateRecieved { get; set; }

        public virtual Book Book { get; set; }

        public virtual Recipient Recipient { get; set; }
    }
}
