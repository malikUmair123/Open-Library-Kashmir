namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public enum BookStatus
    {
        Donated,
        Available
    }

    [Table("Book")]
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            DonorBooks = new HashSet<DonorBook>();
            RecipientBooks = new HashSet<RecipientBook>();
            Wishlists = new HashSet<Wishlist>();
        }

        [Key]
        public int BookId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(255)]
        public string Publisher { get; set; }
        
        [DisplayName("Publication Year")]
        public int? PublicationYear { get; set; }

        [StringLength(20)]
        public string Condition { get; set; }

        [StringLength(100)]
        public string Class { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        [StringLength(100)]
        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        [NotMapped] // ImageFile is for temporary upload only
        [DisplayName("Upload Image")]
        public HttpPostedFileBase ImageFile { get; set; }
        
        [StringLength(500)]
        [DisplayName("Description")]
        public string ShortDescription { get; set; }
        public BookStatus Status { get; set; }

        [StringLength(255)]
        [DisplayName("Available At")]
        public string AvailableAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonorBook> DonorBooks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipientBook> RecipientBooks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }

}
