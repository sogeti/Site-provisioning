using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;
using OfficeDevPnP.Core.Framework.Provisioning.Model;

namespace Sogeti.Provisioning.Business.Services
{
    public class TemplateFilesBlobService: ITemplateFilesBlobService
    {
        private readonly ITemplateFilesBlobRepository _templateFilesBlobRepository;

        public TemplateFilesBlobService(ITemplateFilesBlobRepository templateFilesBlobRepository)
        {
            _templateFilesBlobRepository = templateFilesBlobRepository;
        }
        public bool AllFilesAvailable(SiteTemplate siteTemplate)
        {

            var colorFileAvailable =  _templateFilesBlobRepository.CheckFile(siteTemplate.PnpTemplate.ComposedLook.ColorFile);
            var backgroundFileAvailable =  _templateFilesBlobRepository.CheckFile(siteTemplate.PnpTemplate.ComposedLook.BackgroundFile);
            var fontFileAvailable =  _templateFilesBlobRepository.CheckFile(siteTemplate.PnpTemplate.ComposedLook.FontFile);
            var siteLogoUrlAvailable =  _templateFilesBlobRepository.CheckFile(siteTemplate.PnpTemplate.ComposedLook.SiteLogo);

            var customActions = siteTemplate.PnpTemplate.CustomActions.SiteCustomActions.Concat(siteTemplate.PnpTemplate.CustomActions.WebCustomActions);

            var sourcesAvailable = false;
            foreach (var customAction in customActions)
            {
                sourcesAvailable =  _templateFilesBlobRepository.CheckFile(customAction.ScriptSrc);
            }

            return colorFileAvailable != false &&
                backgroundFileAvailable != false &&
                fontFileAvailable != false &&
                siteLogoUrlAvailable != false && 
                sourcesAvailable != false;
        }

        public async Task SaveFile(HttpPostedFileBase siteProvisioningFile, string nameTemplate)
        {
            await _templateFilesBlobRepository.SaveToBlob(siteProvisioningFile, nameTemplate);
        }

        public string FileInBlob(string fileName, string nameTemplate)
        {
            //parse the filename to something what can be found in blob
            return  _templateFilesBlobRepository.GetFile(fileName, nameTemplate);
            
        }

        public string FileInBlobExt(string extensie, string nameTemplate)
        {
            return  _templateFilesBlobRepository.GetFileExt(extensie, nameTemplate);
        }

     public string GetFileValue(string file, string templateName)
        {
            if (string.IsNullOrEmpty(file)) return null;

            var fileName = Path.GetFileName(file);
            var fileIsInBlob = FileInBlob(fileName, templateName);
            return !string.IsNullOrEmpty(fileIsInBlob) ? fileIsInBlob : null;
        }

    }
}
