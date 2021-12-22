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
    public class LicenseAdminController : Controller
    {
        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();
        readonly ddlLicenseDBContext ddlLicenseDBContext = new ddlLicenseDBContext();
        readonly LicenseAdminEmail licenseAdminEmail = new LicenseAdminEmail();
        readonly LogDBContext logDBContext = new LogDBContext();
        readonly AdminDBContext adminDBContext = new AdminDBContext();

        private IWebHostEnvironment _env;

        public LicenseAdminController(IWebHostEnvironment env)
        {
            _env = env;
        }

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
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
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
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
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
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
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult DetailRenewal(int id)
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
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult EditRequest(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            #region DROPDOWN
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
            #endregion

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

                #region VALIDATION
                if (licenseAdmin.PIC2StaffNo == licenseAdmin.PIC3StaffNo && licenseAdmin.PIC2StaffNo != "-" && licenseAdmin.PIC3StaffNo != "-")
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName))
                {
                    if (licenseAdmin.CategoryID == 0)
                    {
                        if (licenseAdmin.DivID == 0)
                        {
                            if (licenseAdmin.UnitID == 0)
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please type License Name", "Please select License Type", "Please select Business Division", "Please select Business Unit");
                            }
                            else
                            {
                                ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please type License Name", "Please select License Type", "Please select Business Division", "");
                            }
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Name", "Please select License Type", "", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Name");
                    }
                }
                else if (licenseAdmin.CategoryID == 0)
                {
                    if (licenseAdmin.DivID == 0)
                    {
                        if (licenseAdmin.UnitID == 0)
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
                else if (licenseAdmin.DivID == 0)
                {
                    if (licenseAdmin.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (licenseAdmin.UnitID == 0)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                #endregion

                else
                {
                    #region CONVERT NULL DATA
                    if (String.IsNullOrEmpty(licenseAdmin.Remarks))
                    {
                        licenseAdmin.Remarks = "-";
                    }
                    #endregion

                    #region CHECK DUPLICATION
                    LicenseHQ checkLicense = licenseDbContext.CheckLicenseByName(licenseAdmin.LicenseName);

                    if (checkLicense.ExistData == 1 && licenseAdmin.OldLicenseName != licenseAdmin.LicenseName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseAdmin.LicenseName));
                    }
                    #endregion

                    else
                    {
                        licenseDbContext.EditLicenseHQRequest(licenseAdmin, UserName);
                        TempData["EditMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);
                        return RedirectToAction("Index");
                    }
                }

                #region DROPDOWN
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
                #endregion

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
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult EditRegister(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            #region FIX ISSUE DATE IN ISSUED DATE & EXPIRED DATE
            if (!string.IsNullOrEmpty(licenseAdmin.IssuedDT))
            {
                string FirstIssuedDT = licenseAdmin.IssuedDT.Substring(0, 1);
                string SecondIssuedDT = licenseAdmin.IssuedDT.Substring(1, 1);

                if (licenseAdmin.IssuedDT.Length < 10 && FirstIssuedDT != "0" && SecondIssuedDT == "/")
                {
                    string IssuedDT = licenseAdmin.IssuedDT;
                    string adjIssuedDT = IssuedDT.Insert(0, "0");
                    licenseAdmin.IssuedDT = adjIssuedDT;
                }
            }

            if (!string.IsNullOrEmpty(licenseAdmin.ExpiredDT))
            {
                string FirstExpiredDT = licenseAdmin.ExpiredDT.Substring(0, 1);
                string SecondExpiredDT = licenseAdmin.ExpiredDT.Substring(1, 1);

                if (licenseAdmin.ExpiredDT.Length < 10 && FirstExpiredDT != "0" && SecondExpiredDT == "/")
                {
                    string ExpiredDT = licenseAdmin.ExpiredDT;
                    string adjExpiredDT = ExpiredDT.Insert(0, "0");
                    licenseAdmin.ExpiredDT = adjExpiredDT;
                }
            }

            if (!string.IsNullOrEmpty(licenseAdmin.IssuedDT))
            {
                string ThirdIssuedDT = licenseAdmin.IssuedDT.Substring(2, 1);
                string SixthIssuedDT = licenseAdmin.IssuedDT.Substring(5, 1);

                if (licenseAdmin.IssuedDT.Length < 10 && ThirdIssuedDT == "/" && SixthIssuedDT != "/")
                {
                    string adjIssuedDT2 = licenseAdmin.IssuedDT.Insert(3, "0");
                    licenseAdmin.IssuedDT = adjIssuedDT2;
                }
            }

            if (!string.IsNullOrEmpty(licenseAdmin.ExpiredDT))
            {
                string ThirdExpiredDT = licenseAdmin.ExpiredDT.Substring(2, 1);
                string SixthExpiredDT = licenseAdmin.ExpiredDT.Substring(5, 1);

                if (licenseAdmin.ExpiredDT.Length < 10 && ThirdExpiredDT == "/" && SixthExpiredDT != "/")
                {
                    string adjExpiredDT2 = licenseAdmin.ExpiredDT.Insert(3, "0");
                    licenseAdmin.ExpiredDT = adjExpiredDT2;
                }
            }
            #endregion

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
        public ActionResult EditRegister([Bind] LicenseAdmin licenseAdmin, List<IFormFile> LicenseFile)
        {
            string UserName = User.Identity.Name;
            DateTime issuedDT, ExpiredDT;

            try
            {
                List<LicenseHQ> categoryLicenseHQList, businessDivLicenseHQList, businessUnitLicenseHQList, PIC2LicenseHQList, PIC3LicenseHQList;

                #region VALIDATION
                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    licenseAdmin.LicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseAdmin.PIC1Name))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseFileName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (licenseAdmin.PIC2StaffNo == licenseAdmin.PIC3StaffNo && licenseAdmin.PIC2StaffNo != null && licenseAdmin.PIC3StaffNo != null)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName))
                {
                    if (string.IsNullOrEmpty(licenseAdmin.RegistrationNo))
                    {
                        if (licenseAdmin.CategoryID == 0)
                        {
                            if (licenseAdmin.DivID == 0)
                            {
                                if (licenseAdmin.UnitID == 0)
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
                else if (string.IsNullOrEmpty(licenseAdmin.RegistrationNo))
                {
                    if (licenseAdmin.CategoryID == 0)
                    {
                        if (licenseAdmin.DivID == 0)
                        {
                            if (licenseAdmin.UnitID == 0)
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
                else if (licenseAdmin.CategoryID == 0)
                {
                    if (licenseAdmin.DivID == 0)
                    {
                        if (licenseAdmin.UnitID == 0)
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
                else if (licenseAdmin.DivID == 0)
                {
                    if (licenseAdmin.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (licenseAdmin.UnitID == 0)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.IssuedDT))
                {
                    if (!string.IsNullOrEmpty(licenseAdmin.ExpiredDT))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Issued Date if want to select Expired Date");
                    }
                }
                else if (!string.IsNullOrEmpty(licenseAdmin.IssuedDT) && !string.IsNullOrEmpty(licenseAdmin.ExpiredDT))
                {
                    issuedDT = Convert.ToDateTime(DateTime.ParseExact(licenseAdmin.IssuedDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    ExpiredDT = Convert.ToDateTime(DateTime.ParseExact(licenseAdmin.ExpiredDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    if (issuedDT.Date > ExpiredDT.Date)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseAdmin.IssuedDT, licenseAdmin.ExpiredDT));
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(ViewBag.Alert))
                {
                    #region CONVERT NULL DATA
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

                    if (String.IsNullOrEmpty(licenseAdmin.PIC2StaffNo))
                    {
                        licenseAdmin.PIC2StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.PIC3StaffNo))
                    {
                        licenseAdmin.PIC3StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.Remarks))
                    {
                        licenseAdmin.Remarks = "-";
                    }

                    if (LicenseFile.Count == 0)
                    {
                        licenseAdmin.LicenseFileName = "-";
                    }
                    #endregion

                    #region CHECK DUPLICATION LICENSE NAME
                    LicenseHQ checkLicense = licenseDbContext.CheckLicenseByName(licenseAdmin.LicenseName);

                    if (checkLicense.ExistData == 1 && licenseAdmin.OldLicenseName != licenseAdmin.LicenseName)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseAdmin.LicenseName));
                    }
                    #endregion

                    #region SAVE FILE
                    if (LicenseFile.Count != 0)
                    {
                        foreach (var licenseFile in LicenseFile)
                        {
                            if (licenseAdmin.UserType == "SITE")
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

                                    licenseAdmin.LicenseFileName = fileName + extension;
                                }
                            }
                            else
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
                        }
                    }
                    #endregion

                    #region SAVE DATA
                    licenseDbContext.EditLicenseHQRegister(licenseAdmin, UserName);

                    TempData["EditMessage"] = string.Format("{0} has been successfully edited!", licenseAdmin.LicenseName);

                    return RedirectToAction("Index");
                    #endregion
                }

                #region DROPDOWN
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
                #endregion

                return View(licenseAdmin);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "APPLICATION TRACKING";

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

        #region REGISTER LICENSE HQ
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        [HttpGet]
        public ActionResult RegisterLicenseHQ(int id)
        {
            LicenseAdmin licenseAdmin = licenseDbContext.GetLicenseAdminByID(id);

            if (licenseAdmin == null)
            {
                return NotFound();
            }

            #region DROPDOWN
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
            #endregion

            return View(licenseAdmin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterLicenseHQ([Bind] LicenseAdmin licenseAdmin, List<IFormFile> LicenseFile)
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
                    licenseAdmin.LicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseAdmin.PIC1Name))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseFileName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (licenseAdmin.PIC2StaffNo != "-" && licenseAdmin.PIC3StaffNo != "-")
                {
                    if (licenseAdmin.PIC2StaffNo == licenseAdmin.PIC3StaffNo && licenseAdmin.PIC2StaffNo != null && licenseAdmin.PIC3StaffNo != null)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                    }
                }
                else if (string.IsNullOrEmpty(licenseAdmin.LicenseName))
                {
                    if (string.IsNullOrEmpty(licenseAdmin.RegistrationNo))
                    {
                        if (licenseAdmin.CategoryID == 0)
                        {
                            if (licenseAdmin.DivID == 0)
                            {
                                if (licenseAdmin.UnitID == 0)
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
                else if (string.IsNullOrEmpty(licenseAdmin.RegistrationNo))
                {
                    if (licenseAdmin.CategoryID == 0)
                    {
                        if (licenseAdmin.DivID == 0)
                        {
                            if (licenseAdmin.UnitID == 0)
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
                else if (licenseAdmin.CategoryID == 0)
                {
                    if (licenseAdmin.DivID == 0)
                    {
                        if (licenseAdmin.UnitID == 0)
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
                else if (licenseAdmin.DivID == 0)
                {
                    if (licenseAdmin.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (licenseAdmin.UnitID == 0)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.IssuedDT))
                {
                    if (!string.IsNullOrEmpty(licenseAdmin.ExpiredDT))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Issued Date if want to select Expired Date");
                    }
                }
                else if (!string.IsNullOrEmpty(licenseAdmin.IssuedDT) && !string.IsNullOrEmpty(licenseAdmin.ExpiredDT))
                {
                    issuedDT = Convert.ToDateTime(DateTime.ParseExact(licenseAdmin.IssuedDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    ExpiredDT = Convert.ToDateTime(DateTime.ParseExact(licenseAdmin.ExpiredDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    if (issuedDT.Date > ExpiredDT.Date)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseAdmin.IssuedDT, licenseAdmin.ExpiredDT));
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(ViewBag.Alert))
                {
                    #region CONVERT NULL DATA
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

                    if (String.IsNullOrEmpty(licenseAdmin.PIC2StaffNo))
                    {
                        licenseAdmin.PIC2StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.PIC3StaffNo))
                    {
                        licenseAdmin.PIC3StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.Remarks))
                    {
                        licenseAdmin.Remarks = "-";
                    }

                    if (LicenseFile.Count == 0)
                    {
                        licenseAdmin.LicenseFileName = "-";
                    }
                    #endregion

                    #region SAVE FILE
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
                    #endregion

                    #region SAVE DATA
                    licenseDbContext.RegisterLicenseHQ(licenseAdmin, UserName);
                    #endregion

                    #region EMAIL
                    BusinessUnit businessUnit = adminDBContext.GetBusinessUnitByID(licenseAdmin.UnitID);
                    Category category = adminDBContext.GetCategoryByID(licenseAdmin.CategoryID);
                    PIC pic = adminDBContext.GetPICByName(licenseAdmin.PIC1Name);

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

                    licenseAdminEmail.RegisterLicense(licenseAdmin.LicenseName, businessUnit.UnitName, category.CategoryName, licenseAdmin.PIC1Name, pic.PICEmail, pathToFile);
                    #endregion

                    TempData["registerMessage"] = string.Format("{0} has been successfully registered!", licenseAdmin.LicenseName);

                    return RedirectToAction("Index", "LicenseAdmin");
                }

                #region DROPDOWN
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
                #endregion

                return View(licenseAdmin);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "APPLICATION TRACKING";

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

        #region RENEW LICENSE HQ
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
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

            #region DROPDOWN
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
            #endregion

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RenewalLicenseHQAsync([Bind] RenewalLicenseHQViewModel licenseAdmin, List<IFormFile> LicenseFile)
        {
            string UserName = HttpContext.User.Identity.Name;
            DateTime issuedDT, ExpiredDT;

            try
            {
                RenewalLicenseHQViewModel ViewModel = new RenewalLicenseHQViewModel();

                ViewModel.RenewalLicense = licenseAdmin.RenewalLicense;
                ViewModel.History = licenseDbContext.LicenseAdminGetLog(licenseAdmin.RenewalLicense.LicenseName).ToList();

                List<LicenseSite> categoryLicenseSiteList, businessDivLicenseSiteList, businessUnitLicenseSiteList, PIC2LicenseSiteList, PIC3LicenseSiteList;

                #region VALIDATION
                //GET LICENSE NAME
                foreach (var licenseFile in LicenseFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(licenseFile.FileName);
                    licenseAdmin.RenewalLicense.NewLicenseFileName = fileName;
                }

                if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.PIC1Name)) //CHECK PIC 1 STILL ACTIVE
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewLicenseFileName)) //CHECK LICENSE FILE NOT EMPTY
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload License File");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewRegistrationNo)) //CHECK REG NO NOT EMPTY
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Registration No");
                }
                else if (string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewIssuedDT))
                {
                    if (!string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewExpiredDT))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Issued Date if want to select Expired Date");
                    }
                }
                else if (!string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewIssuedDT) && !string.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewExpiredDT))
                {
                    issuedDT = Convert.ToDateTime(DateTime.ParseExact(licenseAdmin.RenewalLicense.NewIssuedDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    ExpiredDT = Convert.ToDateTime(DateTime.ParseExact(licenseAdmin.RenewalLicense.NewExpiredDT, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    if (issuedDT.Date > ExpiredDT.Date)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, string.Format("Expire Date {1} cannot less than Issue Date {0}", licenseAdmin.RenewalLicense.NewIssuedDT, licenseAdmin.RenewalLicense.NewExpiredDT));
                    }
                }
                else if (!string.IsNullOrEmpty(licenseAdmin.RenewalLicense.PIC2Name) && !string.IsNullOrEmpty(licenseAdmin.RenewalLicense.PIC3Name)) //COMPARE PIC 2 & PIC 3
                {
                    if (licenseAdmin.RenewalLicense.PIC2Name == licenseAdmin.RenewalLicense.PIC3Name)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 cannot as same as PIC 3");
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(ViewBag.Alert))
                {
                    #region CONVERT NULL DATA
                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewSerialNo))
                    {
                        licenseAdmin.RenewalLicense.NewSerialNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewIssuedDT))
                    {
                        licenseAdmin.RenewalLicense.NewIssuedDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewExpiredDT))
                    {
                        licenseAdmin.RenewalLicense.NewExpiredDT = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.PIC2Name))
                    {
                        licenseAdmin.RenewalLicense.PIC2Name = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.PIC3Name))
                    {
                        licenseAdmin.RenewalLicense.PIC3Name = "-";
                    }

                    if (String.IsNullOrEmpty(licenseAdmin.RenewalLicense.NewRemarks))
                    {
                        licenseAdmin.RenewalLicense.NewRemarks = "-";
                    }

                    if (LicenseFile.Count == 0)
                    {
                        licenseAdmin.RenewalLicense.NewLicenseFileName = "-";
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

                        licenseAdmin.RenewalLicense.NewLicenseFileName = fileName + extension;

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await licenseFile.CopyToAsync(stream);
                            }
                        }
                    }
                    #endregion

                    #region SAVE DATA
                    licenseDbContext.RenewalLicenseHQ(licenseAdmin, UserName);

                    #region EMAIL
                    PIC pic = adminDBContext.GetPICByName(licenseAdmin.RenewalLicense.PIC1Name);

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

                    licenseAdminEmail.RenewalLicense(licenseAdmin.RenewalLicense.LicenseName, licenseAdmin.RenewalLicense.UnitName, licenseAdmin.RenewalLicense.CategoryName, licenseAdmin.RenewalLicense.PIC1Name, pic.PICEmail, pathToFile);
                    #endregion

                    TempData["renewalMessage"] = string.Format("{0} has been successfully renewed!", licenseAdmin.RenewalLicense.LicenseName);
                    return RedirectToAction("Index", "LicenseAdmin");
                    #endregion
                }

                #region DROPDOWN
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
                #endregion

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "APPLICATION TRACKING";

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
