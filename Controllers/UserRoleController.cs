using BLMS.Context;
using BLMS.Custom_Attributes;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class UserRoleController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();
        readonly ddlAdminDBContext ddlDBContext = new ddlAdminDBContext();

        #region DRIDVIEW
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        public IActionResult Index()
        {
            List<UserRole> UserRoleList = dbContext.UserRoleGetAll().ToList();

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

            return View(UserRoleList);
        }
        #endregion

        #region CREATE
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        [NoDirectAccess]
        public ActionResult Create()
        {
            //ddlStaffName
            List<UserRole> staffNameUserRoleList = ddlDBContext.ddlStaffNameUserRole().ToList();
            staffNameUserRoleList.Insert(0, new UserRole { UserRoleStaffNo = "-", UserRoleName = "Please Select Staff Name" });
            ViewBag.ListofStaffName = staffNameUserRoleList;

            //ddlRole
            List<UserRole> roleUserRoleList = ddlDBContext.ddlRoleUserRole().ToList();
            roleUserRoleList.Insert(0, new UserRole { RoleID = 0, Role = "Please Select User Role" });
            ViewBag.ListofRole = roleUserRoleList;

            //ddlUnitType
            List<UserRole> userTypeUserRoleList = ddlDBContext.ddlUserTypeUserRole().ToList();
            userTypeUserRoleList.Insert(0, new UserRole { UserTypeID = 0, UserType = "Please Select User Type" });
            ViewBag.ListofUserType = userTypeUserRoleList;

            return View();
        }

        // POST: PICController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] UserRole userRole, string UserName)
        {
            try
            {
                UserName = HttpContext.User.Identity.Name;

                List<UserRole> staffNameUserRoleList, roleUserRoleList, userTypeUserRoleList;

                //Validate All
                if (userRole.UserRoleStaffNo == "-" && userRole.RoleID == 0 && userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "Please Select Staff Name");
                    ModelState.AddModelError("", "Please Select User Role");
                    ModelState.AddModelError("", "Please Select User Type");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please Select Staff Name", "Please Select User Role", "Please Select User Type", "");
                }
                else if (userRole.UserRoleStaffNo == "-" && userRole.RoleID == 0)
                {
                    ModelState.AddModelError("", "Please Select Staff Name");
                    ModelState.AddModelError("", "Please Select User Role");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Staff Name", "Please Select User Role", "", "");
                }
                else if (userRole.UserRoleStaffNo == "-" && userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT STAFF NAME");
                    ModelState.AddModelError("", "PLEASE SELECT USER TYPE");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Staff Name", "Please Select User Type", "", "");
                }
                else if (userRole.RoleID == 0 && userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT USER ROLE");
                    ModelState.AddModelError("", "PLEASE SELECT USER TYPE");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select User Role", "Please Select User Type", "", "");
                }
                //Validate Staff Name
                else if (userRole.UserRoleStaffNo == "-")
                {
                    ModelState.AddModelError("", "PLEASE SELECT STAFF NAME");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PLEASE SELECT STAFF NAME");
                }
                //Validate User Type
                else if (userRole.RoleID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT USER ROLE");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PLEASE SELECT USER ROLE");
                }
                else if (userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT USER TYPE");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "PLEASE SELECT USER TYPE");
                }

                if (ModelState.IsValid)
                {
                    string UserRoleStaffNo = userRole.UserRoleStaffNo;

                    UserRole checkUserRole = dbContext.CheckUserRoleByName(UserRoleStaffNo);

                    if (checkUserRole.ExistData == 1)
                    {
                        UserRole GetUserRoleName = dbContext.GetUserRoleName(UserRoleStaffNo);

                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} ALREADY EXISTED IN BLMS DATABASE!", GetUserRoleName.UserRoleName));

                        //ddlStaffName
                        staffNameUserRoleList = ddlDBContext.ddlStaffNameUserRole().ToList();
                        staffNameUserRoleList.Insert(0, new UserRole { UserRoleStaffNo = "-", UserRoleName = "PLEASE SELECT STAFF NAME" });
                        ViewBag.ListofStaffName = staffNameUserRoleList;

                        //ddlRole
                        roleUserRoleList = ddlDBContext.ddlRoleUserRole().ToList();
                        roleUserRoleList.Insert(0, new UserRole { RoleID = 0, Role = "PLEASE SELECT USER ROLE" });
                        ViewBag.ListofRole = roleUserRoleList;

                        //ddlUnitType
                        userTypeUserRoleList = ddlDBContext.ddlUserTypeUserRole().ToList();
                        userTypeUserRoleList.Insert(0, new UserRole { UserTypeID = 0, UserType = "PLEASE SELECT USER TYPE" });
                        ViewBag.ListofUserType = userTypeUserRoleList;

                        return View(userRole);
                    }
                    else
                    {
                        dbContext.AddUserRole(userRole, UserName);

                        UserRole getUserRoleName = dbContext.GetUserRoleName(UserRoleStaffNo);

                        TempData["createMessage"] = string.Format("{0} HAS BEEN SUCCESSFULLY CREATED!", getUserRoleName.UserRoleName);
                        return RedirectToAction("Index");
                    }
                }

                //ddlStaffName
                staffNameUserRoleList = ddlDBContext.ddlStaffNameUserRole().ToList();
                staffNameUserRoleList.Insert(0, new UserRole { UserRoleStaffNo = "-", UserRoleName = "PLEASE SELECT STAFF NAME" });
                ViewBag.ListofStaffName = staffNameUserRoleList;

                //ddlRole
                roleUserRoleList = ddlDBContext.ddlRoleUserRole().ToList();
                roleUserRoleList.Insert(0, new UserRole { RoleID = 0, Role = "PLEASE SELECT USER ROLE" });
                ViewBag.ListofRole = roleUserRoleList;

                //ddlUnitType
                userTypeUserRoleList = ddlDBContext.ddlUserTypeUserRole().ToList();
                userTypeUserRoleList.Insert(0, new UserRole { UserTypeID = 0, UserType = "PLEASE SELECT USER TYPE" });
                ViewBag.ListofUserType = userTypeUserRoleList;

                return View(userRole);
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
            UserRole userRole = dbContext.GetUserRoleByID(id);

            //ddlStaffName
            List<UserRole> staffNameUserRoleList = ddlDBContext.ddlStaffNameUserRole().ToList();
            staffNameUserRoleList.Insert(0, new UserRole { UserRoleStaffNo = "-", UserRoleName = "Please Select Staff Name" });
            ViewBag.ListofStaffName = staffNameUserRoleList;

            //ddlRole
            List<UserRole> roleUserRoleList = ddlDBContext.ddlRoleUserRole().ToList();
            roleUserRoleList.Insert(0, new UserRole { RoleID = 0, Role = "Please Select User Role" });
            ViewBag.ListofRole = roleUserRoleList;

            //ddlUnitType
            List<UserRole> userTypeUserRoleList = ddlDBContext.ddlUserTypeUserRole().ToList();
            userTypeUserRoleList.Insert(0, new UserRole { UserTypeID = 0, UserType = "Please Select User Type" });
            ViewBag.ListofUserType = userTypeUserRoleList;

            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: PICController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] UserRole userRole, string UserName)
        {
            try
            {
                UserName = HttpContext.User.Identity.Name;

                List<UserRole> staffNameUserRoleList, roleUserRoleList, userTypeUserRoleList;

                //Validate All
                if (userRole.UserRoleStaffNo == "-" && userRole.RoleID == 0 && userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "Please Select Staff Name");
                    ModelState.AddModelError("", "Please Select User Role");
                    ModelState.AddModelError("", "Please Select User Type");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningThree, "Please Select Staff Name", "Please Select User Role", "Please Select User Type", "");
                }
                else if (userRole.UserRoleStaffNo == "-" && userRole.RoleID == 0)
                {
                    ModelState.AddModelError("", "Please Select Staff Name");
                    ModelState.AddModelError("", "Please Select User Role");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Staff Name", "Please Select User Role", "", "");
                }
                else if (userRole.UserRoleStaffNo == "-" && userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT STAFF NAME");
                    ModelState.AddModelError("", "PLEASE SELECT USER TYPE");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select Staff Name", "Please Select User Type", "", "");
                }
                else if (userRole.RoleID == 0 && userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT USER ROLE");
                    ModelState.AddModelError("", "PLEASE SELECT USER TYPE");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please Select User Role", "Please Select User Type", "", "");
                }
                //Validate Staff Name
                else if (userRole.UserRoleStaffNo == "-")
                {
                    ModelState.AddModelError("", "PLEASE SELECT STAFF NAME");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select Staff Name");
                }
                //Validate User Type
                else if (userRole.RoleID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT USER ROLE");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select User Role");
                }
                else if (userRole.UserTypeID == 0)
                {
                    ModelState.AddModelError("", "PLEASE SELECT USER TYPE");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please Select User Type");
                }

                if (ModelState.IsValid)
                {
                    string UserRoleStaffNo = userRole.OldUserRoleStaffNo;
                    int OldRole = userRole.OldRoleID;
                    int UserType = userRole.OldUserTypeID;

                    UserRole checkUserRole = dbContext.CheckUserRoleByName(UserRoleStaffNo);

                    if (checkUserRole.ExistData == 1 && OldRole == userRole.RoleID && UserType == userRole.UserTypeID)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (checkUserRole.ExistData == 1 && OldRole == userRole.RoleID && UserType != userRole.UserTypeID)
                    {
                        dbContext.EditUserRole(userRole, UserName);

                        UserRole getUserRoleName = dbContext.GetUserRoleName(UserRoleStaffNo);

                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", getUserRoleName.UserRoleName);
                        return RedirectToAction("Index");
                    }
                    else if (checkUserRole.ExistData == 1 && OldRole != userRole.RoleID && UserType == userRole.UserTypeID)
                    {
                        dbContext.EditUserRole(userRole, UserName);

                        UserRole getUserRoleName = dbContext.GetUserRoleName(UserRoleStaffNo);

                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", getUserRoleName.UserRoleName);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        dbContext.EditUserRole(userRole, UserName);

                        UserRole getUserRoleName = dbContext.GetUserRoleName(UserRoleStaffNo);

                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", getUserRoleName.UserRoleName);
                        return RedirectToAction("Index");
                    }
                }

                //ddlStaffName
                staffNameUserRoleList = ddlDBContext.ddlStaffNameUserRole().ToList();
                staffNameUserRoleList.Insert(0, new UserRole { UserRoleStaffNo = "-", UserRoleName = "Please select Staff Name" });
                ViewBag.ListofStaffName = staffNameUserRoleList;

                //ddlRole
                roleUserRoleList = ddlDBContext.ddlRoleUserRole().ToList();
                roleUserRoleList.Insert(0, new UserRole { RoleID = 0, Role = "Please select User Role" });
                ViewBag.ListofRole = roleUserRoleList;

                //ddlUnitType
                userTypeUserRoleList = ddlDBContext.ddlUserTypeUserRole().ToList();
                userTypeUserRoleList.Insert(0, new UserRole { UserTypeID = 0, UserType = "Please select User Type" });
                ViewBag.ListofUserType = userTypeUserRoleList;

                return View(userRole);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        [Authorize(Roles.ADMINISTRATOR)]
        [Authorize(AccessLevel.ADMINISTRATION)]
        public JsonResult Delete(int Id)
        {

            try
            {
                UserRole userRole = dbContext.GetUserRoleByID(Id);

                dbContext.DeleteUserRole(Id);
                TempData["deleteMessage"] = string.Format("{0} has been deleted from BLMS database!", userRole.UserRoleName);

                return Json(new { status = "Success" });
            }
            catch
            {
                return Json(new { status = "Fail" });
            }
        }
        #endregion

        #region LINKED USER TYPE TO USER ROLE
        public JsonResult JSONGetUserRole(int RoleID)
        {
            DataSet ds = ddlDBContext.ddlUserTypeLinkedRole(RoleID);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["UserType"].ToString(), Value = dr["UserTypeID"].ToString() });
            }
            return Json(list);
        }
        #endregion
    }
}
