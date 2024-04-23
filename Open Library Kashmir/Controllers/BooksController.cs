using CsvHelper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Open_Library_Kashmir.Filters;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

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

        [Route("Books/AddBooks")]
        public ActionResult AddBooks()
        {
            return View();
        }

        // POST: Books/AddBooks
        [HttpPost]
        [Route("Books/AddBooks")]
        public ActionResult AddBooks(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.ContentLength > 0)
                {
                    if (OptimiseAndSaveImage(book))
                    {
                        // Add the book to the database
                        _context.Books.Add(book);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
            }
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
                    if (OptimiseAndSaveImage(book))
                    {
                        // Add the book to the database
                        _context.BookOfTheMonths.Add(book);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }

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
                    if (OptimiseAndSaveImage(book))
                    {
                        _context.BookOfTheMonths.AddOrUpdate(book);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            return View("Error");
        }

        #region Helpers

        [NonAction]
        private Book CreateBookFromCsvLine(string[] values)
        {
            var book = new Book
            {
                BookId = int.Parse(values[0]),
                Title = values[1] != null ? values[1] : "Deleted",
                Author = values[2],
                Publisher = values[3],
                @Class = values[4],
                Subject = values[5],
                //Status = (BookStatus)Enum.Parse(typeof(BookStatus), values[6]),
            };

            // Check if the value should be parsed as enum
            if (Enum.TryParse<BookStatus>(values[6], out var status))
            {
                book.Status = status;
            }
            else
            {
                book.Status = BookStatus.Donated;
            }

            return book;
        }

        public bool OptimiseAndSaveImage(BookOfTheMonth book)
        {
            var supportedTypes = new[] { "jpg", "jpeg", "png", "webp" };
            var fileExt = Path.GetExtension(book.ImageFile.FileName).Substring(1);

            if (!supportedTypes.Contains(fileExt.ToLower()))
            {
                ModelState.AddModelError("ImageFile", "Invalid file type. Only JPG, PNG and WebP images are allowed.");
            }
            else
            {
                var fileName = Guid.NewGuid().ToString() + "." + fileExt;
                var uploadPath = Server.MapPath("~/" + ConfigurationManager.AppSettings["BooksOfTheMonthPath"]); // Adjusted path
                Directory.CreateDirectory(uploadPath); // Create directory if it doesn't exist
                var fullPath = Path.Combine(uploadPath, fileName);

                try
                {
                    // Resize and compress the image before saving
                    using (var image = Image.FromStream(book.ImageFile.InputStream))
                    {
                        // Define the maximum dimensions for the resized image
                        int maxWidth = 500;
                        int maxHeight = 750;

                        // Calculate the new dimensions while maintaining aspect ratio
                        int newWidth, newHeight;
                        if (image.Width > image.Height)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)(((float)image.Height / image.Width) * maxWidth);
                        }
                        else
                        {
                            newWidth = (int)(((float)image.Width / image.Height) * maxHeight);
                            newHeight = maxHeight;
                        }

                        // Create a new bitmap with the new dimensions
                        using (var resizedImage = new Bitmap(newWidth, newHeight))
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            // Draw the original image onto the new bitmap with the new dimensions
                            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

                            if (fileExt.ToLower() == "webp")
                            {
                                resizedImage.Save(fullPath);
                            }
                            else
                            {
                                ImageCodecInfo webpEncoder = GetWebPEncoder();

                                if (webpEncoder != null)
                                {
                                    // Apply WebP encoding
                                    EncoderParameters webpEncoderParams = new EncoderParameters();
                                    webpEncoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 75L); // Adjust quality as needed
                                    resizedImage.Save(fullPath, webpEncoder, webpEncoderParams);
                                }
                                else
                                {
                                    // Apply progressive JPEG encoding
                                    var jpegEncoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                                    var jpegEncoderParams = new EncoderParameters();
                                    jpegEncoderParams.Param[0] = new EncoderParameter(Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                                    resizedImage.Save(fullPath, jpegEncoder, jpegEncoderParams);
                                }
                            }
                        }

                    }

                    book.ImageUrl = "/" + ConfigurationManager.AppSettings["BooksOfTheMonthPath"] + "/" + fileName;
                    return true;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ImageFile", "Error saving file. Try again.");
                }
            }

            return false;

        }

        public bool OptimiseAndSaveImage(Book book)
        {
            var supportedTypes = new[] { "jpg", "jpeg", "png", "webp" };
            var fileExt = Path.GetExtension(book.ImageFile.FileName).Substring(1);

            if (!supportedTypes.Contains(fileExt.ToLower()))
            {
                ModelState.AddModelError("ImageFile", "Invalid file type. Only JPG, PNG and WebP images are allowed.");
            }
            else
            {
                var fileName = Guid.NewGuid().ToString() + "." + fileExt;
                var uploadPath = Server.MapPath(ConfigurationManager.AppSettings["BooksPath"]); // Adjusted path
                Directory.CreateDirectory(uploadPath); // Create directory if it doesn't exist
                var path = Path.Combine(uploadPath, fileName);

                try
                {
                    // Resize and compress the image before saving
                    using (var image = Image.FromStream(book.ImageFile.InputStream))
                    {
                        // Define the maximum dimensions for the resized image
                        int maxWidth = 500;
                        int maxHeight = 750;

                        // Calculate the new dimensions while maintaining aspect ratio
                        int newWidth, newHeight;
                        if (image.Width > image.Height)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)(((float)image.Height / image.Width) * maxWidth);
                        }
                        else
                        {
                            newWidth = (int)(((float)image.Width / image.Height) * maxHeight);
                            newHeight = maxHeight;
                        }

                        // Create a new bitmap with the new dimensions
                        using (var resizedImage = new Bitmap(newWidth, newHeight))
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            // Draw the original image onto the new bitmap with the new dimensions
                            graphics.DrawImage(image, 0, 0, newWidth, newHeight);


                            ImageCodecInfo webpEncoder = GetWebPEncoder();
                            if (webpEncoder != null)
                            {
                                // Apply WebP encoding
                                EncoderParameters webpEncoderParams = new EncoderParameters();
                                webpEncoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 75L); // Adjust quality as needed
                                image.Save(path, webpEncoder, webpEncoderParams);
                            }
                            else
                            {
                                // Apply progressive JPEG encoding
                                var jpegEncoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                                var jpegEncoderParams = new EncoderParameters();
                                jpegEncoderParams.Param[0] = new EncoderParameter(Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                                resizedImage.Save(path, jpegEncoder, jpegEncoderParams);
                            }
                        }

                    }

                    book.ImageUrl = ConfigurationManager.AppSettings["BooksUrl"] + fileName;
                    return true;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ImageFile", "Error saving file. Try again.");
                }
            }

            return false;

        }


        private ImageCodecInfo GetWebPEncoder()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.FormatDescription.Equals("WebP", StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}