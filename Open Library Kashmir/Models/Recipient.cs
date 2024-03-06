namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    // Define enum for request status
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected
    }


    [Table("Recipient")]
    public partial class Recipient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipient()
        {
            RecipientBooks = new HashSet<RecipientBook>();
            Wishlists = new HashSet<Wishlist>();
        }

        public string RecipientId { get; set; }

        [StringLength(256)]
        public string AadharCardUrl { get; set; }

        // Add property for request status
        public RequestStatus RequestStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipientBook> RecipientBooks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
