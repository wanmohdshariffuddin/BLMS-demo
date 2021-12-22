using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Email;
using BLMS.Enums;
using BLMS.Models.Admin;
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
    public class LicenseSiteController : Controller
    {
        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();
        readonly ddlLicenseDBContext ddlLicenseDBContext = new ddlLicenseDBContext();
        readonly LicenseSiteEmail licenseSiteEmail = new LicenseSiteEmail();
        readonly LogDBContext logDBContext = new LogDBContext();
        readonly AdminDBContext adminDBContext = new AdminDBContext();

        private IWebHostEnvironment _env;

        public LicenseSiteController(IWebHostEnvironment env)
        {
            _env = env;
        }

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [NoDirectAccess]
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
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [NoDirectAccess]
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
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [NoDirectAccess]
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
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [NoDirectAccess]
        [HttpGet]
        public ActionResult Register()
        {
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
            DateTime issuedDT, ExpiredDT;

            try
            {
                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                #region VALIDATION
                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    licenseSite.LicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseSite.PIC1Name))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                }
                else if (string.IsNullOrEmpty(licenseSite.LicenseFileName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (licenseSite.PIC2StaffNo == licenseSite.PIC3StaffNo && licenseSite.PIC2StaffNo != null && licenseSite.PIC3StaffNo != null)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                }
                else if (string.IsNullOrEmpty(licenseSite.LicenseName))
                {
                    if (string.IsNullOrEmpty(licenseSite.RegistrationNo))
                    {
                        if (licenseSite.CategoryID == 0)
                        {
                            if (licenseSite.DivID == 0)
                            {
                                if (licenseSite.UnitID == 0)
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertLicense(Alert.WarningFive, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                                }
                                else
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division");
                                }
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please type Registration No", "Please select License Type", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please type Registration No", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                    }
                }
                else if (string.IsNullOrEmpty(licenseSite.RegistrationNo))
                {
                    if (licenseSite.CategoryID == 0)
                    {
                        if (licenseSite.DivID == 0)
                        {
                            if (licenseSite.UnitID == 0)
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Registration No", "Please select License Type", "Please select Business Division", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Registration No", "Please select License Type", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                    }
                }
                else if (licenseSite.CategoryID == 0)
                {
                    if (licenseSite.DivID == 0)
                    {
                        if (licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select License Type", "Please select Business Division", "Please select Business Unit", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Division", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                    }
                }
                else if (licenseSite.DivID == 0)
                {
                    if (licenseSite.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (licenseSite.UnitID == 0)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (!string.IsNullOrEmpty(licenseSite.IssuedDT))
                {
                    issuedDT = Convert.ToDateTime(DateTime.ParseExact(licenseSite.IssuedDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    if (!string.IsNullOrEmpty(licenseSite.ExpiredDT))
                    {
                        ExpiredDT = Convert.ToDateTime(DateTime.ParseExact(licenseSite.ExpiredDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                        if (issuedDT.Date > ExpiredDT.Date)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseSite.IssuedDT, licenseSite.ExpiredDT));
                        }
                    }
                }
                #endregion

                if(string.IsNullOrEmpty(ViewBag.Alert))
                {
                    #region CHECK EXIST DATA
                    LicenseSite checkLicenseSite = licenseDbContext.CheckLicenseSiteByName(licenseSite.LicenseName);

                    if (checkLicenseSite.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseSite.LicenseName));
                    }
                    #endregion

                    #region CONVERT NULL DATA
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

                    if (String.IsNullOrEmpty(licenseSite.PIC2StaffNo))
                    {
                        licenseSite.PIC2StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.PIC3StaffNo))
                    {
                        licenseSite.PIC3StaffNo = "-";
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

                    #region SAVE FILE
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
                    #endregion

                    #region SAVE DATA
                    licenseDbContext.RegisterLicenseSite(licenseSite, UserName);
                    #endregion

                    #region EMAIL
                    BusinessUnit businessUnit = adminDBContext.GetBusinessUnitByID(licenseSite.UnitID);
                    Category category = adminDBContext.GetCategoryByID(licenseSite.CategoryID);
                    PIC pic = adminDBContext.GetPICByName(licenseSite.PIC1Name);

                    var webRoot = _env.WebRootPath; //get wwwroot Folder

                    //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                    var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Email"
                            + Path.DirectorySeparatorChar.ToString()
                            + "License_Registration_Summary.html";

                    if (string.IsNullOrEmpty(pic.PICEmail))
                    {
                        pic.PICEmail = "-";
                    }

                    licenseSiteEmail.RegisterLicense(licenseSite.LicenseName, businessUnit.UnitName, category.CategoryName, licenseSite.PIC1Name, pic.PICEmail, pathToFile);
                    #endregion

                    TempData["registerMessage"] = string.Format("{0} has been successfully registered!", licenseSite.LicenseName);

                    return RedirectToAction("Index", "LicenseSite");
                }

                #region DROPDOWN
                //ddlCategory
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please Select Type of License" });
                ViewBag.ListofCategory = categoryLicenseSiteList;

                //ddlBusinessDiv
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please Select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;

                //ddlBusinessUnit
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please Select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;

                //ddlPIC2
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;

                //ddlPIC3
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;
                #endregion

                return View(licenseSite);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "SITE USER";

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

        #region RENEWAL
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [NoDirectAccess]
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
            DateTime issuedDT, ExpiredDT;

            try
            {
                RenewalLicenseSiteViewModel ViewModel = new RenewalLicenseSiteViewModel();

                ViewModel.RenewalLicense = licenseSite.RenewalLicense;
                ViewModel.History = licenseDbContext.LicenseSiteGetLog(licenseSite.RenewalLicense.LicenseName).ToList();

                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                #region VALIDATION
                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    licenseSite.RenewalLicense.LicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseSite.RenewalLicense.PIC1Name))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                }
                else if (string.IsNullOrEmpty(licenseSite.RenewalLicense.LicenseFileName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (licenseSite.RenewalLicense.PIC2StaffNo == licenseSite.RenewalLicense.PIC3StaffNo && licenseSite.RenewalLicense.PIC2StaffNo != null && licenseSite.RenewalLicense.PIC3StaffNo != null)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                }
                else if (string.IsNullOrEmpty(licenseSite.RenewalLicense.LicenseName))
                {
                    if (string.IsNullOrEmpty(licenseSite.RenewalLicense.NewRegistrationNo))
                    {
                        if (licenseSite.RenewalLicense.CategoryID == 0)
                        {
                            if (licenseSite.RenewalLicense.DivID == 0)
                            {
                                if (licenseSite.RenewalLicense.UnitID == 0)
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertLicense(Alert.WarningFive, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                                }
                                else
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division");
                                }
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please type Registration No", "Please select License Type", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please type Registration No", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                    }
                }
                else if (string.IsNullOrEmpty(licenseSite.RenewalLicense.NewRegistrationNo))
                {
                    if (licenseSite.RenewalLicense.CategoryID == 0)
                    {
                        if (licenseSite.RenewalLicense.DivID == 0)
                        {
                            if (licenseSite.RenewalLicense.UnitID == 0)
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Registration No", "Please select License Type", "Please select Business Division", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Registration No", "Please select License Type", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                    }
                }
                else if (licenseSite.RenewalLicense.CategoryID == 0)
                {
                    if (licenseSite.RenewalLicense.DivID == 0)
                    {
                        if (licenseSite.RenewalLicense.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select License Type", "Please select Business Division", "Please select Business Unit", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Division", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                    }
                }
                else if (licenseSite.RenewalLicense.DivID == 0)
                {
                    if (licenseSite.RenewalLicense.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (licenseSite.RenewalLicense.UnitID == 0)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (!string.IsNullOrEmpty(licenseSite.RenewalLicense.NewIssuedDT))
                {
                    issuedDT = Convert.ToDateTime(DateTime.ParseExact(licenseSite.RenewalLicense.NewIssuedDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    if (!string.IsNullOrEmpty(licenseSite.RenewalLicense.NewExpiredDT))
                    {
                        ExpiredDT = Convert.ToDateTime(DateTime.ParseExact(licenseSite.RenewalLicense.NewExpiredDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                        if (issuedDT.Date > ExpiredDT.Date)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseSite.RenewalLicense.NewIssuedDT, licenseSite.RenewalLicense.NewExpiredDT));
                        }
                    }
                }
                #endregion

                if(string.IsNullOrEmpty(ViewBag.Alert))
                {
                    #region CONVERT NULL DATA
                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewSerialNo))
                    {
                        licenseSite.RenewalLicense.NewSerialNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewIssuedDT))
                    {
                        licenseSite.RenewalLicense.NewIssuedDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.NewExpiredDT))
                    {
                        licenseSite.RenewalLicense.NewExpiredDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.PIC2StaffNo))
                    {
                        licenseSite.RenewalLicense.PIC2StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseSite.RenewalLicense.PIC3StaffNo))
                    {
                        licenseSite.RenewalLicense.PIC3StaffNo = "-";
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

                    #region SAVE FILE
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
                    #endregion

                    #region SAVE DATA
                    licenseDbContext.RenewalLicenseSite(licenseSite, UserName);

                    #region EMAIL
                    BusinessUnit businessUnit = adminDBContext.GetBusinessUnitByID(licenseSite.RenewalLicense.UnitID);
                    Category category = adminDBContext.GetCategoryByID(licenseSite.RenewalLicense.CategoryID);
                    PIC pic = adminDBContext.GetPICByName(licenseSite.RenewalLicense.PIC1Name);

                    var webRoot = _env.WebRootPath; //get wwwroot Folder

                    //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                    var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Email"
                            + Path.DirectorySeparatorChar.ToString()
                            + "License_Renewal_Summary.html";

                    if (string.IsNullOrEmpty(pic.PICEmail))
                    {
                        pic.PICEmail = "-";
                    }

                    licenseSiteEmail.RenewalLicense(licenseSite.RenewalLicense.LicenseName, businessUnit.UnitName, category.CategoryName, licenseSite.RenewalLicense.PIC1Name, pic.PICEmail, pathToFile);
                    #endregion

                    TempData["renewalMessage"] = string.Format("{0} has been successfully renewed!", licenseSite.RenewalLicense.LicenseName);
                    return RedirectToAction("Index", "LicenseSite");
                    #endregion
                }

                #region DROPDOWN
                //ddlCategory
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please Select Type of License" });
                ViewBag.ListofCategory = categoryLicenseSiteList;

                //ddlBusinessDiv
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please Select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;

                //ddlBusinessUnit
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please Select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;

                //ddlPIC2
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;

                //ddlPIC3
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;
                #endregion

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "SITE USER";

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
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.SITE)]
        [NoDirectAccess]
        public ActionResult Edit(int id)
        {
            LicenseSite licenseSite = licenseDbContext.GetLicenseSiteByID(id);

            if (licenseSite == null)
            {
                return NotFound();
            }

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

                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                #region CHECK DUPLICATION
                if (checkLicense.ExistData == 1 && licenseSite.OldLicenseName != licenseSite.LicenseName)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseSite.LicenseName));
                }
                #endregion

                else
                {
                    #region VALIDATION
                    DateTime issuedDT = Convert.ToDateTime(licenseSite.IssuedDT);
                    DateTime expiredDT = Convert.ToDateTime(licenseSite.ExpiredDT);

                    foreach (var licenseFile in LicenseFile)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                        licenseSite.LicenseFileName = fileName;
                    }

                    if (string.IsNullOrEmpty(licenseSite.PIC1Name))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                    }
                    else if (string.IsNullOrEmpty(licenseSite.LicenseFileName))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                    }
                    else if (licenseSite.PIC2StaffNo == licenseSite.PIC3StaffNo && licenseSite.PIC2StaffNo != null && licenseSite.PIC3StaffNo != null)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                    }
                    else if (string.IsNullOrEmpty(licenseSite.LicenseName))
                    {
                        if (string.IsNullOrEmpty(licenseSite.RegistrationNo))
                        {
                            if (licenseSite.CategoryID == 0)
                            {
                                if (licenseSite.DivID == 0)
                                {
                                    if (licenseSite.UnitID == 0)
                                    {
                                        ViewBag.Alert = AlertNotification.ShowAlertLicense(Alert.WarningFive, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                                    }
                                    else
                                    {
                                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please type Registration No", "Please select License Type", "Please select Business Division");
                                    }
                                }
                                else
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please type Registration No", "Please select License Type", "");
                                }
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please type Registration No", "", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                        }
                    }
                    else if (string.IsNullOrEmpty(licenseSite.RegistrationNo))
                    {
                        if (licenseSite.CategoryID == 0)
                        {
                            if (licenseSite.DivID == 0)
                            {
                                if (licenseSite.UnitID == 0)
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type Registration No", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                                }
                                else
                                {
                                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type Registration No", "Please select License Type", "Please select Business Division", "");
                                }
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type Registration No", "Please select License Type", "", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                        }
                    }
                    else if (licenseSite.CategoryID == 0)
                    {
                        if (licenseSite.DivID == 0)
                        {
                            if (licenseSite.UnitID == 0)
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please select License Type", "Please select Business Division", "Please select Business Unit", "");
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select License Type", "Please select Business Division", "", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select License Type");
                        }
                    }
                    else if (licenseSite.DivID == 0)
                    {
                        if (licenseSite.UnitID == 0)
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                        }
                    }
                    else if (licenseSite.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                    }
                    else if (licenseSite.IssuedDT != null && licenseSite.ExpiredDT != null && issuedDT.Date > expiredDT.Date)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseSite.IssuedDT, licenseSite.ExpiredDT));
                    }
                    #endregion

                    else
                    {
                        #region CONVERT NULL DATA
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

                        if (String.IsNullOrEmpty(licenseSite.PIC2StaffNo))
                        {
                            licenseSite.PIC2StaffNo = "-";
                        }

                        if (String.IsNullOrEmpty(licenseSite.PIC3StaffNo))
                        {
                            licenseSite.PIC3StaffNo = "-";
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

                        #region SAVE FILE
                        if (LicenseFile.Count != 0)
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
                        #endregion

                        #region SAVE DATA
                        licenseDbContext.EditLicenseSite(licenseSite, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", licenseSite.LicenseName);

                        return RedirectToAction("Index", "LicenseSite");
                        #endregion
                    }
                }

                #region DROPDOWN
                //ddlCategory
                categoryLicenseSiteList = ddlLicenseDBContext.ddlCategoryLicenseSite().ToList();
                categoryLicenseSiteList.Insert(0, new LicenseSite { CategoryID = 0, CategoryName = "Please Select Type of License" });
                ViewBag.ListofCategory = categoryLicenseSiteList;

                //ddlBusinessDiv
                businessDivLicenseSiteList = ddlLicenseDBContext.ddlBusinessDivLicenseSite().ToList();
                businessDivLicenseSiteList.Insert(0, new LicenseSite { DivID = 0, DivName = "Please Select Business Division" });
                ViewBag.ListofBusinessDiv = businessDivLicenseSiteList;

                //ddlBusinessUnit
                businessUnitLicenseSiteList = ddlLicenseDBContext.ddlBusinessUnitLicenseSite().ToList();
                businessUnitLicenseSiteList.Insert(0, new LicenseSite { UnitID = 0, UnitName = "Please Select Business Unit" });
                ViewBag.ListofBusinessUnit = businessUnitLicenseSiteList;

                //ddlPIC2
                PIC2LicenseSiteList = ddlLicenseDBContext.ddlPIC2LicenseSite().ToList();
                PIC2LicenseSiteList.Insert(0, new LicenseSite { PIC2StaffNo = "-", PIC2Name = "Please Select PIC 2 Name" });
                ViewBag.ListofPIC2 = PIC2LicenseSiteList;

                //ddlPIC3
                PIC3LicenseSiteList = ddlLicenseDBContext.ddlPIC3LicenseSite().ToList();
                PIC3LicenseSiteList.Insert(0, new LicenseSite { PIC3StaffNo = "-", PIC3Name = "Please Select PIC 3 Name" });
                ViewBag.ListofPIC3 = PIC3LicenseSiteList;
                #endregion

                return View(licenseSite);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "SITE USER";

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
