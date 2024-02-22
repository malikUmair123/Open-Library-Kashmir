using CsvHelper;
using Microsoft.AspNetCore.Http;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    public class BooksController : Controller
    {
        [HttpGet]
        [Route("Books/Import")]
        public ActionResult Import()
        {
            BookDonationDBContext _context = new BookDonationDBContext();

            return View(_context);
        }

        // POST: Books/Import
        [HttpPost]
        [Route("Books/Import")]
        //public ActionResult Import(HttpPostedFileBase csvFile)
        //{
        //    if (csvFile != null && csvFile.ContentLength > 0)
        //    {
        //        try
        //        {
        //            // Get the file name
        //            string fileName = Path.GetFileName(csvFile.FileName);

        //            // Specify the path where you want to save the file
        //            string filePath = Path.Combine(Server.MapPath("~/App_Data/"), fileName);

        //            // Save the file
        //            csvFile.SaveAs(filePath);

        //            // Now you can process the CSV file, for example:
        //            // Read the file content
        //            string fileContent = System.IO.File.ReadAllText(filePath);

        //            // Process the CSV file content as needed

        //            // Return a success message or redirect to another action
        //            ViewBag.Message = "CSV file uploaded successfully.";
        //            return View();
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Error = "Error occurred: " + ex.Message;
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Please select a file to upload.";
        //        return View();
        //    }
        //}

        //[HttpPost]
        public async Task<ActionResult> Import(HttpPostedFileBase csvFile)
        {
            if (csvFile == null || csvFile.ContentLength == 0)
            {
                // Handle the error: file not provided
                return View("Error");
            }

            BookDonationDBContext _context = new BookDonationDBContext();
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
            return RedirectToAction("Index", "Home");
        }

        private Book CreateBookFromCsvLine(string[] values)
        {
            // Implement parsing logic here, using indexes to access values[]
            // Example (very simplified)
            return new Book
            {
                book_id = int.Parse(values[0] != null ? values[0] : "99999"),
                title = values[1] != null ? values[1] : "NA",
                author = values[2],
                publisher = values[3],
                @class = values[4],
                subject = values[5],
                status = values[6],
                // ... map other properties
            };
        }

        //public async Task<ActionResult> Import(UploadBooksFromCSVModel model)
        //{


        //    if (model.formFile == null)
        //    {
        //        // Handle the error: file not provided
        //        return View("Error");
        //    }
        //    BookDonationDBContext _context = new BookDonationDBContext();
        //    using (var reader = new StreamReader(model.formFile.OpenReadStream()))
        //    {
        //        string headerLine = reader.ReadLine(); // Optionally skip header
        //        while (!reader.EndOfStream)
        //        {
        //            var line = reader.ReadLine();
        //            var values = line.Split(',');
        //            var newBook = CreateBookFromCsvLine(values); // Helper method
        //            _context.Books.Add(newBook);
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //private Book CreateBookFromCsvLine(string[] values)
        //{
        //    // Implement parsing logic here, using indexes to access values[]
        //    // Example (very simplified)
        //    return new Book
        //    {
        //        title = values[1],
        //        @class = values[2],
        //        subject = values[3],
        //        // ... map other properties
        //    };
        //}
    }
}