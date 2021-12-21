using BLMS.Context;
using BLMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Controllers
{
    public class LogController : Controller
    {
        readonly LogDBContext dbContext = new LogDBContext();

        public IActionResult Index()
        {

            AuditErrorViewModel aevm = new AuditErrorViewModel();

            aevm.auditLog = dbContext.GetAuditLog().ToList();
            aevm.errorLog = dbContext.GetErrorLog().ToList();

            return View(aevm);
        }
    }
}
