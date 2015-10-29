using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface ITemplateFilesBlobService
    {
        bool AllFilesAvailable(SiteTemplate siteTemplate);

        Task SaveFile(HttpPostedFileBase siteProvisioningFile, string nameTemplate);

        string FileInBlob (string fileName, string nameTemplate = "");

        string FileInBlobExt(string extensie, string nameTemplate = "");


        string GetFileValue(string file, string templateName);
    }
}
