using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Open_Library_Kashmir.Controllers
{
    [LogCustomActionFilter]
    [HandleError(ExceptionType = typeof(NullReferenceException), View = "NullReference")]
    public class DonationController : Controller
    {
        private readonly BookDonationDBContext _context;

        public DonationController()
        {
            _context = new BookDonationDBContext();
        }

        // GET: Donation

        [Route("Donation")]
        [OutputCache(CacheProfile = "1MinuteCache", Location = System.Web.UI.OutputCacheLocation.Client)]
        //[OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Donation()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [Route("BookDetails/{id}")]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        public ActionResult BookDetails(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Book_ID == id);
            return View(book);
        }
    }
}