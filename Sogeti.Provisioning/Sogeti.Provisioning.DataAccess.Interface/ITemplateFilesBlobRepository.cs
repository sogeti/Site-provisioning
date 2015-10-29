using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sogeti.Provisioning.DataAccess.Interface
{
    public interface ITemplateFilesBlobRepository
    {
        Task SaveToBlob(HttpPostedFileBase siteProvisioningFile, string nameTemplate);

        string GetFile(string fileName, string nameTemplate);

        string GetFileExt(string ext, string nameTemplate);
        bool CheckFile(string scriptSrc);
    }
}
