using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Open_Library_Kashmir.Models
{
    [Table("BookOfTheMonth")]
    public class BookOfTheMonth
    {

        [Key]
        public int BookId { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(255)]
        public string Title { get; set; }

        [DisplayName("Author")]

        [StringLength(255)]
        public string Author { get; set; }

        [DisplayName("Month")]
        public DateTime MonthYear { get; set; }

        [DisplayName("Image")]
        public string ImageUrl { get; set; }

        [NotMapped] // ImageFile is for temporary upload only
        [DisplayName("Upload Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [DisplayName("Description")]
        public string ShortDescription { get; set; }

    }
}