using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class CertBodyController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();
        readonly LogDBContext logDBContext = new LogDBContext();

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
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
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessDivController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] CertBody certBody)
        {
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                #region VALIDATION
                if (string.IsNullOrEmpty(certBody.CertBodyName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Certificate Body");
                }
                #endregion

                else
                {
                    CertBody checkCertBody = dbContext.CheckCertBodyByName(certBody.CertBodyName);

                    #region CHECK DUPLICATION
                    if (checkCertBody.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", certBody.CertBodyName));
                    }
                    #endregion

                    else
                    {
                        dbContext.AddCertBody(certBody, UserName);
                        TempData["createMessage"] = string.Format("{0} has been successfully created!", certBody.CertBodyName);
                        return RedirectToAction("Index");
                    }
                }

                return View(certBody);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "CERTIFICATE BODY";

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

                string msg = ex.Message;
                string method = trace.GetFrame((trace.FrameCount - 1)).GetMethod().ToString();
                Int32 lineNumber = trace.GetFrame((trace.FrameCount - 1)).GetFileLineNumber();

                logDBContext.AddErrorLog(path, method, lineNumber, msg, UserName);
                #endregion

                return View();
            }
        }
        #endregion

        #region EDIT
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
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
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                #region VALIDATION
                if (string.IsNullOrEmpty(certBody.CertBodyName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Certificate Body");
                }
                #endregion

                else
                {
                    CertBody checkCertBody = dbContext.CheckCertBodyByName(certBody.CertBodyName);

                    #region CHECK DUPLICATION
                    if (checkCertBody.ExistData == 1 && certBody.CertBodyName != certBody.OldCertBodyName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", certBody.CertBodyName));
                    }
                    #endregion

                    else
                    {
                        if (checkCertBody.ExistData == 1 && certBody.CertBodyName == certBody.OldCertBodyName)
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
                }

                return View(certBody);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "CERTIFICATE BODY";

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

                string msg = ex.Message;
                string method = trace.GetFrame((trace.FrameCount - 1)).GetMethod().ToString();
                Int32 lineNumber = trace.GetFrame((trace.FrameCount - 1)).GetFileLineNumber();

                logDBContext.AddErrorLog(path, method, lineNumber, msg, UserName);
                #endregion

                return View();
            }
        }
        #endregion

        #region DELETE
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        public JsonResult Delete(int Id)
        {
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                CertBody certBody = dbContext.GetCertBodyByID(Id);

                dbContext.DeleteCertBody(Id, certBody.CertBodyName, UserName);
                TempData["deleteMessage"] = string.Format("{0} has been deleted from BLMS database!", certBody.CertBodyName);

                return Json(new { status = "Success" });
            }
            catch(Exception ex)
            {
                #region ERROR LOG
                string path = "CERTIFICATE BODY";

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

                string msg = ex.Message;
                string method = trace.GetFrame((trace.FrameCount - 1)).GetMethod().ToString();
                Int32 lineNumber = trace.GetFrame((trace.FrameCount - 1)).GetFileLineNumber();

                logDBContext.AddErrorLog(path, method, lineNumber, msg, UserName);
                #endregion

                return Json(new { status = "Fail" });
            }
        }
        #endregion
    }
}
