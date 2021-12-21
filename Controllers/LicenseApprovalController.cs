using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.License;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Controllers
{
    public class LicenseApprovalController : Controller
    {
        readonly LicenseDBContext licenseDbContext = new LicenseDBContext();

        private IWebHostEnvironment _env;

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        public IActionResult Index()
        {
            List<LicenseApproval> licenseApprovalList = licenseDbContext.LicenseApprovalGetAll().ToList();

            return View(licenseApprovalList);
        }
        #endregion

        #region VIEW REQUEST
        //[Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
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
        //[Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
        public JsonResult ConfirmApprove(int Id)
        {
            try
            {
                string UserName = User.Identity.Name;

                LicenseApproval licenseApproval = licenseDbContext.GetLicenseApprovalByID(Id);

                licenseDbContext.ApproveLicense(Id, UserName);

                return Json(new { status = "Success" });
            }
            catch
            {
                return Json(new { status = "Fail" });
            }
        }
        #endregion

        #region CONFIRM REJECT
        //[Authorize(Roles.ADMINISTRATOR, Roles.BUSINESS_UNIT)]
        //[Authorize(AccessLevel.ADMINISTRATION, AccessLevel.HQ)]
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
                    return Json(new { status = "Success" });
                }
                else
                {
                    #region CHANGE FIRST LETTER (LOWER TO UPPER)
                    TextInfo cultInfoRejectionRemarks = new CultureInfo("en-US", false).TextInfo;
                    string RejectionRemarks = cultInfoRejectionRemarks.ToTitleCase(Remarks);

                    Remarks = RejectionRemarks;
                    #endregion

                    licenseDbContext.RejectLicense(Id, Remarks, UserName);
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
