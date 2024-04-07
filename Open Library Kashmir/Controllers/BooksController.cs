using CsvHelper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Open_Library_Kashmir.Filters;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    //[CustomAuthenticationFilter]
    //[CustomAuthorizationFilter]
    [LogCustomExceptionFilter]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Books/Import")]
        //[OverrideAuthentication]
        //[OverrideAuthorization]
        [AllowAnonymous]
        public ActionResult Import()
        {
            return View();
        }

        // POST: Books/Import
        [HttpPost]
        [Route("Books/ImportBooksFromCSV")]

        public async Task<ActionResult> ImportBooksFromCSV(HttpPostedFileBase csvFile)
        {
            if (csvFile == null || csvFile.ContentLength == 0)
            {
                // Handle the error: file not provided
                return View("Error");
            }

            using (var reader = new StreamReader(csvFile.InputStream))
            {
                string headerLine = reader.ReadLine(); // Optionally skip header
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var newBook = CreateBookFromCsvLine(values); // Helper method
                    _context.Books.Add(newBook);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Donation");
        }

        [NonAction]
        private Book CreateBookFromCsvLine(string[] values)
        {
            return new Book
            {
                BookId = int.Parse(values[0] != null ? values[0] : "99999"),
                Title = values[1] != null ? values[1] : "Deleted",
                Author = values[2],
                Publisher = values[3],
                @Class = values[4],
                Subject = values[5],
                Status = values[6],
            };
        }

        // POST: Books/Import
        [HttpPost]
        [Route("Books/AddBooksThroughForms")]
        public ActionResult AddBooksThroughForms(Book book)
        {
            if (ModelState.IsValid)
            {
                // Add the book to the database
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            // Handle invalid model state
            return View("Error");
        }

        public ActionResult GetBooksOfTheMonth()
        {
            return View(_context.BookOfTheMonths.ToList());
        }
        public ActionResult AddBookOfTheMonth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBookOfTheMonth(BookOfTheMonth book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.ContentLength > 0)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png" };
                    var fileExt = Path.GetExtension(book.ImageFile.FileName).Substring(1);

                    if (!supportedTypes.Contains(fileExt.ToLower()))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file type. Only JPG and PNG are allowed.");
                    }
                    else
                    {
                        var fileName = Guid.NewGuid().ToString() + "." + fileExt;
                        var uploadPath = Server.MapPath("~/Content/Images/BookOfTheMonth"); // Adjusted path
                        Directory.CreateDirectory(uploadPath); // Create directory if it doesn't exist
                        var path = Path.Combine(uploadPath, fileName);

                        try
                        {
                            book.ImageFile.SaveAs(path);
                            book.ImageUrl = "/Content/Images/BookOfTheMonth/" + fileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ImageFile", "Error saving file. Try again.");
                        }
                    }
                    // Add the book to the database
                    _context.BookOfTheMonths.Add(book);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Error");
        }

        public ActionResult EditBookOfTheMonth(int id)
        {
           return View(_context.BookOfTheMonths.FirstOrDefault(book => book.BookId == id));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBookOfTheMonth(BookOfTheMonth book)
        {

            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.ContentLength > 0)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png" };
                    var fileExt = Path.GetExtension(book.ImageFile.FileName).Substring(1);

                    if (!supportedTypes.Contains(fileExt.ToLower()))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file type. Only JPG and PNG are allowed.");
                    }
                    else
                    {
                        var fileName = Guid.NewGuid().ToString() + "." + fileExt;
                        var uploadPath = Server.MapPath("~/Content/Images/BookOfTheMonth"); // Adjusted path
                        Directory.CreateDirectory(uploadPath); // Create directory if it doesn't exist
                        var path = Path.Combine(uploadPath, fileName);

                        try
                        {
                            book.ImageFile.SaveAs(path);
                            book.ImageUrl = "/Content/Images/BookOfTheMonth/" + fileName;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("ImageFile", "Error saving file. Try again.");
                        }
                    }

                    _context.BookOfTheMonths.AddOrUpdate(book);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View("Error");
        }

    }
}