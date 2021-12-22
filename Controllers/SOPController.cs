using BLMS.Context;
using BLMS.CustomAttributes;
using BLMS.Enums;
using BLMS.Models.SOP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BLMS.Helper;

namespace BLMS.Controllers
{
    public class SOPController : Controller
    {
        readonly SOPDBContext sopDbContext = new SOPDBContext();
        readonly LogDBContext logDBContext = new LogDBContext();

        #region GRIDVIEW
        public IActionResult Index()
        {
            List<SOP> SOPList = sopDbContext.SOPGetAll().ToList();

            if (TempData["RegisterMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["RegisterMessage"].ToString());
            }
            else if (TempData["EditMessage"] != null)
            {
                ViewBag.Alert = AlertNotification.ShowAlert(Alert.Success, TempData["EditMessage"].ToString());
            }

            return View(SOPList);
        }
        #endregion

        #region VIEW
        public ActionResult Details(int id)
        {
            SOP sop = sopDbContext.GetSOPByID(id);

            if (sop == null)
            {
                return NotFound();
            }

            return View(sop);
        }
        #endregion

        #region CREATE
        //[NoDirectAccess]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] SOP sop, List<IFormFile> SOPFile)
        {
            string UserName = HttpContext.User.Identity.Name;

            try
            {
                #region VALIDATION
                foreach (var sopFile in SOPFile)
                {
                    var fileName = Path.GetFileNameWithoutExtension(sopFile.FileName);
                    sop.SOPFileName = fileName;
                }

                if (string.IsNullOrEmpty(UserName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                }
                else if (string.IsNullOrEmpty(sop.SOPFileName))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload SOP File");
                }
                else if (string.IsNullOrEmpty(sop.SOPName))
                {
                    if (string.IsNullOrEmpty(sop.RefNo))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type SOP Name", "Please type Reference No", "", "");
                    }
                    else
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type SOP Name");
                    }
                }
                else if (string.IsNullOrEmpty(sop.RefNo))
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Reference No");
                }
                #endregion

                if (string.IsNullOrEmpty(ViewBag.Alert))
                {
                    #region CHECK EXIST DATA
                    SOP checkSOP = sopDbContext.CheckSOPByName(sop.SOPName);

                    if (checkSOP.ExistData == 1)
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", sop.SOPName));
                    }
                    #endregion

                    #region CONVERT NULL DATA
                    if (String.IsNullOrEmpty(sop.RevNo))
                    {
                        sop.RevNo = "-";
                    }

                    if (String.IsNullOrEmpty(sop.EffectiveDT))
                    {
                        sop.EffectiveDT = "-";
                    }

                    if (SOPFile.Count == 0)
                    {
                        sop.SOPFileName = "-";
                    }
                    #endregion

                    #region SAVE FILE
                    foreach (var sopFile in SOPFile)
                    {
                        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SOP\\");
                        bool basePathExists = Directory.Exists(basePath);

                        if (!basePathExists) Directory.CreateDirectory(basePath);

                        var fileName = Path.GetFileNameWithoutExtension(sopFile.FileName);
                        var filePath = Path.Combine(basePath, sopFile.FileName);
                        var extension = Path.GetExtension(sopFile.FileName);

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                sopFile.CopyTo(stream);
                            }

                            sop.SOPFileName = fileName + extension;
                        }
                    }
                    #endregion

                    #region SAVE DATA
                    sopDbContext.CreateSOP(sop, UserName);
                    #endregion

                    TempData["registerMessage"] = string.Format("{0} has been successfully registered!", sop.SOPName);

                    return RedirectToAction("Index", "SOP");
                }

                return View(sop);
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
        public ActionResult Edit(int id)
        {
            SOP sop = sopDbContext.GetSOPByID(id);

            if (sop == null)
            {
                return NotFound();
            }

            return View(sop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind] SOP sop, List<IFormFile> SOPFile)
        {
            string UserName = User.Identity.Name;

            try
            {
                #region CHECK DUPLICATION
                SOP checkSOP = sopDbContext.CheckSOPByName(sop.SOPName);

                if (checkSOP.ExistData == 1 && sop.OldSOPName != sop.SOPName)
                {
                    ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, string.Format("{0} already existed in BLMS database!", sop.SOPName));
                }
                #endregion

                else
                {
                    #region VALIDATION
                    foreach (var sopFile in SOPFile)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(sopFile.FileName);
                        sop.SOPFileName = fileName;
                    }

                    if (string.IsNullOrEmpty(UserName))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Danger, "Login session is expired. Please login again");
                    }
                    else if (string.IsNullOrEmpty(sop.SOPFileName) && string.IsNullOrEmpty(sop.OldSOPFileName))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please upload SOP File");
                    }
                    else if (string.IsNullOrEmpty(sop.SOPName))
                    {
                        if (string.IsNullOrEmpty(sop.RefNo))
                        {
                            ViewBag.Alert = AlertNotification.ShowAlertAll(Alert.WarningTwo, "Please type SOP Name", "Please type Reference No", "", "");
                        }
                        else
                        {
                            ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type SOP Name");
                        }
                    }
                    else if (string.IsNullOrEmpty(sop.RefNo))
                    {
                        ViewBag.Alert = AlertNotification.ShowAlert(Alert.Warning, "Please type Reference No");
                    }
                    #endregion

                    if (string.IsNullOrEmpty(ViewBag.Alert))
                    {
                        #region CONVERT NULL DATA
                        if (String.IsNullOrEmpty(sop.RevNo))
                        {
                            sop.RevNo = "-";
                        }

                        if (String.IsNullOrEmpty(sop.EffectiveDT))
                        {
                            sop.EffectiveDT = "-";
                        }

                        if (SOPFile.Count == 0)
                        {
                            sop.SOPFileName = "-";
                        }
                        #endregion

                        #region SAVE FILE
                        foreach (var sopFile in SOPFile)
                        {
                            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SOP\\");
                            bool basePathExists = Directory.Exists(basePath);

                            if (!basePathExists) Directory.CreateDirectory(basePath);

                            var fileName = Path.GetFileNameWithoutExtension(sopFile.FileName);
                            var filePath = Path.Combine(basePath, sopFile.FileName);
                            var extension = Path.GetExtension(sopFile.FileName);

                            if (!System.IO.File.Exists(filePath))
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    sopFile.CopyTo(stream);
                                }

                                sop.SOPFileName = fileName + extension;
                            }
                        }
                        #endregion

                        #region SAVE DATA
                        sopDbContext.EditSOP(sop, UserName);
                        #endregion

                        TempData["editMessage"] = string.Format("{0} has been successfully edited!", sop.SOPName);

                        return RedirectToAction("Index", "SOP");
                    }
                }

                return View(sop);
            }
            catch (Exception ex)
            {
                #region ERROR LOG
                string path = "SOP";

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

        #region DOWNLOAD FILE
        public async Task<IActionResult> DownloadSOPFile(int id)
        {
            SOP sop = sopDbContext.GetSOPByID(id);

            //check license site is not null
            if (sop == null) return null;

            //get file path
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SOP");
            var filePath = Path.Combine(basePath, sop.SOPFileName);

            //get file extension to check mimeType
            var extension = Path.GetExtension(sop.SOPFileName);
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(sop.SOPFileName, out string contentType))
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
            return File(memory, FileType, sop.SOPFileName);
        }
        #endregion
    }
}
