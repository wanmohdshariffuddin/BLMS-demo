using BLMS.Context;
using BLMS.Models;
using BLMS.Models.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLMS.Controllers
{
    public class HomeController : Controller
    {
        readonly LoginDBContext loginDBContext = new LoginDBContext();

        public IActionResult Index()
        {
            User objLoggedInUser = new User();
            if (User.Identity.IsAuthenticated)
            {
                var claimsIndentity = HttpContext.User.Identity as ClaimsIdentity;
                var userClaims = claimsIndentity.Claims;

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    foreach (var claim in userClaims)
                    {
                        var cType = claim.Type;
                        var cValue = claim.Value;
                        switch (cType)
                        {
                            case "STAFF_EMAIL":
                                objLoggedInUser.STAFF_EMAIL = cValue;
                                break;
                            case "STAFF_NAME":
                                objLoggedInUser.STAFF_NAME = cValue;
                                break;
                            case "STAFF_NO":
                                objLoggedInUser.STAFF_NO = cValue;
                                break;

                            case "ADMINISTRATION":
                                objLoggedInUser.ACCESS_LEVEL = cValue;
                                break;
                            case "HQ":
                                objLoggedInUser.ACCESS_LEVEL = cValue;
                                break;
                            case "SITE":
                                objLoggedInUser.ACCESS_LEVEL = cValue;
                                break;

                            case "ADMINISTRATOR":
                                objLoggedInUser.ROLE = cValue;
                                break;
                            case "BUSINESS UNIT":
                                objLoggedInUser.ROLE = cValue;
                                break;
                            case "PIC":
                                objLoggedInUser.ROLE = cValue;
                                break;
                        }
                    }
                }
            }
            return View("Index", objLoggedInUser);
        }

        public IActionResult LoginUser(User user)
        {
            user.STAFF_EMAIL = HttpContext.Request.Query["EMAIL"];
            user.PASSWORD = "BLMS";

            User getUser = loginDBContext.GetUserByEmail(user.STAFF_EMAIL);

            if (string.IsNullOrEmpty(user.STAFF_EMAIL))
            {
                return Redirect("~/Error/NoAuthentication");
            }
            else if (string.IsNullOrEmpty(getUser.ROLE))
            {
                return Redirect("~/AllUser/Index");
            }
            else
            {
                TokenProvider _tokenProvider = new TokenProvider();
                var userToken = _tokenProvider.LoginUser(user.STAFF_EMAIL.Trim(), user.PASSWORD.Trim());

                if (userToken != null)
                {
                    //Save token in session object
                    HttpContext.Session.SetString("JWToken", userToken);
                }

                return Redirect("~/Dashboard/Index");
            }
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Home/Index");
        }
    }
}
