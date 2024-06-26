﻿using Open_Library_Kashmir.Models;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogCustomExceptionFilter());
        }
    }
}
