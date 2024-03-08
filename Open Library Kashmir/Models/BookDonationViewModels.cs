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
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(128)]
        public string PhoneNumber { get; set; }

        public Address Address { get; set; }

        //[MaxLength(500)] // No need to specify max length as it's already set to max
        public string Remarks { get; set; }
    }



}