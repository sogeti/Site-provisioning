using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.UnitTest
{
    [TestClass]
    public class SiteTemplateRepositoryTest
    {
        private SiteTemplateRepository _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new SiteTemplateRepository();
            Assert.IsNotNull(_target);
        }

        [TestMethod]
        public async Task SiteTemplateRepositoryInsertTest()
        {
            var template = new SiteTemplate
            {
                Name = "test",
                Description = "test"
            };

            await _target.Insert(template);

            var templates = await _target.GetSiteTemplates();
            var siteCreationTemplate = templates.Where(v => v.Name == template.Name);

            Assert.IsNotNull(siteCreationTemplate);

            await _target.Delete(template);
        }

        [TestMethod]
        public async Task SiteTemplateRepositoryReadTest()
        {

            var template = new SiteTemplate
            {
                Name = "test",
                Description = "test"
            };


            await _target.Insert(template);

            var template2 = new SiteTemplate
            {
                Name = "test2",
                Description = "test2"
            };


            await _target.Insert(template2);


            var templates = await _target.GetSiteTemplates();
            Assert.IsTrue(templates.Count() >= 2);

            await _target.Delete(template);
            await _target.Delete(template2);
        }

        //[TestMethod]
        //public async Task SiteTemplateRepositoryDeleteTest()
        //{

        //    var template = new SiteTemplate
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "test",
        //        Description = "test"
        //    };


        //    await _target.Insert(template);

        //    Thread.Sleep(200);

        //    await _target.Delete(template);

        //    var templates = await _target.GetSiteTemplates();
        //    var siteCreationTemplate = templates.FirstOrDefault(v => v.Id == template.Id);

        //    Assert.IsNull(siteCreationTemplate);
        //}
    }
}
