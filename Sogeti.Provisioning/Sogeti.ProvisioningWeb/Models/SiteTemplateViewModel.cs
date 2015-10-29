using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sogeti.ProvisioningWeb.Models
{
    public class SiteTemplateViewModel
    {
     
        public SiteTemplateViewModel()
        {

        }

        public SiteTemplateViewModel(PnpFile pnpModel)
        {
            //PnpProvisioningTemplate = pnpModel.PnpTemplate;
            UsesDefaultTemplateFiles = true; //todo: make it changeable        
        }

        [Required]
        public string SiteTemplateName { get; set; }

        public Guid SiteTemplateGuid { get; set; }

        [Required]
        public string Description { get; set; }

        public string UserAdmin { get; set; }

        public bool UsesDefaultTemplateFiles { get; set; }

        //public ProvisioningTemplate PnpProvisioningTemplate { get; set; }

        public Guid PnpFileGuid { get; set; }


        public List<O365User> Persons => new List<O365User>();


        public HttpPostedFileBase ExternFileBg { get; set; }

        public HttpPostedFileBase ExternFileClr { get; set; }

        public HttpPostedFileBase ExternFileLogo { get; set; }

        public HttpPostedFileBase ExternFileFont { get; set; }


        public string BlobContainer { get; set; }
        public string FileBgBlobLocation { get; set; }

        public string FileClrBlobLocation { get; set; }

        public string LogoBlobLocation { get; set; }

        public string FontBlobLocation { get; set; }

        //public SiteTemplate ToSiteTemplate()
        //{
        //    var returnValue = new SiteTemplate()
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = SiteTemplateName,
        //        Description = Description,
        //        UsesDefaultTemplateFiles = UsesDefaultTemplateFiles,
        //        PnpTemplate = PnpProvisioningTemplate
        //    };

        //    return returnValue;
        //}


        //model.PnpProvisioningTemplate.ComposedLook.BackgroundFile =
        //            _templateFilesBlobService.GetFileValue(model.PnpProvisioningTemplate.ComposedLook.BackgroundFile, "");
        //        model.PnpProvisioningTemplate.ComposedLook.ColorFile =
        //            _templateFilesBlobService.GetFileValue(model.PnpProvisioningTemplate.ComposedLook.BackgroundFile, "");
        //        model.PnpProvisioningTemplate.ComposedLook.FontFile =
        //            _templateFilesBlobService.GetFileValue(model.PnpProvisioningTemplate.ComposedLook.FontFile, "");
        //        model.PnpProvisioningTemplate.ComposedLook.SiteLogo =
        //            _templateFilesBlobService.GetFileValue(model.PnpProvisioningTemplate.ComposedLook.SiteLogo, "");
        //        model.TemplateFile = model.TemplateFile;
        //        model.TemplateValid = true;
    }
}