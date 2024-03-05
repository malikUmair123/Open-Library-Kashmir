using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;
using static Open_Library_Kashmir.Controllers.ManageController;

namespace Open_Library_Kashmir.Controllers
{
    [RequireHttps]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "NullReference")]
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public DonationController()
        {
            _context = new ApplicationDbContext();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));

        }

        // GET: Donation

        //[OutputCache(CacheProfile = "1MinuteCache", Location = System.Web.UI.OutputCacheLocation.Client)]
        //[OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        // GET: BookDetails

        //[OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult BookDetails(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.BookId == id);
            // Retrieve wishlist from the session
            var wishlist = Session["Wishlist"] as SessionWishlist;
            if (wishlist == null)
            {
                wishlist = new SessionWishlist();
                Session["Wishlist"] = wishlist;
            }

            // Set TempData with the result of your wishlist check
            TempData["BookInWishlist"] = wishlist.BookIds.Contains(id);

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
                Books = _context.Books.Where(b => wishlist.BookIds.Contains(b.BookId)).ToList()
            };
            if (User.Identity.IsAuthenticated)
            {
                Session["returnToRequestWishlist"] = false;
            }
            else
            {
            }
            Session["returnToRequestWishlist"] = true;
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

        //[HttpPost]
        //public ActionResult RequestWishlist(WishlistViewModel model)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // Logic to process wishlist request for a logged-in user
        //        // 1. Get Recipient based on User.Identity.GetUserId()
        //        // 2. Create Distribution records for the books in the wishlist
        //        // 3. (Optional) Clear the wishlist 
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        Session["returnToRequestWishlist"] = true;
        //        // Redirect to modified registration form
        //        return RedirectToAction("Register", "Account"); //Assuming AccountController
        //    }
        //}



        //[HttpPost]
        //public ActionResult RequestWishlist(WishlistViewModel wishlistViewModel)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        string userId = User.Identity.GetUserId();
        //        ApplicationUser user = UserManager.FindById(userId);

        //        if (user != null)
        //        {
        //            RecipientViewModel recipientViewModel = new RecipientViewModel()
        //            {
        //                Email = user.Email,
        //                First_Name = user.FirstName,
        //                Last_Name = user.LastName,
        //                Phone = user.PhoneNumber
        //            };

        //            return RedirectToAction("CreateRecipient", routeValues: new { recipientViewModel = recipientViewModel });
        //        }
        //        //see if you can use following
        //        var recipient = _context.Recipients.FirstOrDefault(r => r.Recipient_ID == userId) ??
        //                            CreateBasicRecipient(user);
        //        _context.Recipients.Add(recipient);
        //        _context.SaveChanges();

        //        var wishlistSesssion = Session["Wishlist"] as SessionWishlist;

        //        var wishlistDatabase = _context.Wishlists.FirstOrDefault(w => w.Recipient_ID == userId) ??
        //                       new Wishlist { Recipient_ID = userId };
        //        wishlistDatabase.Books = wishlistViewModel.Books;
        //        wishlistDatabase.Recipient = recipient;
        //        _context.Wishlists.Add(wishlistDatabase);

        //        // ... (Distribution Logic - same as before) ...

        //        if (wishlistSesssion != null)
        //        {
        //            foreach (var bookId in wishlistSesssion.BookIds)
        //            {
        //                var distribution = new Distribution
        //                {
        //                    Book_ID = bookId,
        //                    Recipient_ID = recipient.Recipient_ID,
        //                    Date_Distributed = DateTime.Now
        //                };
        //                _context.Distributions.Add(distribution);
        //            }

        //            _context.SaveChanges();
        //            TempData["SuccessMessage"] = "Your wishlist request has been processed!";
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Wishlist not found for current user.";
        //        }
        //        return RedirectToAction("Register", "Account");
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

        //}

        public ActionResult EditRecipient()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = UserManager.FindById(userId);

                if (user != null)
                {
                    RecipientViewModel recipientViewModel = new RecipientViewModel()
                    {
                        Email = user.Email,
                        First_Name = user.FirstName,
                        Last_Name = user.LastName,
                        Phone = user.PhoneNumber
                    };

                    return View(recipientViewModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        //[HttpPost]
        //public ActionResult EditRecipient(RecipientViewModel recipientViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            string userId = User.Identity.GetUserId();
        //            ApplicationUser user = UserManager.FindById(userId);

        //            //see if you can use following
        //            var recipient = CreateRecipient(recipientViewModel, userId);

        //            if (recipient != null)
        //            {
        //                _context.Recipients.AddOrUpdate(recipient);
        //            }

        //            var wishlistSesssion = Session["Wishlist"] as SessionWishlist;

        //            if (wishlistSesssion != null)
        //            {
        //                // Fetch books from wishlist
        //                var wishlistViewModel = new WishlistViewModel
        //                {
        //                    Books = _context.Books.Where(b => wishlistSesssion.BookIds.Contains(b.Book_ID)).ToList()
        //                };

        //                var wishlistDatabase = _context.Wishlists.FirstOrDefault(w => w.RecipientId == userId) ??
        //                               new SessionWishlist { Recipient_ID = userId };
        //                wishlistDatabase.Books = wishlistViewModel.Books;
        //                wishlistDatabase.Recipient = recipient;
        //                _context.Wishlists.AddOrUpdate(wishlistDatabase);


        //                // ... (Distribution Logic
        //                foreach (var bookId in wishlistSesssion.BookIds)
        //                {
        //                    var distribution = new Distribution
        //                    {
        //                        Book_ID = bookId,
        //                        Recipient_ID = recipient.Recipient_ID,
        //                        Date_Distributed = DateTime.Now
        //                    };
        //                    _context.Distributions.Add(distribution);
        //                }

        //                _context.SaveChanges();

        //                //TempData["SuccessMessage"] = "Your wishlist request has been processed!";
        //                return RedirectToAction("SummaryPage", new { Message = DonationMessageId.SuccessMessage });

        //            }
        //            else
        //            {
        //                //TempData["ErrorMessage"] = "Wishlist not found for current user.";
        //                return RedirectToAction("SummaryPage", new { Message = DonationMessageId.ErrorMessage });

        //            }
        //        }
        //        else
        //        {
        //            //Implement later

        //            //var wishlist = Session["Wishlist"] as Wishlist;
        //            //if (wishlist != null)
        //            //{
        //            //    TempData["WishlistData"] = wishlist; // Store wishlist for later
        //            //}
        //            //return RedirectToAction("Index", "Home");
        //            return RedirectToAction("SummaryPage", new { Message = DonationMessageId.NotImplemented });

        //        }

        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        //[NonAction]
        //private Recipient CreateRecipient(RecipientViewModel recipientViewModel, string userId)
        //{
        //    var recipient = _context.Recipients.FirstOrDefault(r => r.Recipient_ID == userId) ??
        //                                 new Recipient { Recipient_ID = userId };
        //    if (recipientViewModel != null)
        //    {
        //        recipient.First_Name = recipientViewModel.First_Name;
        //        recipient.Last_Name = recipientViewModel.Last_Name;
        //        recipient.Email = recipientViewModel.Email;
        //        recipient.Phone = recipientViewModel.Phone;
        //        recipient.Address = recipientViewModel.Address;
        //        recipient.Aadhar_Card_Path = recipientViewModel.Aadhar_Card_Path;
        //        recipient.Remarks = recipientViewModel.Remarks;
        //    }
        //    return recipient;
        //}

        //public ActionResult SummaryPage(DonationMessageId message)
        //{
        //    ViewBag.StatusMessage =
        //        message == DonationMessageId.SuccessMessage ? "Success"
        //        : message == DonationMessageId.ErrorMessage ? "Error"
        //        : message == DonationMessageId.NotImplemented ? "NotImplemented"
        //        : "";
        //    return View();
        //}
        //#region Helpers
        //public enum DonationMessageId
        //{
        //    SuccessMessage,
        //    ErrorMessage,
        //    NotImplemented,
        //}
        //#endregion

    }
}