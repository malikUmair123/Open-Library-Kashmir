﻿using CsvHelper;
using Microsoft.AspNetCore.Http;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookDonationDBContext _context;

        public BooksController()
        {
            _context = new BookDonationDBContext();
        }

        // GET: Books/Import
        [HttpGet]
        [Route("Books/Import")]
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
            return RedirectToAction("Index", "Home");
        }

        private Book CreateBookFromCsvLine(string[] values)
        {
            return new Book
            {
                Book_ID = int.Parse(values[0] != null ? values[0] : "99999"),
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


    }
}