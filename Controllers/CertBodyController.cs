using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class CertBodyController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        public IActionResult Index()
        {
            List<CertBody> CertbodyList = dbContext.CertBodyGetAll().ToList();

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

            return View(CertbodyList);
        }
        #endregion

        #region CREATE
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessDivController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] CertBody certBody)
        {
            try
            {
                string UserName = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(certBody.CertBodyName))
                {
                    ModelState.AddModelError("", "Please type Certificate Body");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Certificate Body");
                }
                else if (ModelState.IsValid)
                {
                    CertBody checkCertBody = dbContext.CheckCertBodyByName(certBody.CertBodyName);

                    if (checkCertBody.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", certBody.CertBodyName));
                        return View(certBody);
                    }
                    else
                    {
                        dbContext.AddCertBody(certBody, UserName);
                        TempData["createMessage"] = string.Format("{0} has been successfully created!", certBody.CertBodyName);
                        return RedirectToAction("Index");
                    }
                }

                return View(certBody);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region EDIT
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult Edit(int id)
        {
            CertBody certBody = dbContext.GetCertBodyByID(id);

            if (certBody == null)
            {
                return NotFound();
            }

            return View(certBody);
        }

        // POST: BusinessDivController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] CertBody certBody)
        {
            try
            {
                string UserName = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(certBody.CertBodyName))
                {
                    ModelState.AddModelError("", "Please type Certificate Body");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Certificate Body");
                }
                else if (ModelState.IsValid)
                {
                    CertBody checkCertBody = dbContext.CheckCertBodyByName(certBody.CertBodyName);

                    if (checkCertBody.ExistData == 1 && certBody.CertBodyName != certBody.OldCertBodyName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", certBody.CertBodyName));
                        return View(certBody);
                    }
                    else if (checkCertBody.ExistData == 1 && certBody.CertBodyName == certBody.OldCertBodyName)
                    {
                        dbContext.EditCertBody(certBody, UserName);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        dbContext.EditCertBody(certBody, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", certBody.CertBodyName);
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
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        public JsonResult Delete(int Id)
        {

            try
            {
                CertBody certBody = dbContext.GetCertBodyByID(Id);

                dbContext.DeleteCertBody(Id);
                TempData["deleteMessage"] = string.Format("{0} has been deleted from BLMS database!", certBody.CertBodyName);

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
