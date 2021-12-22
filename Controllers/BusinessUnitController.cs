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
    public class BusinessUnitController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();
        readonly ddlAdminDBContext ddlDBContext = new ddlAdminDBContext();
        readonly LogDBContext logDBContext = new LogDBContext();

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult Index()
        {
            List<BusinessUnit> BusinessUnitList = dbContext.BusinessUnitGetAll().ToList();

            #region ALERT MESSAGE
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
            #endregion

            return View(BusinessUnitList);
        }
        #endregion

        #region CREATE
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult Create()
        {
            #region DROPDOWN
            List<BusinessUnit> businessDivList = ddlDBContext.ddlBusinessDiv().ToList();

            businessDivList.Insert(0, new BusinessUnit { DivID = 0, DivName = "Please select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivList;
            #endregion

            return View();
        }

        // POST: BusinessUnitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] BusinessUnit businessUnit, string UserName)
        {
            UserName = HttpContext.User.Identity.Name;

            try
            {
                List<BusinessUnit> businessDivList;

                #region VALIDATION
                if (businessUnit.DivID == 0)
                {
                    if (string.IsNullOrEmpty(businessUnit.UnitName))
                    {
                        if (string.IsNullOrEmpty(businessUnit.HoCName))
                        {
                            if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "Please type HoC Email");
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type Business Unit", "", "");
                        }
                    }
                    else if (string.IsNullOrEmpty(businessUnit.HoCName))
                    {
                        if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type HoC Name", "Please type HoC Email", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Name", "", "");
                        }
                    }
                    else if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Email", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (string.IsNullOrEmpty(businessUnit.UnitName))
                {
                    if (string.IsNullOrEmpty(businessUnit.HoCName))
                    {
                        if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Business Unit", "Please type HoC Name", "Please type HoC Email", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Business Unit", "Please type HoC Name", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Business Unit");
                    }
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type HoC Name", "Please type HoC Email", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Name");
                    }
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Email");
                }
                #endregion

                else
                {
                    BusinessUnit checkbusinessUnit = dbContext.CheckBusinessUnitByName(businessUnit.UnitName);

                    #region CHECK DUPLICATION
                    if (checkbusinessUnit.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", businessUnit.UnitName));
                    }
                    #endregion

                    else
                    {
                        dbContext.AddBusinessUnit(businessUnit, UserName);
                        TempData["createMessage"] = string.Format("{0} has been successfully created!", businessUnit.UnitName);
                        return RedirectToAction("Index");
                    }
                }

                #region DROPDOWN
                //Set Data Back After Postback
                businessDivList = ddlDBContext.ddlBusinessDiv().ToList();
                businessDivList.Insert(0, new BusinessUnit { DivID = 0, DivName = "Please select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivList;
                #endregion

                return View(businessUnit);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "BUSINESS UNIT";

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
            BusinessUnit businessUnit = dbContext.GetBusinessUnitByID(id);

            #region DROPDOWN
            List<BusinessUnit> businessDivList = ddlDBContext.ddlBusinessDiv().ToList();
            businessDivList.Insert(0, new BusinessUnit { DivID = 0, DivName = "PLEASE SELECT BUSINESS DIVISION" });
            ViewBag.ListofBusinessDiv = businessDivList;
            #endregion

            if (businessUnit == null)
            {
                return NotFound();
            }

            return View(businessUnit);
        }

        // POST: BusinessUnitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] BusinessUnit businessUnit, string UserName)
        {
            UserName = HttpContext.User.Identity.Name;

            try
            {
                List<BusinessUnit> businessDivList;

                #region VALIDATION
                if (businessUnit.DivID == 0)
                {
                    if (string.IsNullOrEmpty(businessUnit.UnitName))
                    {
                        if (string.IsNullOrEmpty(businessUnit.HoCName))
                        {
                            if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "Please type HoC Email");
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type Business Unit", "", "");
                        }
                    }
                    else if (string.IsNullOrEmpty(businessUnit.HoCName))
                    {
                        if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type HoC Name", "Please type HoC Email", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Name", "", "");
                        }
                    }
                    else if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Email", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (string.IsNullOrEmpty(businessUnit.UnitName))
                {
                    if (string.IsNullOrEmpty(businessUnit.HoCName))
                    {
                        if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Business Unit", "Please type HoC Name", "Please type HoC Email", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Business Unit", "Please type HoC Name", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Business Unit");
                    }
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type HoC Name", "Please type HoC Email", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Name");
                    }
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Email");
                }
                #endregion

                else
                {
                    BusinessUnit checkbusinessUnit = dbContext.CheckBusinessUnitByName(businessUnit.UnitName);

                    #region CHECK DUPLICATION
                    if (checkbusinessUnit.ExistData == 1 && businessUnit.UnitName != businessUnit.OldUnitName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", businessUnit.UnitName));
                    }
                    #endregion

                    else
                    {
                        if(businessUnit.DivID == businessUnit.OldDivID && businessUnit.OldUnitName == businessUnit.UnitName && businessUnit.OldHoCName == businessUnit.HoCName && businessUnit.OldHoCEmail == businessUnit.OldHoCEmail)
                        {
                            dbContext.EditBusinessUnit(businessUnit, UserName);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            dbContext.EditBusinessUnit(businessUnit, UserName);
                            TempData["editMessage"] = string.Format("{0} has been successfully edited!", businessUnit.UnitName);
                            return RedirectToAction("Index");
                        }
                    }
                }

                #region DROPDOWN
                businessDivList = ddlDBContext.ddlBusinessDiv().ToList();
                businessDivList.Insert(0, new BusinessUnit { DivID = 0, DivName = "Please select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivList;
                #endregion

                return View(businessUnit);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "BUSINESS UNIT";

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
                BusinessUnit businessUnit = dbContext.GetBusinessUnitByID(Id);

                dbContext.DeleteBusinessUnit(Id, businessUnit.UnitName, UserName);
                TempData["deleteMessage"] = string.Format("{0} has been successfully deleted!", businessUnit.UnitName);

                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "BUSINESS UNIT";

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
