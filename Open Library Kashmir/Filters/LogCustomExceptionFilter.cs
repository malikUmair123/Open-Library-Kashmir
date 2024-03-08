﻿using System;
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

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error"
                };
            }
        }
    }
}