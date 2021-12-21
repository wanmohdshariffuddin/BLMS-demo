using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NoPermission()
        {
            return View("NoPermission");
        }
    }
}
