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
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Distributions = new HashSet<Distribution>();
            this.Donations = new HashSet<Donation>();
        }
    
        public int book_id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string isbn { get; set; }
        public string genre { get; set; }
        public string condition { get; set; }
        public Nullable<int> publication_year { get; set; }
        public string short_description { get; set; }
        public string @class { get; set; }
        public string subject { get; set; }
        public string publisher { get; set; }
        public byte[] book_image { get; set; }
        public string status { get; set; }
        public string available_at { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Distribution> Distributions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donation> Donations { get; set; }
    }
}
