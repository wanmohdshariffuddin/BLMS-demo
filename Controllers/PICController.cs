using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class PICController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();
        readonly ddlAdminDBContext ddlDBContext = new ddlAdminDBContext();

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        public ActionResult Index()
        {
            List<PIC> PICList = dbContext.PICGetAll().ToList();

            #region ERROR MESSAGE
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

            return View(PICList);
        }
        #endregion

        #region CREATE
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult Create()
        {
            #region DROPDOWN
            //ddlStaffName
            List<PIC> staffNamePICList = ddlDBContext.ddlStaffNamePIC().ToList();
            staffNamePICList.Insert(0, new PIC { PICStaffNo = "-", PICName = "Please select Staff Name" });
            ViewBag.ListofStaffName = staffNamePICList;

            //ddlUnitType
            List<PIC> userTypePICList = ddlDBContext.ddlUserTypePIC().ToList();
            userTypePICList.Insert(0, new PIC { UserTypeID = 0, UserType = "Please select User Type" });
            ViewBag.ListofUserType = userTypePICList;
            #endregion

            return View();
        }

        // POST: PICController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] PIC pic, string UserName)
        {
            try
            {
                UserName = HttpContext.User.Identity.Name;
                List<PIC> staffNamePICList, userTypePICList;

                #region VALIDATION
                if (pic.PICStaffNo == "-" && pic.UserTypeID == 0 && string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ModelState.AddModelError("", "Please select User Type");
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Staff Name", "Please select User Type", "Please Type Staff Short Name", "");
                }
                else if(pic.PICStaffNo == "-" && pic.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ModelState.AddModelError("", "Please select User Type");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Staff Name", "Please select User Type", "", ""); 
                }
                else if(pic.PICStaffNo == "-" && string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Staff Name", "Please Type Staff Short Name", "", "");
                }
                else if(pic.UserTypeID == 0 && string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please select User Type");
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select User Type", "Please Type Staff Short Name", "", "");
                }
                else if (pic.PICStaffNo == "-")
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Staff Name");
                }
                else if (pic.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "Please select User Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select User Type");
                }
                else if (string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Type Staff Short Name");
                }
                #endregion

                else if (ModelState.IsValid)
                {
                    string PICStaffNo = pic.PICStaffNo;

                    PIC checkPIC = dbContext.CheckPICByName(PICStaffNo);

                    #region CHECK EXIST DATA
                    if (checkPIC.ExistData == 1)
                    {
                        PIC GetPICName = dbContext.GetPICByStaffNo(pic.PICStaffNo);
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", GetPICName.PICName));
                    }
                    #endregion

                    else
                    {
                        dbContext.AddPIC(pic, UserName);

                        PIC GetPICName = dbContext.GetPICByStaffNo(pic.PICStaffNo);

                        TempData["createMessage"] = string.Format("{0} has been successfully created!", GetPICName.PICName);
                        return RedirectToAction("Index");
                    }
                }

                #region DROPDOWN
                //ddlStaffName
                staffNamePICList = ddlDBContext.ddlStaffNamePIC().ToList();
                staffNamePICList.Insert(0, new PIC { PICStaffNo = "-", PICName = "Please select Staff Name" });
                ViewBag.ListofStaffName = staffNamePICList;

                userTypePICList = ddlDBContext.ddlUserTypePIC().ToList();
                userTypePICList.Insert(0, new PIC { UserTypeID = 0, UserType = "Please select User Type" });
                ViewBag.ListofUserType = userTypePICList;
                #endregion

                return View(pic);
            }
            catch
            {
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
            PIC pic = dbContext.GetPICByID(id);

            #region DROPDOWN
            //ddlStaffName
            List<PIC> staffNamePICList = ddlDBContext.ddlStaffNamePIC().ToList();
            staffNamePICList.Insert(0, new PIC { PICStaffNo = "-", PICName = "Please select Staff Name" });
            ViewBag.ListofStaffName = staffNamePICList;

            //ddlUnitType
            List<PIC> userTypePICList = ddlDBContext.ddlUserTypePIC().ToList();
            userTypePICList.Insert(0, new PIC { UserTypeID = 0, UserType = "Please select User Type" });
            ViewBag.ListofUserType = userTypePICList;
            #endregion

            if (pic == null)
            {
                return NotFound();
            }

            return View(pic);
        }

        // POST: PICController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] PIC pic, string UserName)
        {
            try
            {
                UserName = HttpContext.User.Identity.Name;

                List<PIC> staffNamePICList, userTypePICList;

                #region VALIDATION
                if (pic.PICStaffNo == "-" && pic.UserTypeID == 0 && string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ModelState.AddModelError("", "Please select User Type");
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select Staff Name", "Please select User Type", "Please Type Staff Short Name", "");
                }
                else if (pic.PICStaffNo == "-" && pic.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ModelState.AddModelError("", "Please select User Type");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Staff Name", "Please select User Type", "", "");
                }
                else if (pic.PICStaffNo == "-" && string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Staff Name", "Please Type Staff Short Name", "", "");
                }
                else if (pic.UserTypeID == 0 && string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please select User Type");
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select User Type", "Please Type Staff Short Name", "", "");
                }
                else if (pic.PICStaffNo == "-")
                {
                    ModelState.AddModelError("", "Please select Staff Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Staff Name");
                }
                else if (pic.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "Please select User Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select User Type");
                }
                else if (string.IsNullOrEmpty(pic.ShortName))
                {
                    ModelState.AddModelError("", "Please type Staff Short Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Type Staff Short Name");
                }
                #endregion

                else if (ModelState.IsValid)
                {
                    string PICStaffNo = pic.PICStaffNo;
                    string OldPICStaffNo = pic.OldPICStaffNo;

                    PIC checkPIC = dbContext.CheckPICByName(PICStaffNo);
                    PIC checkShortName = dbContext.CheckPICShortName(pic.ShortName);

                    #region CHECK DUPLICATE NAME STAFF
                    if (checkPIC.ExistData == 1 && PICStaffNo != OldPICStaffNo)
                    {
                        PIC GetPICName = dbContext.GetPICByStaffNo(pic.PICStaffNo);
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("Staff Name {0} already existed in BLMS database!", GetPICName.PICName));
                    }
                    #endregion

                    #region CHECK DUPLICATE SHORT NAME
                    else if (checkShortName.ExistShortName == 1 && pic.ShortName !=pic.OldShortName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("Short Name {0} already existed in BLMS database!", pic.ShortName));
                    }
                    #endregion

                    else
                    {
                        if (PICStaffNo == OldPICStaffNo && pic.UserTypeID == pic.OldUserTypeID)
                        {
                            dbContext.EditPIC(pic, UserName);
                            return RedirectToAction("Index");
                        }
                        else if (PICStaffNo != OldPICStaffNo || pic.UserTypeID != pic.OldUserTypeID)
                        {
                            dbContext.EditPIC(pic, UserName);

                            PIC GetPICName = dbContext.GetPICByStaffNo(pic.PICStaffNo);

                            TempData["editMessage"] = string.Format("{0} has been successfully edited!", GetPICName.PICName);
                            return RedirectToAction("Index");
                        }
                    }
                }

                #region DROPDOWN
                staffNamePICList = ddlDBContext.ddlStaffNamePIC().ToList();
                staffNamePICList.Insert(0, new PIC { PICStaffNo = "-", PICName = "Please select Staff Name" });
                ViewBag.ListofStaffName = staffNamePICList;

                userTypePICList = ddlDBContext.ddlUserTypePIC().ToList();
                userTypePICList.Insert(0, new PIC { UserTypeID = 0, UserType = "Please select User Type" });
                ViewBag.ListofUserType = userTypePICList;
                #endregion

                return View(pic);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        //[Authorize(Roles.ADMINISTRATOR)]
        public JsonResult Delete(int Id)
        {
            try
            {
                PIC pic = dbContext.GetPICByID(Id);

                dbContext.DeletePIC(Id);

                TempData["deleteMessage"] = string.Format("{0} has been successfully deleted!", pic.PICName);

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
