using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Models
{
    public class LogCustomExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var exceptionMessage = filterContext.Exception.Message;
                var stackTrace = filterContext.Exception.StackTrace;
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();

                string Message = "Date :" + DateTime.Now.ToString() + ", Controller: " + controllerName + ", Action:" + actionName +
                                 "Error Message : " + exceptionMessage
                                + Environment.NewLine + "Stack Trace : " + stackTrace;

                //saving the data in a text file called Log.txt
                // Path to the log file
                string logFilePath = HttpContext.Current.Server.MapPath("~/Log/LogExceptions.txt");

                // Check if the file exists
                if (!File.Exists(logFilePath))
                {
                    // If the file doesn't exist, create it
                    using (FileStream fs = File.Create(logFilePath))
                    {
                        // File created, close the stream
                        fs.Close();
                    }
                }

                // Append text to the log file
                File.AppendAllText(logFilePath, Message);

               //this doesn't check if the file exists
               //File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/LogExceptions.txt"), Message);

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error"
                };
            }
        }
    }
}