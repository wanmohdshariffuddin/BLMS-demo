using BLMS.Context;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.Admin;
using BLMS.Models.SOP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Controllers
{
    public class CompetentPersonnelController : Controller
    {
        private readonly CompetentDBContext _context;
        readonly CompetentPersonnelDBContext competentDBContext = new CompetentPersonnelDBContext();
        readonly AdminDBContext ddlOthers = new AdminDBContext();

        public CompetentPersonnelController(CompetentDBContext context)
        {
            _context = context;
        }

        #region GRIDVIEW
        public IActionResult Index()
        {
            string UserName = HttpContext.User.Identity.Name;

            List<Competent> CompetentList = competentDBContext.CompetentPersonnelGetAll(UserName).ToList();

            #region Alert Message
            if (TempData["createMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["createMessage"].ToString());
            }
            if (TempData["editMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["editMessage"].ToString());
            }
            #endregion

            return View(CompetentList);
        }
        #endregion

        #region DETAILS
        // GET: CompetentPersonnel/Details/5
        public async Task<IActionResult> Details(int? id, string m)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competentPersonnel = await _context.CompetentPersonnel
                .FirstOrDefaultAsync(m => m.PersonnelId == id);
            if (competentPersonnel == null)
            {
                return NotFound();
            }

            // Update page Competent Personnel data
            PageCompetentPersonnel pCompetentPersonnel = new PageCompetentPersonnel { CompetentPersonnel = competentPersonnel};

            return View(pCompetentPersonnel);
        }
        #endregion

        #region REGISTER
        // GET: CompetentPersonnel/Register
        public IActionResult Register()
        {
            // Update page Competent Personnel data
            CompetentPersonnel pCompetentPersonnel = new CompetentPersonnel();

            #region DROPDOWN
            List<BusinessDiv> businessDivList = ddlOthers.BusinessDivGetAll().ToList();
            businessDivList.Insert(0, new BusinessDiv { DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivList;

            List<BusinessUnit> businessUnitList = ddlOthers.BusinessUnitGetAll().ToList();
            businessUnitList.Insert(0, new BusinessUnit { UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitList;

            List<CertBody> certBodyList = ddlOthers.CertBodyGetAll().ToList();
            certBodyList.Insert(0, new CertBody { CertBodyName = "Please Select Certificate Body" });
            ViewBag.ListofCertBody = certBodyList;
            #endregion

            return View(pCompetentPersonnel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind] CompetentPersonnel competentPersonnel)
        {
            string UserName = HttpContext.User.Identity.Name;

            competentPersonnel.CreatedBy = UserName;
            competentPersonnel.CreatedDt = DateTime.Now;

            #region VALIDATION
            if(string.IsNullOrEmpty(competentPersonnel.PersonnelName))
            {
                if(competentPersonnel.BusinessDiv == "Please Select Business Division")
                {
                    if(competentPersonnel.BusinessUnit == "Please Select Business Unit")
                    {
                        if(competentPersonnel.CertFrom == "Please Select Certificate Body")
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please Type Staff Name", "Please Select Business Division", "Please Select Business Unit", "Please Select Certificate Body");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please Type Staff Name", "Please Select Business Division", "Please Select Business Unit", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Type Staff Name", "Please Select Business Division", "", "");
                    }
                }
                else
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Type Staff Name");
                }
            }
            else if(competentPersonnel.BusinessDiv == "Please Select Business Division")
            {
                if(competentPersonnel.BusinessUnit == "Please Select Business Unit")
                {
                    if(competentPersonnel.CertFrom == "Please Select Certificate Body")
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please Select Business Division", "Please Select Business Unit", "Please Select Certificate Body", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Business Division", "Please Select Business Unit", "", "");
                    }
                }
                else
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Business Division");
                }
            }
            else if(competentPersonnel.BusinessUnit == "Please Select Business Unit")
            {
                if(competentPersonnel.CertFrom == "Please Select Certificate Body")
                {
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Business Unit", "Please Select Certificate Body", "", "");
                }
                else
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Business Unit");
                }
            }
            else if(competentPersonnel.CertFrom == "Please Select Certificate Body")
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Certificate Body");
            }
            #endregion

            else
            {
                _context.Add(competentPersonnel);
                await _context.SaveChangesAsync();
                TempData["createMessage"] = string.Format("{0} has been successfully created!", competentPersonnel.PersonnelName);
                return RedirectToAction("Index");
            }

            #region DROPDOWN
            List<BusinessDiv> businessDivList = ddlOthers.BusinessDivGetAll().ToList();
            businessDivList.Insert(0, new BusinessDiv { DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivList;

            List<BusinessUnit> businessUnitList = ddlOthers.BusinessUnitGetAll().ToList();
            businessUnitList.Insert(0, new BusinessUnit { UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitList;

            List<CertBody> certBodyList = ddlOthers.CertBodyGetAll().ToList();
            certBodyList.Insert(0, new CertBody { CertBodyName = "Please Select Certificate Body" });
            ViewBag.ListofCertBody = certBodyList;
            #endregion

            return View(competentPersonnel);
        }
        #endregion

        #region RENEWAL
        // GET: CompetentPersonnel/Renewal/5
        public async Task<IActionResult> Renewal(int? id)
        {
            var competentPersonnel = await _context.CompetentPersonnel.FindAsync(id);

            if (id == null)
            {
                return NotFound();
            }

            #region DROPDOWN
            List<BusinessDiv> businessDivList = ddlOthers.BusinessDivGetAll().ToList();
            businessDivList.Insert(0, new BusinessDiv { DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivList;

            List<BusinessUnit> businessUnitList = ddlOthers.BusinessUnitGetAll().ToList();
            businessUnitList.Insert(0, new BusinessUnit { UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitList;

            List<CertBody> certBodyList = ddlOthers.CertBodyGetAll().ToList();
            certBodyList.Insert(0, new CertBody { CertBodyName = "Please Select Certificate Body" });
            ViewBag.ListofCertBody = certBodyList;
            #endregion

            return View(competentPersonnel);
        }

        // POST: CompetentPersonnel/Renewal/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renewal(int id, [Bind] CompetentPersonnel competentPersonnel)
        {
            string UserName = HttpContext.User.Identity.Name;

            competentPersonnel.UpdatedBy = UserName;
            competentPersonnel.UpdatedDt = DateTime.Now;

            #region VALIDATION
            if (string.IsNullOrEmpty(competentPersonnel.PersonnelName))
            {
                if (string.IsNullOrEmpty(competentPersonnel.BusinessDiv))
                {
                    if (string.IsNullOrEmpty(competentPersonnel.BusinessUnit))
                    {
                        if (string.IsNullOrEmpty(competentPersonnel.CertFrom))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningFour, "Please Type Staff Name", "Please Select Business Division", "Please Select Business Unit", "Please Select Certificate Body");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please Type Staff Name", "Please Select Business Division", "Please Select Business Unit", "");
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Type Staff Name", "Please Select Business Division", "", "");
                    }
                }
                else
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Type Staff Name");
                }
            }
            else if (string.IsNullOrEmpty(competentPersonnel.BusinessDiv))
            {
                if (string.IsNullOrEmpty(competentPersonnel.BusinessUnit))
                {
                    if (string.IsNullOrEmpty(competentPersonnel.CertFrom))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please Select Business Division", "Please Select Business Unit", "Please Select Certificate Body", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Business Division", "Please Select Business Unit", "", "");
                    }
                }
                else
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Business Division");
                }
            }
            else if (string.IsNullOrEmpty(competentPersonnel.BusinessUnit))
            {
                if (string.IsNullOrEmpty(competentPersonnel.CertFrom))
                {
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Business Unit", "Please Select Certificate Body", "", "");
                }
                else
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Business Unit");
                }
            }
            else if (string.IsNullOrEmpty(competentPersonnel.CertFrom))
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Certificate Body");
            }
            #endregion

            else
            {
                _context.Update(competentPersonnel);
                await _context.SaveChangesAsync();
                TempData["editMessage"] = string.Format("{0} has been successfully renewed!", competentPersonnel.PersonnelName);
                return RedirectToAction("Index");
            }

            #region DROPDOWN
            List<BusinessDiv> businessDivList = ddlOthers.BusinessDivGetAll().ToList();
            businessDivList.Insert(0, new BusinessDiv { DivName = "Please Select Business Division" });
            ViewBag.ListofBusinessDiv = businessDivList;

            List<BusinessUnit> businessUnitList = ddlOthers.BusinessUnitGetAll().ToList();
            businessUnitList.Insert(0, new BusinessUnit { UnitName = "Please Select Business Unit" });
            ViewBag.ListofBusinessUnit = businessUnitList;

            List<CertBody> certBodyList = ddlOthers.CertBodyGetAll().ToList();
            certBodyList.Insert(0, new CertBody { CertBodyName = "Please Select Certificate Body" });
            ViewBag.ListofCertBody = certBodyList;
            #endregion

            return View(competentPersonnel);
        }
        #endregion
    }
}
