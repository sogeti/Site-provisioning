using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Sogeti.Provisioning.DataAccess.Repositories;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Web.Configuration;
using Sogeti.Provisioning.DataAccess.Interface;

namespace Sogeti.Provisioning.DataAccess.UnitTest
{
    [TestClass]
    public class TemplateFilesBlobRepositoryTest
    {
        private TemplateFilesBlobRepository _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new TemplateFilesBlobRepository();
            Assert.IsNotNull(_target);
        }

             
        //[TestMethod]
        //public async Task GetFileTest()
        //{
        //    string fileName = "{site}SiteAssets{site}SogetiMsLogo.png";
        //    string strUri = "https://spsogeti.blob.core.windows.net/SiteProvisioningFiles/" + fileName;

        //    string output = await _target.GetFile(fileName);

        //    Assert.AreEqual(output, strUri);
            
        //}
    }
}
