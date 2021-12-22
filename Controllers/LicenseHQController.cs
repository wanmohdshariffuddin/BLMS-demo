using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Email;
using BLMS.Enums;
using BLMS.Models.Admin;
using BLMS.Models.License;
using BLMS.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class LicenseHQController : Controller
    {
        private IWebHostEnvironment _env;

        public LicenseHQController(IWebHostEnvironment env)
        {
            _env = env;
        }

        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();
        readonly ddlLicenseDBContext ddlLicenseDBContext = new ddlLicenseDBContext();
        readonly LicenseHQEmail licenseHQEmail = new LicenseHQEmail();
        readonly LogDBContext logDBContext = new LogDBContext();
        readonly AdminDBContext adminDBContext = new AdminDBContext();

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        [NoDirectAccess]
        public IActionResult Index()
        {
            List<LicenseHQ> licenseHQList = licenseDbContext.LicenseHQGetAll().ToList();

            if (TempData["requestLicenseMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["requestLicenseMessage"].ToString());
            }
            else if (TempData["requestRenewalMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["requestRenewalMessage"].ToString());
            }

            return View(licenseHQList);
        }
        #endregion

        #region VIEW REQUEST LICENSE
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        [NoDirectAccess]
        public ActionResult DetailRequest(int id)
        {
            LicenseHQ licenseHQ = licenseDbContext.GetLicenseHQByID(id);

            if (licenseHQ == null)
            {
                return NotFound();
            }

            return View(licenseHQ);
        }
        #endregion

        #region VIEW REGISTER LICENSE
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        [NoDirectAccess]
        public ActionResult DetailRegister(int id)
        {
            LicenseHQ licenseHQ = licenseDbContext.GetLicenseHQByID(id);

            if (licenseHQ == null)
            {
                return NotFound();
            }

            return View(licenseHQ);
        }
        #endregion

        #region VIEW RENEWAL LICENSE
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        [NoDirectAccess]
        public ActionResult DetailRenewal(int id)
        {
            LicenseHQ licenseHQ = licenseDbContext.GetLicenseHQByID(id);

            List<LicenseHQ> HistorylicenseHQ = licenseDbContext.LicenseHQGetLog(licenseHQ.LicenseName).ToList();

            LicenseHQRenewal ViewModel = new LicenseHQRenewal();

            ViewModel.RenewalLicense = licenseHQ;
            ViewModel.LicenseLog = licenseDbContext.LicenseHQGetLog(licenseHQ.LicenseName).ToList();

            if (ViewModel == null)
            {
                return NotFound();
            }

            return View(ViewModel);
        }
        #endregion

        #region REQUEST
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        [NoDirectAccess]
        public ActionResult RequestLicense()
        {
            List<LicenseHQ> categoryLicenseHQList, businessDivLicenseHQList, businessUnitLicenseHQList, PIC2LicenseHQList, PIC3LicenseHQList;

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

            return View();
        }

        // POST: LicenseHQController/Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestLicense([Bind] LicenseHQ licenseHQ)
        {
            string UserName = User.Identity.Name;

            try
            {
                List<LicenseHQ> categoryLicenseHQList, businessDivLicenseHQList, businessUnitLicenseHQList, PIC2LicenseHQList, PIC3LicenseHQList;

                #region VALIDATION
                if (string.IsNullOrEmpty(licenseHQ.PIC1Name))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Your session has been expired. Please login again.");
                }
                else if (licenseHQ.PIC2StaffNo == licenseHQ.PIC3StaffNo && !string.IsNullOrEmpty(licenseHQ.PIC2StaffNo) && !string.IsNullOrEmpty(licenseHQ.PIC3StaffNo))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PIC 2 and PIC 3 has same staff name.");
                }
                else if (string.IsNullOrEmpty(licenseHQ.LicenseName))
                {
                    if (licenseHQ.CategoryID == 0)
                    {
                        if (licenseHQ.DivID == 0)
                        {
                            if (licenseHQ.UnitID == 0)
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
                else if (licenseHQ.CategoryID == 0)
                {
                    if (licenseHQ.DivID == 0)
                    {
                        if (licenseHQ.UnitID == 0)
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
                else if (licenseHQ.DivID == 0)
                {
                    if (licenseHQ.UnitID == 0)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please select Business Division", "Please select Business Unit", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Division");
                    }
                }
                else if (licenseHQ.UnitID == 0)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please select Business Unit");
                }
                #endregion

                else
                {
                    #region CONVERT NULL REMARKS TO "-"
                    if (String.IsNullOrEmpty(licenseHQ.Remarks))
                    {
                        licenseHQ.Remarks = "-";
                    }

                    if (String.IsNullOrEmpty(licenseHQ.PIC2StaffNo))
                    {
                        licenseHQ.PIC2StaffNo = "-";
                    }

                    if (String.IsNullOrEmpty(licenseHQ.PIC3StaffNo))
                    {
                        licenseHQ.PIC3StaffNo = "-";
                    }
                    #endregion

                    LicenseHQ checkLicense = licenseDbContext.CheckLicenseByName(licenseHQ.LicenseName);

                    if (checkLicense.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", licenseHQ.LicenseName));
                    }
                    else
                    {
                        licenseDbContext.RequestLicenseHQ(licenseHQ, UserName);

                        #region EMAIL
                        BusinessUnit businessUnit = adminDBContext.GetBusinessUnitByID(licenseHQ.UnitID);
                        Category category = adminDBContext.GetCategoryByID(licenseHQ.CategoryID);
                        PIC pic = adminDBContext.GetPICByName(licenseHQ.PIC1Name);

                        var webRoot = _env.WebRootPath; //get wwwroot Folder

                        //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                        var pathToFile = _env.WebRootPath
                                + Path.DirectorySeparatorChar.ToString()
                                + "Templates"
                                + Path.DirectorySeparatorChar.ToString()
                                + "Email"
                                + Path.DirectorySeparatorChar.ToString()
                                + "License_Request_Summary.html";

                        licenseHQEmail.RequestLicense(licenseHQ, businessUnit.UnitName, category.CategoryName, pic.PICEmail, pathToFile);
                        #endregion

                        TempData["requestLicenseMessage"] = string.Format("{0} has been successfully requested!", licenseHQ.LicenseName);
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

                return View(licenseHQ);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "HQ USER";

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

        #region EMAIL REQUEST RENEWAL LICENSE
        [Authorize(Roles.ADMINISTRATOR, Roles.PIC)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        [NoDirectAccess]
        public JsonResult RequestRenewal(int Id)
        {
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                LicenseHQ licenseHQ = licenseDbContext.GetLicenseHQByID(Id);

                var webRoot = _env.WebRootPath; //get wwwroot Folder

                //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Email"
                        + Path.DirectorySeparatorChar.ToString()
                        + "License_Request_Renewal.html";

                licenseHQEmail.RequestRenewal(licenseHQ, pathToFile);

                TempData["requestRenewalMessage"] = string.Format("{0} has been requested for renewal! Please wait until Administrator renew the license.", licenseHQ.LicenseName);

                return Json(new { status = "Success" });
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "HQ USER";

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

        #region DOWNLOAD LICENSE FILE
        public async Task<IActionResult> DownloadLicenseFile(int id)
        {
            LicenseHQ licenseHQ = licenseDbContext.GetLicenseHQByID(id);

            //check license site is not null
            if (licenseHQ == null) return null;

            //get file path
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\HQ");
            var filePath = Path.Combine(basePath, licenseHQ.LicenseFileName);

            //get file extension to check mimeType
            var extension = Path.GetExtension(licenseHQ.LicenseFileName);
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(licenseHQ.LicenseFileName, out string contentType))
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
            return File(memory, FileType, licenseHQ.LicenseFileName);
        }
        #endregion

        #region LINKED BUSINESS UNIT TO BUSINESS DIV
        public JsonResult JSONGetUnitName(int DivID)
        {
            DataSet ds = ddlLicenseDBContext.ddlBusinessUnitLinkedDivHQ(DivID);
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
