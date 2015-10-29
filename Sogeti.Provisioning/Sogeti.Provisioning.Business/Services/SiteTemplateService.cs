using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Configuration;

namespace Sogeti.Provisioning.Business.Services
{
    public class SiteTemplateService: ISiteTemplateService
    {
        private readonly ISiteTemplateRepository _siteTemplateRepository;
        
        public SiteTemplateService(ISiteTemplateRepository siteTemplateRepository)
        {
            _siteTemplateRepository = siteTemplateRepository;
        }

        public async Task<SiteTemplate> Read(string siteTemplateName)
        {
            var templates = await _siteTemplateRepository.GetSiteTemplates();
            var siteCreationTemplate = templates.FirstOrDefault(v => v.Id.ToString() == siteTemplateName);

            return siteCreationTemplate;
        }

        public async Task<IEnumerable<SiteTemplate>> Read()
        {
            var templates = await _siteTemplateRepository.GetSiteTemplates();
            return templates;
        }

        public async Task Insert(SiteTemplate siteTemplate)
        {
            await _siteTemplateRepository.Insert(siteTemplate);

        }

        public async Task Update(SiteTemplate newSiteTemplate, SiteTemplate oldSiteTemplate)
        {
            await _siteTemplateRepository.Update(newSiteTemplate, oldSiteTemplate);

        }

        public async Task Delete(SiteTemplate siteTemplate)
        {
            await  _siteTemplateRepository.Delete(siteTemplate);

        }

   
    }
}
