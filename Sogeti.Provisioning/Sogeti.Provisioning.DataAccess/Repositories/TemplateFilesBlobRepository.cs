using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Sogeti.Provisioning.DataAccess.Interface;
using System;

namespace Sogeti.Provisioning.DataAccess.Repositories
{
    public class TemplateFilesBlobRepository : ITemplateFilesBlobRepository
    {
        private readonly string _accountName = WebConfigurationManager.AppSettings["StorageAccountName"];
        private readonly string _accountKey = WebConfigurationManager.AppSettings["StorageAccountKey"];
        private static readonly string ContainerName = WebConfigurationManager.AppSettings["SiteProvisioningBlobContainer"];
        private static CloudStorageAccount _storageAccount;


        public TemplateFilesBlobRepository()
        {
            var credentials = new StorageCredentials(_accountName, _accountKey);
            _storageAccount = new CloudStorageAccount(credentials, useHttps: true);
        }

        public async Task SaveToBlob(HttpPostedFileBase siteProvisioningFile, string nameTemplate)
        {
            HttpPostedFileBase file = siteProvisioningFile;
            var container = GetBlobContainer();
            await SaveToBlobStorage(container, file, nameTemplate);
        }

        public string GetFile(string fileName, string nameTemplate = "")
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = GetBlobContainer();
            //var container = blobClient.GetContainerReference(ContainerName);
            int pos;

            foreach (IListBlobItem item in container.ListBlobs(Path.GetFileNameWithoutExtension(nameTemplate), true))
            {
                var blockBlob = item as CloudBlockBlob;
                if (blockBlob != null)
                {
                    var blob = blockBlob;
                    pos = blob.Name.LastIndexOf("/", StringComparison.Ordinal) + 1;
                    if (fileName == blob.Name.Substring(pos, blob.Name.Length - pos))
                    {
                        return blob.Uri.ToString();
                    }
                }
                else if (item is CloudPageBlob)
                {
                    var pageBlob = (CloudPageBlob)item;

                    return pageBlob.Uri.ToString();

                }
                else if (item is CloudBlobDirectory)
                {
                    var directory = (CloudBlobDirectory)item;

                    return directory.Uri.ToString();
                }
            }

            return "";
        }

        public string GetFileExt(string ext, string nameTemplate = "")
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs(Path.GetFileNameWithoutExtension(nameTemplate), true))
            {
                var blockBlob = item as CloudBlockBlob;
                if (blockBlob != null)
                {
                    var blob = blockBlob;
                    var pos = blob.Uri.ToString().LastIndexOf(".", StringComparison.Ordinal) + 1;
                    if (ext == blob.Uri.ToString().Substring(pos, blob.Uri.ToString().Length - pos))
                        return blob.Uri.ToString();
                }
                else if (item is CloudPageBlob)
                {
                    var pageBlob = (CloudPageBlob)item;

                    return pageBlob.Uri.ToString();

                }
                else if (item is CloudBlobDirectory)
                {
                    var directory = (CloudBlobDirectory)item;

                    return directory.Uri.ToString();
                }
            }

            return "";
        }

        public bool CheckFile(string scriptSrc)
        {

            throw new System.NotImplementedException();

    }


        private static async Task SaveToBlobStorage(CloudBlobContainer container, HttpPostedFileBase file, string nameTemplate)
        {
            var pos = file.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            var blockBlob = container.GetBlockBlobReference(Path.GetFileNameWithoutExtension(nameTemplate) + "/" + file.FileName.Substring(pos, file.FileName.Length - pos));
            using (var fileStream = file.InputStream)
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }

        private static CloudBlobContainer GetBlobContainer()
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }
    }
}
