using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context ?? new ApplicationDbContext();
        }

        [OutputCache(CacheProfile = "1MinuteCache", Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var todayYear = DateTime.Today.Year;
            var todayMonth = DateTime.Today.Month;

            var bookOfTheMonth = _context.BookOfTheMonths
                .FirstOrDefault(book => book.MonthYear.Year == todayYear && book.MonthYear.Month == todayMonth)
                ?? defaultBookOfTheMonth();

            return View(bookOfTheMonth);
        }

        #region Helpers

        BookOfTheMonth defaultBookOfTheMonth()
        {
            return new BookOfTheMonth
            {
                Title = "Man’s Search for Meaning",
                Author = "Viktor Frankl",
                MonthYear = DateTime.Now,
                ImageUrl = "https://cdn.penguin.co.uk/dam-assets/books/9781846046384/9781846046384-jacket-large.jpg",
                ShortDescription = "Psychiatrist Viktor Frankl's memoir has riveted generations of readers with its descriptions of life in Nazi death camps and its lessons for spiritual survival. Based on his own experience and the stories of his patients, Frankl argues that we cannot avoid suffering but we can choose how to cope with it, find meaning in it, and move forward with renewed purpose. At the heart of his theory, known as logotherapy, is a conviction that the primary human drive is not pleasure but the pursuit of what we find meaningful. Man's Search for Meaning has become one of the most influential books in America; it continues to inspire us all to find significance in the very act of living."
            };
        }

        #endregion
    }
}