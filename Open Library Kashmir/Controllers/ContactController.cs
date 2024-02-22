using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact

        [Route("Contact")]

        public ActionResult Contact()
        {
            return View();
        }
    }
}