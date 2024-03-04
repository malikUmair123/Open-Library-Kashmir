using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Open_Library_Kashmir.Models
{
    public class WishlistViewModel
    {
        public List<Book> Books { get; set; }
    }

    public class RecipientViewModel 
    {
        [Required]
        [StringLength(50)]
        public string First_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Last_Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Aadhar_Card_Path { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }

}