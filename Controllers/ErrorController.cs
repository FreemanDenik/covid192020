using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Controllers
{
    public class ErrorController : Controller
    {
        
        public ActionResult Index(int code)
        {
            switch(code)
            {
                case 404:
                    return View("404");

            }
            return View();
        }

    }
}
