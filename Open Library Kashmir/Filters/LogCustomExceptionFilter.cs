using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Models
{
    public interface ILog
    {
        void LogException(ExceptionContext filterContext);
    }

    public sealed class Log : ILog
    {
        //Private Constructor to Restrict Class Instantiation from outside the Log class
        private Log()
        {
        }
        //Creating Log Instance using Eager Loading
        private static readonly Log LogInstance = new Log();
        //Returning the Singleton LogInstance
        //This Method is Thread Safe as it uses Eager Loading
        public static Log GetInstance()
        {
            return LogInstance;
        }
        //This Method Log the Exception Details in a Log File
        public void LogException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is ValidationException validationException)
            {
                // Define the path to your log file
                string logFilePath = HttpContext.Current.Server.MapPath("~/Log/LogExceptions.txt");

                try
                {
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

                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur while writing to the log file
                    Console.WriteLine($"Error creating log file: {ex.Message}");
                }

                // Log validation errors
                foreach (var modelStateEntry in filterContext.Controller.ViewData.ModelState.Values)
                {

                    foreach (var error in modelStateEntry.Errors)
                    {
                        // Capture the error message
                        var errorMessage = error.ErrorMessage;

                        //// Log the validation error message
                        //LogErrorMessage(errorMessage);
                        try
                        {
                            //Append text to the log file
                            File.AppendAllText(logFilePath, errorMessage);
                        }
                        catch
                        {
                            // e any exceptions that occur while writing to the log file
                            Console.WriteLine($"Error writing to log file: {errorMessage}");
                        }
                    }
                }
            }
            else
            {
                var exceptionMessage = filterContext.Exception.Message;
                var stackTrace = filterContext.Exception.StackTrace;
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();

                string message = $"Date: {DateTime.Now}" + Environment.NewLine +
                 $"Controller: {controllerName}" + Environment.NewLine +
                 $"Action: {actionName}" + Environment.NewLine +
                 $"Error Message: {exceptionMessage}" + Environment.NewLine +
                 $"Stack Trace: {stackTrace}" + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                // Define the path to your log file
                string logFilePath = HttpContext.Current.Server.MapPath("~/Log/LogExceptions.txt");

                try
                {
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
                    File.AppendAllText(logFilePath, message);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur while writing to the log file
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }

        }
    }
    public class LogCustomExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                ILog _ILog = Log.GetInstance();
                _ILog.LogException(filterContext);

               filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error"
                };
            }
        }
    }
}