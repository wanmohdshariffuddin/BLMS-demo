using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.SOP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class AuthorityController : Controller
    {
        readonly SOPDBContext dbContext = new SOPDBContext();

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT, Roles.PIC)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ, AccessLevel.SITE)]
        public ActionResult Index()
        {
            List<Authority> authorityList = dbContext.AuthorityGetAll().ToList();

            if (TempData["createMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["createMessage"].ToString());
            }
            else if (TempData["editMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["editMessage"].ToString());
            }
            else if (TempData["deleteMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Delete, TempData["deleteMessage"].ToString());
            }

            return View(authorityList);
        }
        #endregion

        #region CREATE
        //[NoDirectAccess]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessDivController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Authority authority)
        {
            try
            {
                string UserName = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(authority.AuthorityName) && string.IsNullOrEmpty(authority.AuthorityLink))
                {
                    ModelState.AddModelError("", "Please type Authority Name");
                    ModelState.AddModelError("", "Please type Authority Link");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Authority Name", "Please type Authority Link", "", "");
                }
                else if (string.IsNullOrEmpty(authority.AuthorityName))
                {
                    ModelState.AddModelError("", "Please type Authority Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Authority Name");
                }
                else if (string.IsNullOrEmpty(authority.AuthorityLink))
                {
                    ModelState.AddModelError("", "Please type Authority Link");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Authority Link");
                }
                else if (ModelState.IsValid)
                {
                    string AuthorityName = authority.AuthorityName;

                    Authority checkAuthority = dbContext.CheckAuthorityByName(AuthorityName);

                    if (checkAuthority.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", AuthorityName));
                        return View(authority);
                    }
                    else
                    {
                        dbContext.AddAuthority(authority, UserName);
                        TempData["createMessage"] = string.Format("{0} has been successfully created!", AuthorityName);
                        return RedirectToAction("Index");
                    }
                }

                return View(authority);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region EDIT
        //[NoDirectAccess]
        public ActionResult Edit(int id)
        {
            Authority authority = dbContext.GetAuthorityByID(id);

            if (authority == null)
            {
                return NotFound();
            }

            return View(authority);
        }

        // POST: BusinessDivController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] Authority authority)
        {
            try
            {
                string UserName = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(authority.AuthorityName) && string.IsNullOrEmpty(authority.AuthorityLink))
                {
                    ModelState.AddModelError("", "Please type Authority Name");
                    ModelState.AddModelError("", "Please type Authority Link");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Authority Name", "Please type Authority Link", "", "");
                }
                else if (string.IsNullOrEmpty(authority.AuthorityName))
                {
                    ModelState.AddModelError("", "Please type Authority Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Authority Name");
                }
                else if (string.IsNullOrEmpty(authority.AuthorityLink))
                {
                    ModelState.AddModelError("", "Please type Authority Link");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Authority Link");
                }
                else if (ModelState.IsValid)
                {
                    Authority checkAuthority= dbContext.CheckAuthorityByName(authority.AuthorityName);

                    if (checkAuthority.ExistData == 1 && authority.AuthorityName == authority.OldAuthorityName && authority.AuthorityLink == authority.OldAuthorityLink)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (checkAuthority.ExistData == 1 && authority.AuthorityName != authority.OldAuthorityName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", authority.AuthorityName));
                        return View(authority);
                    }
                    if (checkAuthority.ExistData == 1 && (authority.AuthorityName != authority.OldAuthorityName | authority.AuthorityLink == authority.OldAuthorityLink))
                    {
                        dbContext.EditAuthority(authority, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", authority.AuthorityName);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        dbContext.EditAuthority(authority, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", authority.AuthorityName);
                        return RedirectToAction("Index");
                    }
                }

                return View(dbContext);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        public JsonResult Delete(int Id)
        {

            try
            {
                Authority authority = dbContext.GetAuthorityByID(Id);

                dbContext.DeleteAuthority(Id);
                TempData["deleteMessage"] = string.Format("{0} has been deleted from BLMS database!", authority.AuthorityName);

                return Json(new { status = "Success" });
            }
            catch
            {
                return Json(new { status = "Fail" });
            }
        }
        #endregion
    }
}
