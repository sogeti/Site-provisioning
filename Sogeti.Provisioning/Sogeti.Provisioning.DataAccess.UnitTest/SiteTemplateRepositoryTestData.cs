using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.UnitTest
{
    [TestClass]
    public class SiteTemplateRepositoryTestData
    {
        private SiteTemplateRepository _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new SiteTemplateRepository();
            Assert.IsNotNull(_target);
        }

        [TestMethod]
        public void LoadTestDataInDocumentDb()
        {
            //todo create testdata files
            var template = new SiteTemplate
            {
                Name = "test",
                Description = "test"
            };


            _target.Insert(template).Wait(); ;
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DeleteTestDataInDocumentDb()
        {
            //todo create testdata files
            var template = new SiteTemplate
            {
                Name = "test",
                Description = "test"
            };


            _target.Insert(template).Wait();
            Assert.IsTrue(true);
        }
    }
}
