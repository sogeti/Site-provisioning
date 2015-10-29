using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sogeti.ProvisioningWeb.Models;
using Sogeti.Provisioning.Domain;
using Sogeti.Provisioning.Business.Interface;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System.Net;
using Microsoft.Ajax.Utilities;
using Microsoft.SharePoint.Client.UserProfiles;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using Sogeti.Provisioning.Business.Services;

namespace Sogeti.ProvisioningWeb.Controllers
{
    public class SiteTemplateController : Controller
    {
        private readonly ISiteTemplateService _siteTemplateService;
        private readonly ITemplateFilesBlobService _templateFilesBlobService;
        private readonly IPnpFileService _pnpFileService;


        public SiteTemplateController(ISiteTemplateService siteTemplateService, ITemplateFilesBlobService templateFilesBlobService, IPnpFileService pnpFileService)
        {
            _siteTemplateService = siteTemplateService;
            _templateFilesBlobService = templateFilesBlobService;
            _pnpFileService = pnpFileService;
        }


		[SharePointContextFilter]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var templates = await _siteTemplateService.Read();
            var model = templates.OrderByDescending(st => st.CreationTimeStamp).ToList();
            return View(model);
        }

        [SharePointContextFilter]
        [HttpGet]
        public async Task<ActionResult> PickFile()
        {
            var pnpFiles = await _pnpFileService.Read();
            var model = new PnpFileViewModel
            {
                Id = Guid.NewGuid(),
                PnpFiles = pnpFiles.ToList<PnpFile>()
            };

            return View(model);
        }

        [SharePointContextFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PickFile(PnpFileViewModel model)
        {
         
            if (model.SelectedPnpFile != "0")
            {
                return RedirectToAction("Create", new { id = Guid.Parse(model.SelectedPnpFile), SPHostUrl = Request.QueryString["SPHostUrl"] });
            }

            if (model.file != null)
            {
                var pnpFiles =  _pnpFileService.Read().Result.ToList();
                var info = model.file.FileName;
                bool sameName = pnpFiles.Any(l => l.Filename == model.file.FileName);

                if (sameName)
                    return null;
                else
                {
                    _pnpFileService.Insert(model.ToPnpFile());
                    return RedirectToAction("Create", new {id = model.Id, SPHostUrl = Request.QueryString["SPHostUrl"]});
                }
            }

            return View(model);
        }

        [SharePointContextFilter]
        [HttpGet]
        public ViewResult Create(Guid id)
        {
            var model = new SiteTemplateViewModel();
            model.SiteTemplateGuid = Guid.NewGuid();
            model.PnpFileGuid = id;
            model.UsesDefaultTemplateFiles = false;

            model.LogoBlobLocation = "";
            model.FileBgBlobLocation = "";
            model.FileClrBlobLocation = "";
            model.FontBlobLocation = "";

            var templateFolder = model.SiteTemplateGuid.ToString();

            return View(model);
        }
  
        [SharePointContextFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(SiteTemplateViewModel model)
        {
            var pnpFile = await _pnpFileService.Read(model.PnpFileGuid);
            if (pnpFile.PnpTemplate == null)
            {
                return RedirectToAction("PickFile", new { SPHostUrl = Request.QueryString["SPHostUrl"] });
            }


            pnpFile.PnpTemplate.ComposedLook.BackgroundFile = model.FileBgBlobLocation;
            pnpFile.PnpTemplate.ComposedLook.SiteLogo = model.LogoBlobLocation;
            pnpFile.PnpTemplate.ComposedLook.ColorFile= model.FileClrBlobLocation;
            pnpFile.PnpTemplate.ComposedLook.FontFile = model.FontBlobLocation ;

            var siteTemplate = new SiteTemplate
            {
                Id = Guid.NewGuid(),
                Name = model.SiteTemplateName,
                Description = model.Description,
                PnpTemplate = pnpFile.PnpTemplate,
                CreationTimeStamp = DateTime.Now
            };

            await _siteTemplateService.Insert(siteTemplate);

            return RedirectToAction("Index", new { SPHostUrl = Request.QueryString["SPHostUrl"] });
        }

        [HttpGet]
        public JsonResult GetFileLocation (string folder, string file, string defaultloc)
        {
            if (defaultloc == "true")
                folder = "Default";

            var location = _templateFilesBlobService.GetFileValue(file, folder) ?? "";
            return Json(location, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ViewResult> Edit(string id)
        {
            var model = await _siteTemplateService.Read(id);
            var returnValue = new SiteTemplateViewModel
            {
                SiteTemplateName = model.Name,
                Description = model.Description
            };

            return View(returnValue);
        }

        private static ProvisioningTemplate LoadPnpFromFile(HttpPostedFileBase file)
        {
            var schemaFormatter = new XMLPnPSchemaFormatter();
            if (!schemaFormatter.IsValid(file.InputStream))
                return null;

            var pnpProvisioningTemplate = schemaFormatter.ToProvisioningTemplate(file.InputStream);
            return pnpProvisioningTemplate;
        }


        [HttpPost]
        public async Task<ActionResult> ValidateTemplate(PnpFileViewModel pnpViewModel)
        {
            string fileName = pnpViewModel.file.FileName;
            var validFileName = await _pnpFileService.Validate(fileName);

            return Json(new
            {
                Result = validFileName.Key,
                Message = validFileName.Value
            });
        }

        [HttpPost]
        public async Task<ActionResult> SaveExternFile(SiteTemplateViewModel model, string fileType, string location)
        {
            int pos;
            string padnaam;
            var fileLocation = model.SiteTemplateGuid.ToString();

            if (location == "on")
                fileLocation = "Default";

            try
            {
                if (model.ExternFileBg != null && fileType == "Background.jpg")
                {
                    await _templateFilesBlobService.SaveFile(model.ExternFileBg, fileLocation);
                    pos = model.ExternFileBg.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
                    padnaam = _templateFilesBlobService.FileInBlob(model.ExternFileBg.FileName.Substring(pos, model.ExternFileBg.FileName.Length - pos), model.SiteTemplateName);

                    return Json(new { pad = padnaam });
                }
                if (model.ExternFileClr != null && fileType == "Color.spcolor")
                {
                    await _templateFilesBlobService.SaveFile(model.ExternFileClr, fileLocation);
                    pos = model.ExternFileClr.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
                    padnaam = _templateFilesBlobService.FileInBlob(model.ExternFileClr.FileName.Substring(pos, model.ExternFileClr.FileName.Length - pos), model.SiteTemplateName);

                    return Json(new { pad = padnaam });
                }
                if (model.ExternFileFont != null && fileType == "Font.spfont")
                {
                    await _templateFilesBlobService.SaveFile(model.ExternFileFont, fileLocation);
                    pos = model.ExternFileFont.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
                    padnaam = _templateFilesBlobService.FileInBlob(model.ExternFileFont.FileName.Substring(pos, model.ExternFileFont.FileName.Length - pos), model.SiteTemplateName);

                    return Json(new { pad = padnaam });
                }
                if (model.ExternFileLogo != null && fileType == "Logo.png")
                {
                    await _templateFilesBlobService.SaveFile(model.ExternFileLogo, fileLocation);
                    pos = model.ExternFileLogo.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
                    padnaam = _templateFilesBlobService.FileInBlob(model.ExternFileLogo.FileName.Substring(pos, model.ExternFileLogo.FileName.Length - pos), model.SiteTemplateName);

                    return Json(new { pad = padnaam });
                }
            }
            catch (Exception e)
            {
                return Json(new { pad = e.ToString() });
            }

            return null; 
        }

    
        [SharePointContextFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Edit")]
        public async Task<ActionResult> EditSiteTemplate(SiteTemplate model)
        {
            await _siteTemplateService.Insert(model);

            return Redirect("Index?" + SharePointContext.CloneQueryString(HttpContext.Request));
        }

        [SharePointContextFilter]
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            
            var template = await _siteTemplateService.Read(id);
            
            return View(template);
        }

        [SharePointContextFilter]
        [HttpPost]        
        public async Task<ActionResult> DeleteSiteTemplate(SiteTemplate templ)
        {
            
            await _siteTemplateService.Delete(templ);
            
            return Redirect("/SiteTemplate/Index?" + SharePointContext.CloneQueryString(HttpContext.Request));
        }


    }
}
