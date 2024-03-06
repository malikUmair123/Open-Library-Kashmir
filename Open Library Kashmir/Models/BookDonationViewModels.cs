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
        [MaxLength]
        public string FirstName { get; set; }

        [Required]
        [MaxLength]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength]
        public string PhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }

        [MaxLength]
        public string Remarks { get; set; }
    }


}