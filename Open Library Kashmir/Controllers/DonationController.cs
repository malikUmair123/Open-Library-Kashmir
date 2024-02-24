using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Open_Library_Kashmir.Controllers
{
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
        public ActionResult Donation()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [Route("BookDetails/{id}")]
        public ActionResult BookDetails(int id)
        {
            Book book = _context.Books.FirstOrDefault(x => x.Book_ID == id);
            return View(book);
        }
    }
}