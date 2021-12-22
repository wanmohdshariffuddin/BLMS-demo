using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Email;
using BLMS.Enums;
using BLMS.Models.License;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BLMS.Controllers
{
    public class LicenseApprovalController : Controller
    {
        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();
        readonly LicenseBUEmail licenseBUEmail = new LicenseBUEmail();

        private IWebHostEnvironment _env;

        public LicenseApprovalController(IWebHostEnvironment env)
        {
            _env = env;
        }

        #region GRIDVIEW
        [Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        public IActionResult Index()
        {
            List<LicenseApproval> licenseApprovalList = licenseDbContext.LicenseApprovalGetAll().ToList();

            return View(licenseApprovalList);
        }
        #endregion

        #region VIEW REQUEST
        [Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT)]
        [Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        public IActionResult View(int id)
        {
            LicenseApproval licenseApproval = licenseDbContext.GetLicenseApprovalByID(id);

            if (TempData["remarksMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, TempData["remarksMessage"].ToString());
            }

            if (licenseApproval == null)
            {
                return NotFound();
            }

            return View(licenseApproval);
        }
        #endregion

        #region CONFIRM APPROVE
        public JsonResult ConfirmApprove(int Id)
        {
            try
            {
                string UserName = User.Identity.Name;

                LicenseApproval licenseApproval = licenseDbContext.GetLicenseApprovalByID(Id);

                licenseDbContext.ApproveLicense(Id, UserName);

                #region EMAIL TO PIC
                var webRoot = _env.WebRootPath; //get wwwroot Folder

                //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Email"
                        + Path.DirectorySeparatorChar.ToString()
                        + "License_Approval_Approved.html";

                licenseBUEmail.ApprovedLicensePIC(licenseApproval.LicenseName, pathToFile);
                #endregion

                #region EMAIL TO ADMIN
                var webRootAdmin = _env.WebRootPath; //get wwwroot Folder

                //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                var pathToFileAdmin = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Email"
                        + Path.DirectorySeparatorChar.ToString()
                        + "License_Approval_Admin.html";

                licenseBUEmail.ApprovedLicenseAdmin(licenseApproval.LicenseName, pathToFileAdmin);
                #endregion

                return Json(new { status = "Success" });
            }
            catch
            {
                return Json(new { status = "Fail" });
            }
        }
        #endregion

        #region CONFIRM REJECT
        public JsonResult ConfirmReject(int Id, string Remarks)
        {
            try
            {
                string UserName = User.Identity.Name;

                LicenseApproval licenseApproval = licenseDbContext.GetLicenseApprovalByID(Id);

                if (string.IsNullOrEmpty(Remarks))
                {
                    ViewBag.Inserted = !string.IsNullOrEmpty(Remarks) ? true : false;
                    TempData["remarksMessage"] = string.Format("Please Type Rejection Remarks!");
                }
                else
                {
                    #region CHANGE FIRST LETTER (LOWER TO UPPER)
                    TextInfo cultInfoRejectionRemarks = new CultureInfo("en-US", false).TextInfo;
                    string RejectionRemarks = cultInfoRejectionRemarks.ToTitleCase(Remarks);

                    Remarks = RejectionRemarks;
                    #endregion

                    licenseDbContext.RejectLicense(Id, Remarks, UserName);

                    #region EMAIL TO PIC
                    var webRoot = _env.WebRootPath; //get wwwroot Folder

                    //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html
                    var pathToFile = _env.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Email"
                            + Path.DirectorySeparatorChar.ToString()
                            + "License_Approval_Rejected.html";

                    licenseBUEmail.RejectedLicensePIC(licenseApproval.LicenseName, pathToFile);
                    #endregion
                }

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
