namespace Open_Library_Kashmir.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class SessionWishlist
    {
        public string Recipient_ID { get; set; }
        public int WishlistId { get; set; }
        public List<int> BookIds { get; set; }

        public SessionWishlist()
        {
            BookIds = new List<int>();
        }
    }
}
