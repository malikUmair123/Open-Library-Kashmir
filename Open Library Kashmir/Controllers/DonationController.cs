﻿using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Open_Library_Kashmir.Models;
using PagedList;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;
using Twilio.TwiML.Messaging;
using static Open_Library_Kashmir.Controllers.ManageController;

namespace Open_Library_Kashmir.Controllers
{
    [RequireHttps]
    [LogCustomExceptionFilter]
    //HandleError(ExceptionType = typeof(NullReferenceException), View = "NullReference")]
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        private IMapper _mapper;

        public DonationController()
        {
            _context = new ApplicationDbContext();
        }
        public DonationController(IMapper mapper, ApplicationDbContext context, ApplicationUserManager userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            //_context = new ApplicationDbContext();
        }

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


        // GET: Donation

        //[OutputCache(CacheProfile = "1MinuteCache", Location = System.Web.UI.OutputCacheLocation.Client)]
        //[OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        //public ActionResult Index()
        //{
        //    var books = _context.Books.ToList();
        //    return View(books);
        //}

        public ActionResult Index(string searchString, int? page)
        {
            var books = from b in _context.Books select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString)
                                         || b.Author.Contains(searchString));
            }

            // Add Ordering here
            books = books.OrderBy(b => b.Title); // Order by Title

            int pageSize = 20;  // Display 3 items per page
            int pageNumber = (page ?? 1); // Default to page 1

            return View(books.ToPagedList(pageNumber, pageSize));
        }



        // GET: BookDetails

        //[OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult BookDetails(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.BookId == id);

            //Books in request pipeline
       
            TempData["BooksInWishlistDB"] = WishlistInDB() != null;
          

            // Retrieve wishlist from the session
            var wishlist = Session["Wishlist"] as Wishlist;
            if (wishlist == null)
            {
                wishlist = new Wishlist();
                Session["Wishlist"] = wishlist;
            }

            // Set TempData with the result of your wishlist session
            // or if wishlist already exists in database
            // don't allow the user to add to wishlist
            // updates properties of Add To Wishlist Button to disables
            TempData["BookInWishlist"] = wishlist.Books.Any(b => b.BookId == id) /*|| WishlistInDB() != null*/;

            return View(book);
        }

        // Add to Wishlist Action
        [HttpPost]
        public ActionResult AddToWishlist(Book book)
        {
            // Retrieve wishlist from the session
            var wishlist = Session["Wishlist"] as Wishlist;
            if (wishlist == null)
            {
                wishlist = new Wishlist();
                Session["Wishlist"] = wishlist;
            }

            // If the book isn't present
            if (!wishlist.Books.Any(b => b.BookId == book.BookId))
            {
                wishlist.Books.Add(book);
                //_context.SaveChanges(); // Assuming you want to persist in an actual wishlist later
            }
            return RedirectToAction("BookDetails", new { id = book.BookId });
        }

        public JsonResult GetWishlistCount()
        {
            var wishlist = Session["Wishlist"] as Wishlist;
            int count = wishlist != null ? wishlist.Books.Count : 0;
            return Json(new { Count = count }, JsonRequestBehavior.AllowGet);
        }


        // Wishlist View 
        public ActionResult Wishlist()
        {
            var wishlist = Session["Wishlist"] as Wishlist;

            if (wishlist == null)
            {
                return View(new WishlistViewModel());
            }

            // Fetch books from wishlist
            var wishlistViewModel = new WishlistViewModel
            {
                Books = wishlist.Books.ToList()
            };
            //if (User.Identity.IsAuthenticated)
            //{
            //    Session["returnToRequestWishlist"] = false;
            //}
            //else
            //{
            //}
            //Session["returnToRequestWishlist"] = true;
            
            return View(wishlistViewModel);

        }

        public ActionResult EditRecipient()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = UserManager.FindById(userId);
                if (user?.Address == null)
                { 
                    user.Address = _context.Addresses.Create();
                }
                // Create a new MapperConfiguration
                var config = new MapperConfiguration(cfg =>
                {
                    // Configure mappings here
                    cfg.CreateMap<ApplicationUser, RecipientViewModel>()
                            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                            .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
                });

                // Create a new IMapper instance from the MapperConfiguration
                _mapper = config.CreateMapper();

                if (user != null)
                {
                    RecipientViewModel recipientViewModel = _mapper.Map<ApplicationUser, RecipientViewModel>(user);

                    ////RecipientViewModel recipientViewModel = new RecipientViewModel()
                    ////{
                    ////    Email = user.Email,
                    ////    First_Name = user.FirstName,
                    ////    Last_Name = user.LastName,
                    ////    Phone = user.PhoneNumber
                    ////};

                    return View(recipientViewModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public async Task<ActionResult> EditRecipient(RecipientViewModel recipientViewModel)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string userId = User.Identity.GetUserId();
                    ApplicationUser user = UserManager.FindById(userId);

                    //// Create a new MapperConfiguration
                    //var config = new MapperConfiguration(cfg =>
                    //{
                    //    // Configure mappings here
                    //    cfg.CreateMap<RecipientViewModel, ApplicationUser>()
                    //        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    //        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    //        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    //        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                    //        .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                    //        .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    //        .ForAllOtherMembers(opt => opt.Condition(src => src != null)); 

                    //        //.ForAllOtherMembers(opt => opt.Ignore()); // Ignore existing properties if not provided in the source
                    //        ////.ForAllOtherMembers(opt => opt.UseDestinationValue()); // Use destination value if source value is null
                    //});

                    //// Create a new IMapper instance from the MapperConfiguration
                    //_mapper = config.CreateMapper();

                    //user = _mapper.Map<RecipientViewModel, ApplicationUser>(recipientViewModel);

                    //Mapper doesn't update user but ovverides it, user manual mapping as of now
                    user.FirstName = recipientViewModel.FirstName;
                    user.LastName = recipientViewModel.LastName;
                    user.Email = recipientViewModel.Email;
                    user.PhoneNumber = recipientViewModel.PhoneNumber;
                    user.Remarks = recipientViewModel.Remarks;
                    user.Address = recipientViewModel.Address;

                    //if (user != null)
                    //{
                    //    _context.Users.AddOrUpdate(user);
                    //    _context.SaveChanges();

                    //}

                    // Save changes to the database
                    var result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // Update successful
                    }
                    else
                    {
                        // Handle update failure, possibly by logging errors or displaying error messages to the user
                    }

                    //need to add wishlist to recipeint which first needs to add books
                    var recipient = CreateRecipient(userId);

                    var wishlist = Session["Wishlist"] as Wishlist;

                    if (wishlist != null && recipient != null)
                    {
                        // Set recipient ID in wishlist
                        wishlist.RecipientId = recipient.RecipientId;

                        recipient.RequestStatus = RequestStatus.Pending;
                        // Add wishlist to recipient's collection
                        //recipient.Wishlists.Add(wishlist);
                        wishlist.Recipient = recipient;
                        // Add or update wishlist and recipient in the database
                        _context.Wishlists.AddOrUpdate(wishlist);
                        //_context.Recipients.AddOrUpdate(recipient);

                        // Save changes to the database
                        var finalresult = _context.SaveChanges();

                        if (finalresult > 0)
                        {
                            wishlist = null;
                        }

                    // ... (Distribution Logic for actual distribution...see if you only need wishlist table

                    //foreach (var books in wishlist.Books)
                    //{
                    //    var recipientBook = new RecipientBook
                    //    {
                    //        RecipientId = recipient.RecipientId,
                    //        BookId = books.BookId,
                    //        DateRecieved = DateTime.Now
                    //    };
                    //_context.RecipientBooks.AddOrUpdate(recipientBook);
                    //}

                    //Final save

                    //TempData["SuccessMessage"] = "Your wishlist request has been processed!";
                    return RedirectToAction("RequestSummary", new { Message = DonationMessageId.SuccessMessage });

                    }
                    else
                    {
                        //TempData["ErrorMessage"] = "Wishlist not found for current user.";
                        return RedirectToAction("RequestSummary", new { Message = DonationMessageId.ErrorMessage });

                    }
                }
                else
                {
                    //Implement later

                    //var wishlist = Session["Wishlist"] as Wishlist;
                    //if (wishlist != null)
                    //{
                    //    TempData["WishlistData"] = wishlist; // Store wishlist for later
                    //}
                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction("RequestSummary", new { Message = DonationMessageId.NotImplemented});

                }

            }
            return View("Error");
        }

        [NonAction]
        private Recipient CreateRecipient(string userId)
        {
            var recipient = _context.Recipients.FirstOrDefault(r => r.RecipientId == userId) ??
                                          new Recipient { RecipientId = userId };
            return recipient;
        }

        public ActionResult RequestSummary(DonationMessageId message)
        {
            //////need to add wishlist to recipeint which first needs to add books
            ////        var recipient = CreateRecipient(userId);

            ////        var wishlist = Session["Wishlist"] as Wishlist;

            ////        if (wishlist != null && recipient != null)
            ////        {
            ////            // Set recipient ID in wishlist
            ////            wishlist.RecipientId = recipient.RecipientId;

            ////            recipient.RequestStatus = RequestStatus.Pending;
            ////            // Add wishlist to recipient's collection
            ////            //recipient.Wishlists.Add(wishlist);
            ////            wishlist.Recipient = recipient;
            ////            // Add or update wishlist and recipient in the database
            ////            _context.Wishlists.AddOrUpdate(wishlist);
            ////            //_context.Recipients.AddOrUpdate(recipient);

            ////            // Save changes to the database
            ////            var finalresult = _context.SaveChanges();

            ////            if (finalresult > 0)
            ////            {
            ////                wishlist = null;
            ////            }

            ////        // ... (Distribution Logic for actual distribution...see if you only need wishlist table

            ////        //foreach (var books in wishlist.Books)
            ////        //{
            ////        //    var recipientBook = new RecipientBook
            ////        //    {
            ////        //        RecipientId = recipient.RecipientId,
            ////        //        BookId = books.BookId,
            ////        //        DateRecieved = DateTime.Now
            ////        //    };
            ////        //_context.RecipientBooks.AddOrUpdate(recipientBook);
            ////        //}

            ////        //Final save

            ////        //TempData["SuccessMessage"] = "Your wishlist request has been processed!";
            ////        return RedirectToAction("RequestSummary", new { Message = DonationMessageId.SuccessMessage });

            ////        }
            ////        else
            ////        {
            ////            //TempData["ErrorMessage"] = "Wishlist not found for current user.";
            ////            return RedirectToAction("RequestSummary", new { Message = DonationMessageId.ErrorMessage });

            ////        }
            ///
            //if (message == DonationMessageId.SuccessMessage)
            //{
            //    // Send an email with this link
            //    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //    string emailBody = @"
            //            <p>Dear " + model.FirstName + @",</p>
            //            <p>Thank you for registering with Open Library Kashmir (OLK)!</p>
            //            <p>To complete the registration process and access your account, please confirm your email address by clicking the link below:</p>
            //            <p><a href=""" + callbackUrl + @""">Confirm Email</a></p>
            //            <p>If you did not register with OLK or believe you received this email in error, please disregard it.</p>
            //            <p>Thank you,</p>
            //            <p>Open Library Kashmir (OLK) Team</p>";

            //    //SMS Test

            //    //await UserManager.SendSmsAsync(user.PhoneNumber, callbackUrl);

            //    //SendGrid Implementation...see IdentityConfig.cs EmailService

            //    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //    //Smtp...Gmail Implementation
            //    bool IsSendEmail = Helpers.Helpers.EmailSend(model.Email, "Confirm Email", emailBody, true);

            //    if (IsSendEmail)
            //    {
            //        return RedirectToAction("ConfirmEmailConfirmation");
            //    }

                ViewBag.StatusMessage =
                message == DonationMessageId.SuccessMessage ? "Success"
                : message == DonationMessageId.GiftBooksSuccess ? "Your request has been recieved, We will contact on on your given contact details"
                : message == DonationMessageId.ErrorMessage ? "Error"
                : message == DonationMessageId.NotImplemented ? "NotImplemented"
                : "";

            return View();
        }

        [ActionName("RequestStatus")]
        public ActionResult RequestStatusPage()
        {
            if (Request.IsAuthenticated)
            {
                    
                    var wishlist = WishlistInDB();

                    if (wishlist != null) 
                    {
                         var BooksInWishlist = wishlist.Books.ToList();
                         ViewBag.StatusMessage =
                               wishlist?.Recipient?.RequestStatus == RequestStatus.Pending ? "Pending...Wait for us to contact you."
                             : wishlist?.Recipient?.RequestStatus == RequestStatus.Approved ? "Approved...You will be informed about the order soon."
                             : wishlist?.Recipient?.RequestStatus == RequestStatus.Rejected ? "Rejected...Kindly check with us to know why."
                             : "";
                         return View(BooksInWishlist);
                    }

            }

            ViewBag.StatusMessage = "No books requested yet.";

            return View();

        }

        public ActionResult GiftBooks()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                ApplicationUser user = UserManager.FindById(userId);
                if (user?.Address == null)
                {
                    user.Address = _context.Addresses.Create();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }

        [HttpPost]
        public async Task<ActionResult> GiftBooks(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Save changes to the database
                    var result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("RequestSummary", new { Message = DonationMessageId.GiftBooksSuccess });
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
                    return View();
        }
        public Wishlist WishlistInDB()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                Recipient recipient = _context.Recipients.FirstOrDefault(r => r.RecipientId == userId);

                if (recipient != null)
                {
                    return _context.Wishlists.FirstOrDefault(w => w.RecipientId == userId);
                    // Return wishlist directly if found
                }
            }

            return null; // Return null if no wishlist is associated with the user
        }

        #region Helpers
        public enum DonationMessageId
        {
            SuccessMessage,
            GiftBooksSuccess,
            ErrorMessage,
            NotImplemented,
        }
        #endregion

    }
}