using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    public class AboutController : Controller
    {
        // GET: About

        //[Route("About")]

        public ActionResult Index()
        {
            return View();
        }
    }
}