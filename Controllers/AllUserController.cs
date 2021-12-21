using BLMS.Context;
using BLMS.Models.License;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.Controllers
{
    public class AllUserController : Controller
    {
        readonly AllUserDBContext licenseDbContext = new AllUserDBContext();

        #region GRIDVIEW
        public IActionResult Index()
        {
            List<LicenseAllUser> licenseAllUserList = licenseDbContext.LicenseAllUserGetAll().ToList();

            return View(licenseAllUserList);
        }
        #endregion

        #region DOWNLOAD FILE ON SYSTEM (SITE)
        public async Task<IActionResult> DownloadSiteLicenseFile(int id)
        {
            LicenseAllUser licenseAllUser = licenseDbContext.GetLicenseAllUserByID(id);

            //check license site is not null
            if (licenseAllUser == null) return null;

            //get file path
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\SITE");
            var filePath = Path.Combine(basePath, licenseAllUser.LicenseFileName);

            //get file extension to check mimeType
            var extension = Path.GetExtension(licenseAllUser.LicenseFileName);
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(licenseAllUser.LicenseFileName, out string contentType))
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
            return File(memory, FileType, licenseAllUser.LicenseFileName);
        }
        #endregion

        #region DOWNLOAD FILE ON SYSTEM (HQ)
        public async Task<IActionResult> DownloadHQLicenseFile(int id)
        {
            LicenseAllUser licenseAllUser = licenseDbContext.GetLicenseAllUserByID(id);

            //check license site is not null
            if (licenseAllUser == null) return null;

            //get file path
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\HQ");
            var filePath = Path.Combine(basePath, licenseAllUser.LicenseFileName);

            //get file extension to check mimeType
            var extension = Path.GetExtension(licenseAllUser.LicenseFileName);
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(licenseAllUser.LicenseFileName, out string contentType))
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
            return File(memory, FileType, licenseAllUser.LicenseFileName);
        }
        #endregion
    }
}
