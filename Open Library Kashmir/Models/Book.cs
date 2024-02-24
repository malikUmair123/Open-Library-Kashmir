//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Distributions = new HashSet<Distribution>();
            this.Donations = new HashSet<Donation>();
        }
    
        public int Book_ID { get; set; }

        [UIHint("OpenInNewWindow")]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }

        [Display(Name = "Publication Year")]

        [Range(1000, 9999, ErrorMessage = "Please enter a valid year between 1000 and 9999.")]
        public Nullable<int> Publication_Year { get; set; }
        public string Condition { get; set; }
        public string Class { get; set; }
        public string Genre { get; set; }
        public string Subject { get; set; }
        public string ISBN { get; set; }

        [Display(Name = "Image Path")]
        public string Image_Path { get; set; }
      
        [Display(Name = "Short Description")]
        public string Short_Description { get; set; }
        public string Status { get; set; }

        [Display(Name = "Available At")]
        public string Available_At { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Distribution> Distributions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donation> Donations { get; set; }
    }

    public enum Status {

        Available = 0,
        Donated = 1
    }
}
