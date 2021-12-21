using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.License;
using BLMS.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class LicenseAdminController : Controller
    {
        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();
        readonly ddlLicenseDBContext ddlLicenseDBContext = new ddlLicenseDBContext();

        private IWebHostEnvironment _env;

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        public IActionResult Index()
        {
            List<LicenseAdmin> licenseHQList = licenseDbContext.LicenseAdminGetAll().ToList();

            if (TempData["RegisterMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["RegisterMessage"].ToString());
            }
            else if (TempData["RenewalMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["RenewalMessage"].ToString());
            }
            else if (TempData["EditMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["EditMessage"].ToString());
            }

            return View(licenseHQList);
        }
        #endregion

        #region VIEW REQUEST/APPROVE/REJECT LICENSE
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult DetailRequest(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            return View(licenseAdmin);
        }
        #endregion

        #region VIEW REGISTER LICENSE
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult DetailRegister(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            return View(licenseAdmin);
        }
        #endregion

        #region VIEW RENEWAL LICENSE
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult DetailRenewal (int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            List<LicenseAdmin> HistorylicenseAdmin = licenseDbContext.LicenseAdminGetLog(licenseAdmin.LicenseName).ToList();

            RenewalLicenseHQViewModel ViewModel = new RenewalLicenseHQViewModel();

            ViewModel.RenewalLicense = licenseAdmin;
            ViewModel.History = licenseDbContext.LicenseAdminGetLog(licenseAdmin.LicenseName).ToList();

            if (ViewModel == null)
            {
                return NotFound();
            }

            return View(ViewModel);
        }
        #endregion

        #region EDIT REQUEST/APPROVE LICENSE HQ
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult EditRequest(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            //ddlCategory
            List<LicenseAdmin> categoryLicenseAdminList = ddlLicenseDBContext.ddlCategoryLicenseAdmin().ToList();
            categoryLicenseAdminList.Insert(0, new LicenseAdmin { CategoryID = 0, CategoryName = "Please Select Type of License" });
            ViewBag.ListofCategory = categoryLicenseAdminList;

            //ddlBusinessDiv
            List<LicenseAdmin> businessDivLicenseAdminList = ddlLicenseDBContext.ddlBusinessDivLicenseAdmin().ToList();
            businessDivLicenseAdminList.Insert(0, new LicenseAdmin { DivID = 0, DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivLicenseAdminList;

            //ddlBusinessUnit
            List<LicenseAdmin> businessUnitLicenseAdminList = ddlLicenseDBContext.ddlBusinessUnitLicenseAdmin().ToList();
            businessUnitLicenseAdminList.Insert(0, new LicenseAdmin { UnitID = 0, UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitLicenseAdminList;

            //ddlPIC2
            List<LicenseAdmin> PIC2LicenseAdminList = ddlLicenseDBContext.ddlPIC2HQLicenseAdmin().ToList();
            PIC2LicenseAdminList.Insert(0, new LicenseAdmin { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
            ViewBag.ListofPIC2 = PIC2LicenseAdminList;

            //ddlPIC3
            List<LicenseAdmin> PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3HQLicenseAdmin().ToList();
            PIC3LicenseHQList.Insert(0, new LicenseAdmin { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
            ViewBag.ListofPIC3 = PIC3LicenseHQList;

            return View(licenseAdmin);
        }

        // POST: LicenseHQController/Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRequest([Bind] LicenseAdmin licenseAdmin)
        {
            try
            {
                string UserName = User.Identity.Name;

                List<LicenseHQ> categoryLicenseHQList, businessDivLicenseHQList, businessUnitLicenseHQList, PIC2LicenseHQList, PIC3LicenseHQList;

                if (string.IsNullOrEmpty(licenseAdmin.PIC1Name))
                {
                    ModelState.AddModelError("", "Your session has been expired. Please login again.");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Your session has been expired. Please login again.");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Division", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Unit", "");
                }
                else if (licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select License Type", "Please select Business Division", "Please select Business Unit", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select License Type", "", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select Business Division", "", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select Business Unit", "", "");
                }
                else if (licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Division", "", "");
                }
                else if (licenseAdmin.CategoryID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Unit", "", "");
                }
                else if (licenseAdmin.DivID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName))
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                }
                else if (licenseAdmin.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                }
                else if (licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(licenseAdmin.Remarks))
                    {
                        licenseAdmin.Remarks = "-";
                    }

                    #region CHANGE FIRST LETTER (LOWER TO UPPER)
                    TextInfo cultInfoLicenseName = new CultureInfo("en-US", false).TextInfo;
                    string LicenseName = cultInfoLicenseName.ToTitleCase(licenseAdmin.LicenseName);

                    TextInfo cultInfoRemarks = new CultureInfo("en-US", false).TextInfo;
                    string Remarks = cultInfoRemarks.ToTitleCase(licenseAdmin.Remarks);

                    licenseAdmin.LicenseName = LicenseName;
                    licenseAdmin.Remarks = Remarks;
                    #endregion

                    LicenseHQ checkLicense = licenseDbContext.CheckLicenseByName(licenseAdmin.LicenseName);

                    if (checkLicense.ExistData == 1 && licenseAdmin.OldLicenseName != licenseAdmin.LicenseName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseAdmin.LicenseName));

                        //ddlCategory
                        categoryLicenseHQList = ddlLicenseDBContext.ddlCategoryLicenseHQ().ToList();
                        categoryLicenseHQList.Insert(0, new LicenseHQ { CategoryID = 0, CategoryName = "Please Select Type of License" });
                        ViewBag.ListofCategory = categoryLicenseHQList;

                        //ddlBusinessDiv
                        businessDivLicenseHQList = ddlLicenseDBContext.ddlBusinessDivLicenseHQ().ToList();
                        businessDivLicenseHQList.Insert(0, new LicenseHQ { DivID = 0, DivName = "Please Select Business Division" });
                        ViewBag.ListofBusinessDiv = businessDivLicenseHQList;

                        //ddlBusinessUnit
                        businessUnitLicenseHQList = ddlLicenseDBContext.ddlBusinessUnitLicenseHQ().ToList();
                        businessUnitLicenseHQList.Insert(0, new LicenseHQ { UnitID = 0, UnitName = "Please Select Business Unit" });
                        ViewBag.ListofBusinessUnit = businessUnitLicenseHQList;

                        //ddlPIC2
                        PIC2LicenseHQList = ddlLicenseDBContext.ddlPIC2LicenseHQ().ToList();
                        PIC2LicenseHQList.Insert(0, new LicenseHQ { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                        ViewBag.ListofPIC2 = PIC2LicenseHQList;

                        //ddlPIC3
                        PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3LicenseHQ().ToList();
                        PIC3LicenseHQList.Insert(0, new LicenseHQ { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                        ViewBag.ListofPIC3 = PIC3LicenseHQList;

                        return View(licenseAdmin);
                    }
                    else if (checkLicense.ExistData == 1 && licenseAdmin.OldLicenseName == licenseAdmin.LicenseName)
                    {
                        licenseDbContext.EditLicenseHQRequest(licenseAdmin, UserName);

                        TempData["EditMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        licenseDbContext.EditLicenseHQRequest(licenseAdmin, UserName);

                        TempData["EditMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);
                        return RedirectToAction("Index");
                    }
                }

                //ddlCategory
                categoryLicenseHQList = ddlLicenseDBContext.ddlCategoryLicenseHQ().ToList();
                categoryLicenseHQList.Insert(0, new LicenseHQ { CategoryID = 0, CategoryName = "Please Select Type of License" });
                ViewBag.ListofCategory = categoryLicenseHQList;

                //ddlBusinessDiv
                businessDivLicenseHQList = ddlLicenseDBContext.ddlBusinessDivLicenseHQ().ToList();
                businessDivLicenseHQList.Insert(0, new LicenseHQ { DivID = 0, DivName = "Please Select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseHQList;

                //ddlBusinessUnit
                businessUnitLicenseHQList = ddlLicenseDBContext.ddlBusinessUnitLicenseHQ().ToList();
                businessUnitLicenseHQList.Insert(0, new LicenseHQ { UnitID = 0, UnitName = "Please Select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseHQList;

                //ddlPIC2
                PIC2LicenseHQList = ddlLicenseDBContext.ddlPIC2LicenseHQ().ToList();
                PIC2LicenseHQList.Insert(0, new LicenseHQ { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseHQList;

                //ddlPIC3
                PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3LicenseHQ().ToList();
                PIC3LicenseHQList.Insert(0, new LicenseHQ { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseHQList;

                return View(licenseAdmin);
            }
            catch
            {
                TempData["EditLicenseReqHQMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region EDIT REGISTER/RENEW LICENSE HQ
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult EditRegister(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            #region DROPDOWN
            List<LicenseAdmin> categoryLicenseAdminList, businessDivLicenseAdminList, businessUnitLicenseAdminList, PIC2LicenseAdminList, PIC3LicenseAdminList;

            //ddlCategory
            categoryLicenseAdminList = ddlLicenseDBContext.ddlCategoryLicenseAdmin().ToList();
            categoryLicenseAdminList.Insert(0, new LicenseAdmin { CategoryID = 0, CategoryName = "Please Select Type of License" });
            
            //ddlBusinessDiv
            businessDivLicenseAdminList = ddlLicenseDBContext.ddlBusinessDivLicenseAdmin().ToList();
            businessDivLicenseAdminList.Insert(0, new LicenseAdmin { DivID = 0, DivName = "Please Select Business Division" });
            
            //ddlBusinessUnit
            businessUnitLicenseAdminList = ddlLicenseDBContext.ddlBusinessUnitLicenseAdmin().ToList();
            businessUnitLicenseAdminList.Insert(0, new LicenseAdmin { UnitID = 0, UnitName = "Please Select Business Unit" });
            
            //ddlPIC2
            PIC2LicenseAdminList = ddlLicenseDBContext.ddlPIC2HQLicenseAdmin().ToList();
            PIC2LicenseAdminList.Insert(0, new LicenseAdmin { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
            
            //ddlPIC3
            PIC3LicenseAdminList = ddlLicenseDBContext.ddlPIC3HQLicenseAdmin().ToList();
            PIC3LicenseAdminList.Insert(0, new LicenseAdmin { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });

            ViewBag.ListofCategory = categoryLicenseAdminList;
            ViewBag.ListofBusinessDiv = businessDivLicenseAdminList;
            ViewBag.ListofBusinessUnit = businessUnitLicenseAdminList;
            ViewBag.ListofPIC2 = PIC2LicenseAdminList;
            ViewBag.ListofPIC3 = PIC3LicenseAdminList;
            #endregion

            return View(licenseAdmin);
        }

        // POST: LicenseHQController/Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRegister([Bind] LicenseAdmin licenseAdmin)
        {
            try
            {
                string UserName = User.Identity.Name;

                List<LicenseHQ> categoryLicenseHQList, businessDivLicenseHQList, businessUnitLicenseHQList, PIC2LicenseHQList, PIC3LicenseHQList;

                if (string.IsNullOrEmpty(licenseAdmin.PIC1Name))
                {
                    ModelState.AddModelError("", "Your session has been expired. Please login again.");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Your session has been expired. Please login again.");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Division", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Unit", "");
                }
                else if (licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select License Type", "Please select Business Division", "Please select Business Unit", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select License Type", "", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select Business Division", "", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName) && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select Business Unit", "", "");
                }
                else if (licenseAdmin.CategoryID == 0 && licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Division", "", "");
                }
                else if (licenseAdmin.CategoryID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Unit", "", "");
                }
                else if (licenseAdmin.DivID == 0 && licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName))
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                }
                else if (licenseAdmin.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                }
                else if (licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(licenseAdmin.Remarks))
                    {
                        licenseAdmin.Remarks = "-";
                    }

                    #region CHANGE FIRST LETTER (LOWER TO UPPER)
                    TextInfo cultInfoLicenseName = new CultureInfo("en-US", false).TextInfo;
                    string LicenseName = cultInfoLicenseName.ToTitleCase(licenseAdmin.LicenseName);

                    TextInfo cultInfoRemarks = new CultureInfo("en-US", false).TextInfo;
                    string Remarks = cultInfoRemarks.ToTitleCase(licenseAdmin.Remarks);

                    licenseAdmin.LicenseName = LicenseName;
                    licenseAdmin.Remarks = Remarks;
                    #endregion

                    LicenseHQ checkLicense = licenseDbContext.CheckLicenseByName(licenseAdmin.LicenseName);

                    if (checkLicense.ExistData == 1 && licenseAdmin.OldLicenseName != licenseAdmin.LicenseName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseAdmin.LicenseName));

                        //ddlCategory
                        categoryLicenseHQList = ddlLicenseDBContext.ddlCategoryLicenseHQ().ToList();
                        categoryLicenseHQList.Insert(0, new LicenseHQ { CategoryID = 0, CategoryName = "Please Select Type of License" });
                        ViewBag.ListofCategory = categoryLicenseHQList;

                        //ddlBusinessDiv
                        businessDivLicenseHQList = ddlLicenseDBContext.ddlBusinessDivLicenseHQ().ToList();
                        businessDivLicenseHQList.Insert(0, new LicenseHQ { DivID = 0, DivName = "Please Select Business Division" });
                        ViewBag.ListofBusinessDiv = businessDivLicenseHQList;

                        //ddlBusinessUnit
                        businessUnitLicenseHQList = ddlLicenseDBContext.ddlBusinessUnitLicenseHQ().ToList();
                        businessUnitLicenseHQList.Insert(0, new LicenseHQ { UnitID = 0, UnitName = "Please Select Business Unit" });
                        ViewBag.ListofBusinessUnit = businessUnitLicenseHQList;

                        //ddlPIC2
                        PIC2LicenseHQList = ddlLicenseDBContext.ddlPIC2LicenseHQ().ToList();
                        PIC2LicenseHQList.Insert(0, new LicenseHQ { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                        ViewBag.ListofPIC2 = PIC2LicenseHQList;

                        //ddlPIC3
                        PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3LicenseHQ().ToList();
                        PIC3LicenseHQList.Insert(0, new LicenseHQ { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                        ViewBag.ListofPIC3 = PIC3LicenseHQList;

                        return View(licenseAdmin);
                    }
                    else if (checkLicense.ExistData == 1 && licenseAdmin.OldLicenseName == licenseAdmin.LicenseName)
                    {
                        licenseDbContext.EditLicenseHQRegister(licenseAdmin, UserName);

                        TempData["EditMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        licenseDbContext.EditLicenseHQRequest(licenseAdmin, UserName);

                        TempData["EditMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);
                        return RedirectToAction("Index");
                    }
                }

                //ddlCategory
                categoryLicenseHQList = ddlLicenseDBContext.ddlCategoryLicenseHQ().ToList();
                categoryLicenseHQList.Insert(0, new LicenseHQ { CategoryID = 0, CategoryName = "Please Select Type of License" });
                ViewBag.ListofCategory = categoryLicenseHQList;

                //ddlBusinessDiv
                businessDivLicenseHQList = ddlLicenseDBContext.ddlBusinessDivLicenseHQ().ToList();
                businessDivLicenseHQList.Insert(0, new LicenseHQ { DivID = 0, DivName = "Please Select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseHQList;

                //ddlBusinessUnit
                businessUnitLicenseHQList = ddlLicenseDBContext.ddlBusinessUnitLicenseHQ().ToList();
                businessUnitLicenseHQList.Insert(0, new LicenseHQ { UnitID = 0, UnitName = "Please Select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseHQList;

                //ddlPIC2
                PIC2LicenseHQList = ddlLicenseDBContext.ddlPIC2LicenseHQ().ToList();
                PIC2LicenseHQList.Insert(0, new LicenseHQ { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseHQList;

                //ddlPIC3
                PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3LicenseHQ().ToList();
                PIC3LicenseHQList.Insert(0, new LicenseHQ { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseHQList;

                return View(licenseAdmin);
            }
            catch
            {
                TempData["EditLicenseReqHQMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region REGISTER LICENSE HQ
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        [HttpGet]
        public ActionResult RegisterLicenseHQ(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            //ddlCategory
            List<LicenseAdmin> categoryLicenseAdminList = ddlLicenseDBContext.ddlCategoryLicenseAdmin().ToList();
            categoryLicenseAdminList.Insert(0, new LicenseAdmin { CategoryID = 0, CategoryName = "Please Select Type of License" });
            ViewBag.ListofCategory = categoryLicenseAdminList;

            //ddlBusinessDiv
            List<LicenseAdmin> businessDivLicenseAdminList = ddlLicenseDBContext.ddlBusinessDivLicenseAdmin().ToList();
            businessDivLicenseAdminList.Insert(0, new LicenseAdmin { DivID = 0, DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivLicenseAdminList;

            //ddlBusinessUnit
            List<LicenseAdmin> businessUnitLicenseAdminList = ddlLicenseDBContext.ddlBusinessUnitLicenseAdmin().ToList();
            businessUnitLicenseAdminList.Insert(0, new LicenseAdmin { UnitID = 0, UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitLicenseAdminList;

            //ddlPIC2
            List<LicenseAdmin> PIC2LicenseAdminList = ddlLicenseDBContext.ddlPIC2HQLicenseAdmin().ToList();
            PIC2LicenseAdminList.Insert(0, new LicenseAdmin { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
            ViewBag.ListofPIC2 = PIC2LicenseAdminList;

            //ddlPIC3
            List<LicenseAdmin> PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3HQLicenseAdmin().ToList();
            PIC3LicenseHQList.Insert(0, new LicenseAdmin { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
            ViewBag.ListofPIC3 = PIC3LicenseHQList;

            return View(licenseAdmin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterLicenseHQ([Bind] LicenseAdmin licenseAdmin, List<IFormFile> LicenseFile)
        {
            try
            {
                string UserName = HttpContext.User.Identity.Name;

                DateTime issuedDT = Convert.ToDateTime(licenseAdmin.IssuedDT);
                DateTime expiredDT = Convert.ToDateTime(licenseAdmin.ExpiredDT);

                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    licenseAdmin.LicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseAdmin.LicenseFileName))
                {
                    ModelState.AddModelError("", "Please upload License File");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName))
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RegistrationNo))
                {
                    ModelState.AddModelError("", "Please type Registration No");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                }
                else if (licenseAdmin.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                }
                else if (licenseAdmin.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (licenseAdmin.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (licenseAdmin.IssuedDT != null && licenseAdmin.ExpiredDT != null && issuedDT.Date > expiredDT.Date)
                {
                    ModelState.AddModelError("", "Expire Date {1} cannot less than Issue Date {0}");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseAdmin.IssuedDT, licenseAdmin.ExpiredDT));
                }
                else if (string.IsNullOrEmpty(licenseAdmin.PIC1Name))
                {
                    ModelState.AddModelError("", "Login session is expired. Please login again");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Login session is expired. Please login again");
                }
                else if(licenseAdmin.PIC2StaffNo == licenseAdmin.PIC3StaffNo)
                {
                    ModelState.AddModelError("", "PIC 2 and PIC 3 contains same Staff Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 contains same Staff Name");
                }
                else if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(licenseAdmin.SerialNo))
                    {
                        licenseAdmin.SerialNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.IssuedDT))
                    {
                        licenseAdmin.IssuedDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.ExpiredDT))
                    {
                        licenseAdmin.ExpiredDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.Remarks))
                    {
                        licenseAdmin.Remarks = "-";
                    }

                    foreach (var licenseFile in LicenseFile)
                    {
                        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\HQ\\");
                        bool basePathExists = System.IO.Directory.Exists(basePath);

                        if (!basePathExists) Directory.CreateDirectory(basePath);

                        var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                        var filePath = Path.Combine(basePath, licenseFile.FileName);
                        var extension = Path.GetExtension(licenseFile.FileName);

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                licenseFile.CopyToAsync(stream);
                            }

                            licenseAdmin.LicenseFileName = fileName + extension;
                        }
                    }

                    if (LicenseFile.Count == 0)
                    {
                        licenseAdmin.LicenseFileName = "-";
                    }

                    string LicenseName = licenseAdmin.LicenseName;
                    string Issued = licenseAdmin.IssuedDT;
                    string Expired = licenseAdmin.ExpiredDT;

                    licenseDbContext.RegisterLicenseHQ(licenseAdmin, Issued, Expired, UserName);

                    TempData["registerMessage"] = string.Format("{0} has been successfully registered!", LicenseName);

                    return RedirectToAction("Index");
                }

                //ddlCategory      
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });
                ViewBag.ListofCategory = categoryLicenseSiteList;

                //ddlBusinessDiv      
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;

                //ddlBusinessUnit             
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;

                //ddlPIC2                  
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;

                //ddlPIC3   
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;

                return View(licenseAdmin);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region RENEW LICENSE HQ
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        [HttpGet]
        public ActionResult RenewalLicenseHQ(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            List<LicenseAdmin> HistorylicenseAdmin = licenseDbContext.LicenseAdminGetLog(licenseAdmin.LicenseName).ToList();

            RenewalLicenseHQViewModel ViewModel = new RenewalLicenseHQViewModel();

            ViewModel.RenewalLicense = licenseAdmin;
            ViewModel.History = licenseDbContext.LicenseAdminGetLog(licenseAdmin.LicenseName).ToList();

            if (ViewModel == null)
            {
                return NotFound();
            }

            //ddlCategory
            List<LicenseAdmin> categoryLicenseAdminList = ddlLicenseDBContext.ddlCategoryLicenseAdmin().ToList();
            categoryLicenseAdminList.Insert(0, new LicenseAdmin { CategoryID = 0, CategoryName = "Please Select Type of License" });
            ViewBag.ListofCategory = categoryLicenseAdminList;

            //ddlBusinessDiv
            List<LicenseAdmin> businessDivLicenseAdminList = ddlLicenseDBContext.ddlBusinessDivLicenseAdmin().ToList();
            businessDivLicenseAdminList.Insert(0, new LicenseAdmin { DivID = 0, DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivLicenseAdminList;

            //ddlBusinessUnit
            List<LicenseAdmin> businessUnitLicenseAdminList = ddlLicenseDBContext.ddlBusinessUnitLicenseAdmin().ToList();
            businessUnitLicenseAdminList.Insert(0, new LicenseAdmin { UnitID = 0, UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitLicenseAdminList;

            //ddlPIC2
            List<LicenseAdmin> PIC2LicenseAdminList = ddlLicenseDBContext.ddlPIC2HQLicenseAdmin().ToList();
            PIC2LicenseAdminList.Insert(0, new LicenseAdmin { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
            ViewBag.ListofPIC2 = PIC2LicenseAdminList;

            //ddlPIC3
            List<LicenseAdmin> PIC3LicenseHQList = ddlLicenseDBContext.ddlPIC3HQLicenseAdmin().ToList();
            PIC3LicenseHQList.Insert(0, new LicenseAdmin { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
            ViewBag.ListofPIC3 = PIC3LicenseHQList;

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenewalLicenseHQ([Bind] RenewalLicenseHQViewModel licenseAdmin, List<IFormFile> LicenseFile)
        {
            try
            {
                RenewalLicenseHQViewModel ViewModel = new RenewalLicenseHQViewModel();

                ViewModel.RenewalLicense = licenseAdmin.RenewalLicense;
                ViewModel.History = licenseDbContext.LicenseAdminGetLog(licenseAdmin.RenewalLicense.LicenseName).ToList();

                string UserName = HttpContext.User.Identity.Name;

                DateTime issuedDT = Convert.ToDateTime(licenseAdmin.RenewalLicense.NewIssuedDT);
                DateTime expiredDT = Convert.ToDateTime(licenseAdmin.RenewalLicense.NewExpiredDT);

                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    licenseAdmin.RenewalLicense.NewLicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewLicenseFileName))
                {
                    ModelState.AddModelError("", "Please upload License File");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.LicenseName))
                {
                    ModelState.AddModelError("", "Please type License Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewRegistrationNo))
                {
                    ModelState.AddModelError("", "Please type Registration No");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                }
                else if (licenseAdmin.RenewalLicense.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                }
                else if (licenseAdmin.RenewalLicense.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (licenseAdmin.RenewalLicense.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (licenseAdmin.RenewalLicense.NewIssuedDT != null && licenseAdmin.RenewalLicense.NewExpiredDT != null && issuedDT.Date > expiredDT.Date)
                {
                    ModelState.AddModelError("", "Expire Date {1} cannot less than Issue Date {0}");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseAdmin.RenewalLicense.IssuedDT, licenseAdmin.RenewalLicense.ExpiredDT));
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewPIC1Name))
                {
                    ModelState.AddModelError("", "Login session is expired. Please login again");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Login session is expired. Please login again");
                }
                else if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewSerialNo))
                    {
                        licenseAdmin.RenewalLicense.SerialNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewIssuedDT))
                    {
                        licenseAdmin.RenewalLicense.IssuedDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewExpiredDT))
                    {
                        licenseAdmin.RenewalLicense.ExpiredDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewRemarks))
                    {
                        licenseAdmin.RenewalLicense.Remarks = "-";
                    }

                    foreach (var licenseFile in LicenseFile)
                    {
                        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\HQ\\");
                        bool basePathExists = System.IO.Directory.Exists(basePath);

                        if (!basePathExists) Directory.CreateDirectory(basePath);

                        var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                        var filePath = Path.Combine(basePath, licenseFile.FileName);
                        var extension = Path.GetExtension(licenseFile.FileName);

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                licenseFile.CopyToAsync(stream);
                            }

                            licenseAdmin.RenewalLicense.NewLicenseFileName = fileName + extension;
                        }
                    }

                    if (LicenseFile.Count == 0)
                    {
                        licenseAdmin.RenewalLicense.NewLicenseFileName = "-";
                    }

                    string LicenseName = licenseAdmin.RenewalLicense.LicenseName;
                    string Issued = licenseAdmin.RenewalLicense.NewIssuedDT;
                    string Expired = licenseAdmin.RenewalLicense.NewExpiredDT;

                    licenseDbContext.RenewalLicenseHQ(licenseAdmin, Issued, Expired, UserName);

                    TempData["registerMessage"] = string.Format("{0} has been successfully renewed!", LicenseName);

                    return RedirectToAction("Index");
                }

                //ddlCategory      
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });
                ViewBag.ListofCategory = categoryLicenseSiteList;

                //ddlBusinessDiv      
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;

                //ddlBusinessUnit             
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;

                //ddlPIC2                  
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;

                //ddlPIC3   
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;

                return View(ViewModel);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DOWNLOAD LICENSE FILE
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        public async Task<IActionResult> DownloadLicenseFile(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            //check license site is not null
            if (licenseAdmin == null) return null;

            var basePath = "";

            //get file path
            if (licenseAdmin.UserType == "HQ")
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\HQ");
            }
            else if (licenseAdmin.UserType == "SITE")
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\Site");
            }

            var filePath = Path.Combine(basePath, licenseAdmin.LicenseFileName);

            //get file extension to check mimeType
            var extension = Path.GetExtension(licenseAdmin.LicenseFileName);
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(licenseAdmin.LicenseFileName, out string contentType))
            {
                contentType = DefaultContentType;
            }

            var FileType = contentType;

            //get memory stream of file
            var memory = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, FileType, licenseAdmin.LicenseFileName);
        }
        #endregion

        #region LINKED BUSINESS UNIT TO BUSINESS DIV
        public JsonResult JSONGetUnitName(int DivID)
        {
            DataSet ds = ddlLicenseDBContext.ddlBusinessUnitLinkedDivAdmin(DivID);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["UnitName"].ToString(), Value = dr["UnitID"].ToString() });
            }
            return Json(list);
        }
        #endregion
    }
}
