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
    public class CategoryController : Controller
    {
        readonly AdminDBContext dbContext = new AdminDBContext();

        #region GRIDVIEW
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        public ActionResult Index()
        {
            List<Category> CategoryList = dbContext.CategoryGetAll().ToList();

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

            return View(CategoryList);
        }
        #endregion

        #region CREATE
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Category category, string UserName)
        {
            try
            {
                UserName = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(category.CategoryName) && string.IsNullOrEmpty(category.Description))
                {
                    ModelState.AddModelError("", "Please type License Type");
                    ModelState.AddModelError("", "Please type License Description");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Type", "Please type License Description", "", "");
                }
                else if (string.IsNullOrEmpty(category.CategoryName))
                {
                    ModelState.AddModelError("", "Please type License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Type");
                }
                else if (string.IsNullOrEmpty(category.Description))
                {
                    ModelState.AddModelError("", "Please type License Description");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Description");
                }
                if (ModelState.IsValid)
                {
                    string CategoryName = category.CategoryName;

                    Category checkCategory = dbContext.CheckCategoryByName(CategoryName);

                    if (checkCategory.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", CategoryName));
                        return View(category);
                    }
                    else
                    {
                        dbContext.AddCategory(category, UserName);
                        TempData["createMessage"] = string.Format("{0} has been successfully created!", CategoryName);
                        return RedirectToAction("Index");
                    }
                }

                return View(category);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region EDIT
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        //[NoDirectAccess]
        public ActionResult Edit(int id)
        {
            Category category = dbContext.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind] Category category, string UserName)
        {
            try
            {
                UserName = HttpContext.User.Identity.Name;

                if (string.IsNullOrEmpty(category.CategoryName) && string.IsNullOrEmpty(category.Description))
                {
                    ModelState.AddModelError("", "Please type License Type");
                    ModelState.AddModelError("", "Please type License Description");
                    ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type License Type", "Please type License Description", "", "");
                }
                else if (string.IsNullOrEmpty(category.CategoryName))
                {
                    ModelState.AddModelError("", "Please type License Type");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Type");
                }
                else if (string.IsNullOrEmpty(category.Description))
                {
                    ModelState.AddModelError("", "Please type License Description");
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type License Description");
                }
                if (ModelState.IsValid)
                {
                    string CategoryName = category.CategoryName;
                    string oldCategoryName = category.OldCategoryName;
                    string oldDesc = category.OldDesc;

                    Category checkCategory = dbContext.CheckCategoryByName(CategoryName);

                    if (checkCategory.ExistData == 1 && CategoryName != oldCategoryName && category.Description == oldDesc)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", CategoryName));
                        return View(category);
                    }
                    else if (checkCategory.ExistData == 1 && CategoryName != oldCategoryName && category.Description != oldDesc)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", CategoryName));
                        return View(category);
                    }
                    else if (checkCategory.ExistData == 1 && CategoryName == oldCategoryName && category.Description == oldDesc)
                    {
                        dbContext.EditCategory(category, UserName);
                        return RedirectToAction("Index");
                    }
                    else if (checkCategory.ExistData == 1 && CategoryName == oldCategoryName && category.Description != oldDesc)
                    {
                        dbContext.EditCategory(category, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", category.CategoryName);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        dbContext.EditCategory(category, UserName);
                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", category.CategoryName);
                        return RedirectToAction("Index");
                    }
                }

                return View(dbContext);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        //[Authorize(Roles.ADMINISTRATOR)]
        //[Authorize(AccessLevel.ADMINISTRATION)]
        public JsonResult Delete(int Id)
        {

            try
            {
                Category category = dbContext.GetCategoryByID(Id);

                dbContext.DeleteCategory(Id);
                TempData["deleteMessage"] = string.Format("{0} has been successfully deleted!", category.CategoryName);

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
