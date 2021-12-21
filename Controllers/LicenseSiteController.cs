using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models;
using BLMS.Models.License;
using BLMS.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class LicenseSiteController : Controller
    {
        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();
        readonly ddlLicenseDBContext ddlLicenseDBContext = new ddlLicenseDBContext();

        private IWebHostEnvironment _env;

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        public IActionResult Index()
        {
            List<LicenseSite> licenseSiteList = licenseDbContext.LicenseSiteGetAll().ToList();

            if (TempData["registerMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["registerMessage"].ToString());
            }
            else if (TempData["renewalMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["renewalMessage"].ToString());
            }
            else if (TempData["editMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["editMessage"].ToString());
            }

            return View(licenseSiteList);
        }
        #endregion

        #region VIEW REGISTER LICENSE
        //[Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        //[NoDirectAccess]
        public ActionResult DetailRegister(int id)
        {
            LicenseSite licenseSite = licenseDbContext.GetLicenseSiteByID(id);

            if (licenseSite == null)
            {
                return NotFound();
            }

            return View(licenseSite);
        }
        #endregion

        #region VIEW RENEWAL LICENSE
        //[Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        //[NoDirectAccess]
        public ActionResult DetailRenewal(int id)
        {
            LicenseSite licenseSite = licenseDbContext.GetLicenseSiteByID(id);

            List<LicenseSite> HistorylicenseSite = licenseDbContext.LicenseSiteGetLog(licenseSite.LicenseName).ToList();

            RenewalLicenseSiteViewModel ViewModel = new RenewalLicenseSiteViewModel();

            ViewModel.RenewalLicense = licenseSite;
            ViewModel.History = licenseDbContext.LicenseSiteGetLog(licenseSite.LicenseName).ToList();

            if (ViewModel == null)
            {
                return NotFound();
            }

            return View(ViewModel);
        }
        #endregion

        #region REGISTER
        //[Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [HttpGet]
        public ActionResult Register()
        {
            #region Dropdownlist
            List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

            //ddlCategory      
            categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
            categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });

            //ddlBusinessDiv      
            businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
            businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });

            //ddlBusinessUnit             
            businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
            businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });

            //ddlPIC2                  
            PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
            PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });

            //ddlPIC3   
            PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
            PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });

            ViewBag.ListofCategory = categoryLicenseSiteList;
            ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;
            ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;
            ViewBag.ListofPIC2 = PIC2LicenseSiteList;
            ViewBag.ListofPIC3 = PIC3LicenseSiteList;
            #endregion

            return View();
        }

        // POST: LicenseSiteController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind] LicenseSite licenseSite, List<IFormFile> LicenseFile)
        {
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                #region Dropdownlist
                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                //ddlCategory      
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });

                //ddlBusinessDiv      
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });

                //ddlBusinessUnit             
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });

                //ddlPIC2                  
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });

                //ddlPIC3   
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                #endregion

                LicenseSite checkLicenseSite = licenseDbContext.CheckLicenseSiteByName(licenseSite.LicenseName);

                if (checkLicenseSite.ExistData == 1)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseSite.LicenseName));
                }
                else
                {
                    foreach (var licenseFile in LicenseFile)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                        licenseSite.LicenseFileName = fileName;
                    }

                    #region CUSTOM VALIDATION
                    DateTime issuedDT = Convert.ToDateTime(licenseSite.IssuedDT);
                    DateTime expiredDT = Convert.ToDateTime(licenseSite.ExpiredDT);

                    if (string.IsNullOrEmpty(licenseSite.LicenseFileName))
                    {
                        ModelState.AddModelError("", "Please upload License File");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                    }
                    else if (string.IsNullOrEmpty(licenseSite.LicenseName))
                    {
                        ModelState.AddModelError("", "Please type License Name");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                    }
                    else if (string.IsNullOrEmpty(licenseSite.RegistrationNo))
                    {
                        ModelState.AddModelError("", "Please type Registration No");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                    }
                    else if (licenseSite.CategoryID == 0)
                    {
                        ModelState.AddModelError("", "Please select License Type");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                    }
                    else if (licenseSite.DivID == 0)
                    {
                        ModelState.AddModelError("", "Please select Business Division");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                    else if (licenseSite.UnitID == 0)
                    {
                        ModelState.AddModelError("", "Please select Business Unit");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                    }
                    else if (licenseSite.IssuedDT != null && licenseSite.ExpiredDT != null && issuedDT.Date > expiredDT.Date)
                    {
                        ModelState.AddModelError("", "Expire Date {1} cannot less than Issue Date {0}");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseSite.IssuedDT, licenseSite.ExpiredDT));
                    }
                    else if (string.IsNullOrEmpty(licenseSite.PIC1Name))
                    {
                        ModelState.AddModelError("", "Login session is expired. Please login again");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Login session is expired. Please login again");
                    }
                    else if (licenseSite.PIC2StaffNo != "-" && licenseSite.PIC3StaffNo != "-" && licenseSite.PIC2StaffNo == licenseSite.PIC3StaffNo)
                    {
                        ModelState.AddModelError("", "PIC 2 and PIC 3 contains same Staff Name");
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 contains same Staff Name");
                    }
                    #endregion
                    else
                    {
                        #region INITIALIZE MODEL DATA
                        if (String.IsNullOrEmpty(licenseSite.SerialNo))
                        {
                            licenseSite.SerialNo = "-";
                        }

                        if (String.IsNullOrEmpty(licenseSite.IssuedDT))
                        {
                            licenseSite.IssuedDT = "-";
                        }

                        if (String.IsNullOrEmpty(licenseSite.ExpiredDT))
                        {
                            licenseSite.ExpiredDT = "-";
                        }

                        if (String.IsNullOrEmpty(licenseSite.Remarks))
                        {
                            licenseSite.Remarks = "-";
                        }

                        if (LicenseFile.Count == 0)
                        {
                            licenseSite.LicenseFileName = "-";
                        }
                        #endregion

                        #region CHANGE FIRST LETTER (LOWER TO UPPER)
                        TextInfo cultInfoLicenseName = new CultureInfo("en-US", false).TextInfo;
                        string LicenseName = cultInfoLicenseName.ToTitleCase(licenseSite.LicenseName);

                        TextInfo cultInfoRemarks = new CultureInfo("en-US", false).TextInfo;
                        string Remarks = cultInfoRemarks.ToTitleCase(licenseSite.Remarks);

                        licenseSite.LicenseName = LicenseName;
                        licenseSite.Remarks = Remarks;
                        #endregion

                        #region SAVE DATA
                        foreach (var licenseFile in LicenseFile)
                        {
                            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE\\");
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

                                licenseSite.LicenseFileName = fileName + extension;
                            }
                        }

                        TempData["registerMessage"] = string.Format("{0} has been successfully registered!", licenseSite.LicenseName);
                        licenseDbContext.RegisterLicenseSite(licenseSite, UserName);

                        return RedirectToAction("Index", "LicenseSite");
                        #endregion
                    }
                }

                ViewBag.ListofCategory = categoryLicenseSiteList;
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;

                return View(licenseSite);
            }
            catch
            {
                #region INITIALIZE MODEL DATA
                if (String.IsNullOrEmpty(licenseSite.SerialNo))
                {
                    licenseSite.SerialNo = "-";
                }

                if (String.IsNullOrEmpty(licenseSite.IssuedDT))
                {
                    licenseSite.IssuedDT = "-";
                }

                if (String.IsNullOrEmpty(licenseSite.ExpiredDT))
                {
                    licenseSite.ExpiredDT = "-";
                }

                if (String.IsNullOrEmpty(licenseSite.Remarks))
                {
                    licenseSite.Remarks = "-";
                }

                if (LicenseFile.Count == 0)
                {
                    licenseSite.LicenseFileName = "-";
                }
                #endregion

                #region CHANGE FIRST LETTER (LOWER TO UPPER)
                TextInfo cultInfoLicenseName = new CultureInfo("en-US", false).TextInfo;
                string LicenseName = cultInfoLicenseName.ToTitleCase(licenseSite.LicenseName);

                TextInfo cultInfoRemarks = new CultureInfo("en-US", false).TextInfo;
                string Remarks = cultInfoRemarks.ToTitleCase(licenseSite.Remarks);

                licenseSite.LicenseName = LicenseName;
                licenseSite.Remarks = Remarks;
                #endregion

                #region SAVE DATA
                foreach (var licenseFile in LicenseFile)
                {
                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE\\");
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

                        licenseSite.LicenseFileName = fileName + extension;
                    }
                }

                TempData["registerMessage"] = string.Format("{0} has been successfully registered!", licenseSite.LicenseName);
                licenseDbContext.RegisterLicenseSite(licenseSite, UserName);

                return RedirectToAction("Index", "LicenseSite");
                #endregion
            }
        }
        #endregion

        #region RENEWAL
        //[Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [HttpGet]
        public ActionResult Renewal(int id)
        {
            #region VIEW MODEL
            LicenseSite licenseSite = licenseDbContext.GetLicenseSiteByID(id);

            List<LicenseSite> HistorylicenseSite = licenseDbContext.LicenseSiteGetLog(licenseSite.LicenseName).ToList();

            RenewalLicenseSiteViewModel ViewModel = new RenewalLicenseSiteViewModel();

            ViewModel.RenewalLicense = licenseSite;
            ViewModel.History = licenseDbContext.LicenseSiteGetLog(licenseSite.LicenseName).ToList();
            #endregion

            if (ViewModel == null)
            {
                return NotFound();
            }

            #region DROPDOWNLIST
            List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

            //ddlCategory      
            categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
            categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });

            //ddlBusinessDiv      
            businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
            businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });

            //ddlBusinessUnit             
            businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
            businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });

            //ddlPIC2                  
            PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
            PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });

            //ddlPIC3   
            PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
            PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });

            ViewBag.ListofCategory = categoryLicenseSiteList;
            ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;
            ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;
            ViewBag.ListofPIC2 = PIC2LicenseSiteList;
            ViewBag.ListofPIC3 = PIC3LicenseSiteList;
            #endregion

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renewal([Bind] RenewalLicenseSiteViewModel licenseSite, List<IFormFile> LicenseFile)
        {
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                RenewalLicenseSiteViewModel ViewModel = new RenewalLicenseSiteViewModel();

                ViewModel.RenewalLicense = licenseSite.RenewalLicense;
                ViewModel.History = licenseDbContext.LicenseSiteGetLog(licenseSite.RenewalLicense.LicenseName).ToList();

                #region DROPDOWNLIST
                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                //ddlCategory      
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });

                //ddlBusinessDiv      
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });

                //ddlBusinessUnit             
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });

                //ddlPIC2                  
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });

                //ddlPIC3   
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                #endregion

                #region CUSTOM VALIDATION
                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    var extension = Path.GetExtension(licenseFile.FileName);

                    licenseSite.RenewalLicense.NewLicenseFileName = fileName + extension;
                }

                LicenseSite checkLicenseSiteFile = licenseDbContext.CheckLicenseSiteFileByName(licenseSite.RenewalLicense.NewLicenseFileName);

                DateTime issuedDT = Convert.ToDateTime(licenseSite.RenewalLicense.NewIssuedDT);
                DateTime expiredDT = Convert.ToDateTime(licenseSite.RenewalLicense.NewExpiredDT);

                if (string.IsNullOrEmpty(licenseSite.RenewalLicense.NewLicenseFileName))
                {
                    ModelState.AddModelError("", "Please upload License File");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (checkLicenseSiteFile.ExistData == 1)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("License file {0} already existed in BLMS database!", licenseSite.RenewalLicense.NewLicenseFileName));
                }
                else if (string.IsNullOrEmpty(licenseSite.RenewalLicense.NewRegistrationNo))
                {
                    ModelState.AddModelError("", "Please type Registration No");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                }
                else if (licenseSite.RenewalLicense.CategoryID == 0)
                {
                    ModelState.AddModelError("", "Please select License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                }
                else if (licenseSite.RenewalLicense.DivID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Division");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                }
                else if (licenseSite.RenewalLicense.UnitID == 0)
                {
                    ModelState.AddModelError("", "Please select Business Unit");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (licenseSite.RenewalLicense.NewIssuedDT != null && licenseSite.RenewalLicense.NewExpiredDT != null && issuedDT.Date > expiredDT.Date)
                {
                    ModelState.AddModelError("", "Expire Date {1} cannot less than Issue Date {0}");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseSite.RenewalLicense.NewIssuedDT, licenseSite.RenewalLicense.NewExpiredDT));
                }
                else if (string.IsNullOrEmpty(licenseSite.RenewalLicense.PIC1Name))
                {
                    ModelState.AddModelError("", "Login session is expired. Please login again");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Login session is expired. Please login again");
                }
                else if (licenseSite.RenewalLicense.PIC2StaffNo != "-" && licenseSite.RenewalLicense.PIC3StaffNo != "-" && licenseSite.RenewalLicense.PIC2StaffNo == licenseSite.RenewalLicense.PIC3StaffNo)
                {
                    ModelState.AddModelError("", "PIC 2 and PIC 3 contains same Staff Name");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 contains same Staff Name");
                }
                #endregion
                else
                {
                    #region INITIALIZE MODEL DATA
                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewSerialNo))
                    {
                        licenseSite.RenewalLicense.SerialNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewIssuedDT))
                    {
                        licenseSite.RenewalLicense.NewIssuedDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewExpiredDT))
                    {
                        licenseSite.RenewalLicense.NewExpiredDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewRemarks))
                    {
                        licenseSite.RenewalLicense.NewRemarks = "-";
                    }

                    if (LicenseFile.Count == 0)
                    {
                        licenseSite.RenewalLicense.NewLicenseFileName = "-";
                    }
                    #endregion

                    #region CHANGE FIRST LETTER (LOWER TO UPPER)
                    TextInfo cultInfoLicenseName = new CultureInfo("en-US", false).TextInfo;
                    string LicenseName = cultInfoLicenseName.ToTitleCase(licenseSite.RenewalLicense.LicenseName);

                    TextInfo cultInfoRemarks = new CultureInfo("en-US", false).TextInfo;
                    string Remarks = cultInfoRemarks.ToTitleCase(licenseSite.RenewalLicense.NewRemarks);

                    licenseSite.RenewalLicense.LicenseName = LicenseName;
                    licenseSite.RenewalLicense.NewRemarks = Remarks;
                    #endregion

                    #region SAVE DATA
                    foreach (var licenseFile in LicenseFile)
                    {
                        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE\\");
                        bool basePathExists = System.IO.Directory.Exists(basePath);

                        if (!basePathExists) Directory.CreateDirectory(basePath);

                        var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                        var filePath = Path.Combine(basePath, licenseFile.FileName);
                        var extension = Path.GetExtension(licenseFile.FileName);

                        licenseSite.RenewalLicense.NewLicenseFileName = fileName + extension;

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                licenseFile.CopyToAsync(stream);
                            }
                        }
                    }

                    TempData["renewalMessage"] = string.Format("{0} has been successfully renewed!", licenseSite.RenewalLicense.LicenseName);
                    licenseDbContext.RenewalLicenseSite(licenseSite, UserName);

                    return RedirectToAction("Index", "LicenseSite");
                    #endregion
                }

                ViewBag.ListofCategory = categoryLicenseSiteList;
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;

                return View(ViewModel);
            }
            catch
            {
                #region INITIALIZE MODEL DATA
                if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewSerialNo))
                {
                    licenseSite.RenewalLicense.SerialNo = "-";
                }

                if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewIssuedDT))
                {
                    licenseSite.RenewalLicense.NewIssuedDT = "-";
                }

                if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewExpiredDT))
                {
                    licenseSite.RenewalLicense.NewExpiredDT = "-";
                }

                if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewRemarks))
                {
                    licenseSite.RenewalLicense.NewRemarks = "-";
                }

                if (LicenseFile.Count == 0)
                {
                    licenseSite.RenewalLicense.NewLicenseFileName = "-";
                }
                #endregion

                #region CHANGE FIRST LETTER (LOWER TO UPPER)
                TextInfo cultInfoLicenseName = new CultureInfo("en-US", false).TextInfo;
                string LicenseName = cultInfoLicenseName.ToTitleCase(licenseSite.RenewalLicense.LicenseName);

                TextInfo cultInfoRemarks = new CultureInfo("en-US", false).TextInfo;
                string Remarks = cultInfoRemarks.ToTitleCase(licenseSite.RenewalLicense.NewRemarks);

                licenseSite.RenewalLicense.LicenseName = LicenseName;
                licenseSite.RenewalLicense.NewRemarks = Remarks;
                #endregion

                #region SAVE DATA
                foreach (var licenseFile in LicenseFile)
                {
                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE\\");
                    bool basePathExists = System.IO.Directory.Exists(basePath);

                    if (!basePathExists) Directory.CreateDirectory(basePath);

                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    var filePath = Path.Combine(basePath, licenseFile.FileName);
                    var extension = Path.GetExtension(licenseFile.FileName);

                    licenseSite.RenewalLicense.NewLicenseFileName = fileName + extension;

                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            licenseFile.CopyToAsync(stream);
                        }
                    }
                }

                TempData["renewalMessage"] = string.Format("{0} has been successfully renewed!", licenseSite.RenewalLicense.LicenseName);
                licenseDbContext.RenewalLicenseSite(licenseSite, UserName);

                return RedirectToAction("Index", "LicenseSite");
                #endregion
            }
        }
        #endregion

        #region EDIT
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult Edit(int id)
        {
            LicenseSite licenseSite = licenseDbContext.GetLicenseSiteByID(id);

            if (licenseSite == null)
            {
                return NotFound();
            }

            #region DROPDOWNLIST
            List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

            //ddlCategory      
            categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
            categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });

            //ddlBusinessDiv      
            businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
            businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });

            //ddlBusinessUnit             
            businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
            businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });

            //ddlPIC2                  
            PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
            PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });

            //ddlPIC3   
            PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
            PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });

            ViewBag.ListofCategory = categoryLicenseSiteList;
            ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;
            ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;
            ViewBag.ListofPIC2 = PIC2LicenseSiteList;
            ViewBag.ListofPIC3 = PIC3LicenseSiteList;
            #endregion

            return View(licenseSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind] LicenseSite licenseSite, List<IFormFile> LicenseFile)
        {
            string UserName = User.Identity.Name;

            try
            {
                LicenseHQ checkLicense = licenseDbContext.CheckLicenseByName(licenseSite.LicenseName);

                #region DROPDOWN
                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                //ddlCategory      
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please select License Type" });

                //ddlBusinessDiv      
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please select Business Division" });

                //ddlBusinessUnit             
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please select Business Unit" });

                //ddlPIC2                  
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please select PIC 2 Name" });

                //ddlPIC3   
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                #endregion

                DateTime issuedDT = Convert.ToDateTime(licenseSite.IssuedDT);
                DateTime expiredDT = Convert.ToDateTime(licenseSite.ExpiredDT);

                #region CHECK DUPLICATION
                if (checkLicense.ExistData == 1 && licenseSite.OldLicenseName != licenseSite.LicenseName)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseSite.LicenseName));
                }
                #endregion

                else
                {
                    #region GET LICENSE FILENAME
                    if(string.IsNullOrEmpty(licenseSite.LicenseFileName))
                    {
                        foreach (var licenseFile in LicenseFile)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                            licenseSite.LicenseFileName = fileName;
                        }
                    }
                    #endregion

                    #region 5 WARNING
                    if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.CategoryID == 0 && licenseSite.DivID == 0 && licenseSite.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertLicense(Alert.WarningFive, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                    }
                    #endregion

                    else
                    {
                        #region 4 WARNING
                        if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.CategoryID == 0 && licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.CategoryID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Unit");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.DivID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please type Registration No", "Please select Business Division", "Please select Business Unit");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.CategoryID == 0 && licenseSite.DivID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.CategoryID == 0 && licenseSite.DivID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                        }
                        #endregion

                        #region 3 WARNING
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.CategoryID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree , "Please type License Name", "Please type Registration No", "Please select License Type", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please type Registration No", "Please select Business Division", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please type Registration No", "Please select Business Unit", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.CategoryID == 0 && licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Division", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.CategoryID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Unit", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.DivID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select Business Division", "Please select Business Unit", "");
                        }
                        #endregion

                        #region 2 WARNING
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && string.IsNullOrEmpty(licenseSite.RegistrationNo) )
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please type Registration No", "", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.CategoryID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select License Type", "", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select Business Division", "", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName) && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select Business Unit", "", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.CategoryID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Registration No", "Please select License Type", "", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Registration No", "Please select Business Division", "", "");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.RegistrationNo) && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Registration No", "Please select Business Unit", "", "");
                        }
                        else if (licenseSite.CategoryID == 0 && licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Division", "", "");
                        }
                        else if (licenseSite.CategoryID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Unit", "", "");
                        }
                        else if (licenseSite.DivID == 0 && licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                        }
                        #endregion

                        #region 1 WARNING
                        else if (string.IsNullOrEmpty(licenseSite.LicenseFileName))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Upload License File");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.LicenseName))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                        }
                        else if (string.IsNullOrEmpty(licenseSite.RegistrationNo))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                        }
                        else if (licenseSite.CategoryID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                        }
                        else if (licenseSite.DivID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                        }
                        else if (licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                        }
                        else if (licenseSite.IssuedDT != null && licenseSite.ExpiredDT != null && issuedDT.Date > expiredDT.Date)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseSite.IssuedDT, licenseSite.ExpiredDT));
                        }
                        else if (licenseSite.PIC2Name != "Please select PIC 2 Name" && licenseSite.PIC3Name != "Please select PIC 3 Name" && licenseSite.PIC2Name == licenseSite.PIC3Name)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 contains same Staff Name");
                        }
                        #endregion

                        else
                        {
                            #region INITIALIZE MODEL DATA
                            if (String.IsNullOrEmpty(licenseSite.SerialNo))
                            {
                                licenseSite.SerialNo = "-";
                            }

                            if (String.IsNullOrEmpty(licenseSite.IssuedDT))
                            {
                                licenseSite.IssuedDT = "-";
                            }

                            if (String.IsNullOrEmpty(licenseSite.ExpiredDT))
                            {
                                licenseSite.ExpiredDT = "-";
                            }

                            if (String.IsNullOrEmpty(licenseSite.Remarks))
                            {
                                licenseSite.Remarks = "-";
                            }

                            if (LicenseFile.Count == 0 & string.IsNullOrEmpty(licenseSite.LicenseFileName))
                            {
                                licenseSite.LicenseFileName = "-";
                            }
                            #endregion

                            #region SAVE DATA
                            if(LicenseFile.Count != 0)
                            {
                                foreach (var licenseFile in LicenseFile)
                                {
                                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE\\");
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

                                        licenseSite.LicenseFileName = fileName + extension;
                                    }
                                }
                            }

                            licenseDbContext.EditLicenseSite(licenseSite, UserName);
                            TempData["editMessage"] = string.Format("{0} has been successfully edited!", licenseSite.LicenseName);

                            return RedirectToAction("Index", "LicenseSite");
                            #endregion
                        }
                    }
                }

                #region DROPDOWN
                ViewBag.ListofCategory = categoryLicenseSiteList;
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;
                #endregion

                return View(licenseSite);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region EMAIL SERVICE

        #endregion

        #region DOWNLOAD FILE ON SYSTEM
        public async Task<IActionResult> DownloadLicenseFile(int id)
        {
            LicenseSite licenseSite = licenseDbContext.GetLicenseSiteByID(id);

            //check license site is not null
            if (licenseSite == null) return null;

            //get file path
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE");
            var filePath = Path.Combine(basePath, licenseSite.LicenseFileName);

            //get file extension to check mimeType
            var extension = Path.GetExtension(licenseSite.LicenseFileName);
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(licenseSite.LicenseFileName, out string contentType))
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
            return File(memory, FileType, licenseSite.LicenseFileName);
        }
        #endregion

        #region GET LINKED BUSINESS UNIT
        public JsonResult JSONGetUnitName(int DivID)
        {
            DataSet ds = ddlLicenseDBContext.ddlBusinessUnitLinkedDivSite(DivID);
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
