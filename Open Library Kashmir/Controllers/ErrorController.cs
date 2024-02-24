using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    // GET: Error
    public class ErrorController : Controller
    {
        public ActionResult PageNotFoundError()
        {
            return View();
        }

        public ActionResult UnauthorizedError()
        {
            return View();
        }

        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult GenericError()
        {
            return View();
        }
    }

}