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
    public class BusinessUnitController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();
        readonly ddlAdminDBContext ddlDBContext = new ddlAdminDBContext();

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
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
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
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
            try
            {
                string UnitName = businessUnit.UnitName;

                UserName = HttpContext.User.Identity.Name;

                List<BusinessUnit> businessDivList;

                #region VALIDATION
                if (businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "Please type HoC Email");
                }
                else if(businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "");
                }
                else if(businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type Business Unit", "Please type HoC Email", "");
                }
                else if(businessUnit.DivID == 0 && string.IsNullOrEmpty(businessUnit.HoCName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type HoC Name", "Please type HoC Email", "");
                }
                else if(string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please type  Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Business Unit", "Please type HoC Name", "Please type HoC Email", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type Business Unit", "", "");
                }
                else if (string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Business Unit", "Please type HoC Name", "", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Name", "", "");
                }
                else if(businessUnit.DivID == 0 && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Email", "", "");
                }
                else if (businessUnit.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (string.IsNullOrEmpty(UnitName))
                {
                    ModelState.AddModelError("", "Please type Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Business Unit");
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Name");
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Email");
                }
                #endregion

                else
                {
                    BusinessUnit checkbusinessUnit = dbContext.CheckBusinessUnitByName(UnitName);

                    #region CHECK DUPLICATION
                    if (checkbusinessUnit.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", UnitName));
                    }
                    #endregion

                    else
                    {
                        dbContext.AddBusinessUnit(businessUnit, UserName);
                        TempData["createMessage"] = string.Format("{0} has been successfully created!", UnitName);
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
            try
            {
                string UnitName = businessUnit.UnitName;
                UserName = HttpContext.User.Identity.Name;
                List<BusinessUnit> businessDivList;

                #region VALIDATION
                if (businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "Please type HoC Email");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type Business Unit", "Please type HoC Name", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type Business Unit", "Please type HoC Email", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(businessUnit.HoCName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Business Division", "Please type HoC Name", "Please type HoC Email", "");
                }
                else if (string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName) && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please type  Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Business Unit", "Please type HoC Name", "Please type HoC Email", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(UnitName))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type Business Unit", "", "");
                }
                else if (string.IsNullOrEmpty(UnitName) && string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please type Business Unit");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Business Unit", "Please type HoC Name", "", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Name", "", "");
                }
                else if (businessUnit.DivID == 0 && string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please type HoC Email", "", "");
                }
                else if (businessUnit.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (string.IsNullOrEmpty(UnitName))
                {
                    ModelState.AddModelError("", "Please type Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Business Unit");
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCName))
                {
                    ModelState.AddModelError("", "Please type HoC Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Name");
                }
                else if (string.IsNullOrEmpty(businessUnit.HoCEmail))
                {
                    ModelState.AddModelError("", "Please type HoC Email");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type HoC Email");
                }
                #endregion

                else
                {
                    string OldUnitName = businessUnit.OldUnitName;

                    BusinessUnit checkbusinessUnit = dbContext.CheckBusinessUnitByName(UnitName);

                    #region CHECK DUPLICATION
                    if (checkbusinessUnit.ExistData == 1 && UnitName != OldUnitName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", UnitName));
                    }
                    #endregion

                    else
                    {
                        dbContext.EditBusinessUnit(businessUnit, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", businessUnit.UnitName);

                        return RedirectToAction("Index");
                    }
                }

                #region DROPDOWN
                businessDivList = ddlDBContext.ddlBusinessDiv().ToList();
                businessDivList.Insert(0, new BusinessUnit { DivID = 0, DivName = "Please select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivList;
                #endregion

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
                BusinessUnit businessUnit = dbContext.GetBusinessUnitByID(Id);

                dbContext.DeleteBusinessUnit(Id);
                TempData["deleteMessage"] = string.Format("{0} has been successfully deleted!", businessUnit.UnitName);

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
