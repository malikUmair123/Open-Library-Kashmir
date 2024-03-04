using Microsoft.AspNet.Identity;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Open_Library_Kashmir.Controllers
{
    [RequireHttps]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "NullReference")]
    public class DonationController : Controller
    {
        private readonly BookDonationDataModels _context;

        public DonationController()
        {
            _context = new BookDonationDataModels();
        }

        // GET: Donation

        [OutputCache(CacheProfile = "1MinuteCache", Location = System.Web.UI.OutputCacheLocation.Client)]
        //[OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Donation()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult BookDetails(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Book_ID == id);
            return View(book);
        }

        // Add to Wishlist Action
        [HttpPost]
        public ActionResult AddToWishlist(int id)
        {
            // Retrieve wishlist from the session
            var wishlist = Session["Wishlist"] as SessionWishlist;
            if (wishlist == null)
            {
                wishlist = new SessionWishlist();
                Session["Wishlist"] = wishlist;
            }

            // If the book isn't present
            if (!wishlist.BookIds.Contains(id))
            {
                wishlist.BookIds.Add(id);
                //_context.SaveChanges(); // Assuming you want to persist in an actual wishlist later
            }
            return RedirectToAction("BookDetails", new { id = id });
            //return RedirectToAction("BookDetails", new { id = id });

            //if (User.Identity.IsAuthenticated)
            //{

            //    string recipientId = User.Identity.GetUserId() ?? Guid.NewGuid().ToString(); // For unauthenticated users

            //    var wishlist = _context.Wishlists.FirstOrDefault(w => w.Recipient_ID == recipientId) ??
            //                   new Wishlist { Recipient_ID = recipientId };
            //    _context.Wishlists.Add(wishlist);

            //    if (!wishlist.Books.Any(b => b.Book_ID == id))
            //    {
            //        var book = _context.Books.Find(id);
            //        wishlist.Books.Add(book);
            //    }

            //    _context.SaveChanges();
            //} 
            //else
            //{
            //    //Session["Wishlist"] = wishlist;
            //}

            //////return RedirectToAction("BookDetails", new { id = id });
            //return RedirectToAction("Wishlist");
        }

        public JsonResult GetWishlistCount()
        {
            var wishlist = Session["Wishlist"] as SessionWishlist;
            int count = wishlist != null ? wishlist.BookIds.Count : 0;
            return Json(new { Count = count }, JsonRequestBehavior.AllowGet);
        }


        // Wishlist View 
        public ActionResult Wishlist()
        {
            var wishlist = Session["Wishlist"] as SessionWishlist;

            if (wishlist == null)
            {
                return View(new WishlistViewModel());
            }

            // Fetch books from wishlist
            var wishlistViewModel = new WishlistViewModel
            {
                Books = _context.Books.Where(b => wishlist.BookIds.Contains(b.Book_ID)).ToList()
            };

            return View(wishlistViewModel);
            //if (User.Identity.IsAuthenticated)
            //{
            //    string recipientId = User.Identity.GetUserId();
            //    var wishlist = _context.Wishlists
            //                           .FirstOrDefault(w => w.Recipient_ID == recipientId);

            //    var wishlistViewModel = new WishlistViewModel
            //    {
            //        Books = wishlist?.Books.ToList() ?? new List<Book>()
            //    };

            //    return View(wishlistViewModel);
            //}
            //else
            //{
            //    // For unauthenticated users, get wishlist from session... 
            //    var wishlist = Session["Wishlist"] as Wishlist;
            //    var wishlistViewModel = new WishlistViewModel
            //    {
            //        Books = wishlist?.Books.ToList() ?? new List<Book>()
            //    };
            //    return View(wishlistViewModel);
            //}
        }

        [HttpPost]
        public ActionResult RequestWishlist(WishlistViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Logic to process wishlist request for a logged-in user
                // 1. Get Recipient based on User.Identity.GetUserId()
                // 2. Create Distribution records for the books in the wishlist
                // 3. (Optional) Clear the wishlist 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Redirect to modified registration form
                return RedirectToAction("Register", "Account"); //Assuming AccountController
            }
        }



        //[HttpPost]
        //public ActionResult RequestWishlist()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        string userId = User.Identity.GetUserId();
        //        var recipient = _context.Recipients.FirstOrDefault(r => r.Recipient_ID == userId) ??
        //                            new Recipient { Recipient_ID = userId };
        //        _context.Recipients.Add(recipient);

        //        var wishlist = _context.Wishlists.FirstOrDefault(w => w.Recipient_ID == userId) ??
        //                       new Wishlist { Recipient_ID = userId };
        //        _context.Wishlists.Add(wishlist);

        //        // ... (Distribution Logic - same as before) ...

        //        _context.SaveChanges();
        //        TempData["SuccessMessage"] = "Your wishlist request has been processed!";
        //    }
        //    else
        //    {
        //        var wishlist = Session["Wishlist"] as Wishlist;
        //        if (wishlist != null)
        //        {
        //            TempData["WishlistData"] = wishlist; // Store wishlist for later
        //        }
        //        return RedirectToAction("Register", "Account");
        //    }

        //    return RedirectToAction("Wishlist");
        //}

    }
}