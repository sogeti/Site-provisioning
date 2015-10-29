using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using Sogeti.Provisioning.Business.Services;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;
using Sogeti.Provisioning.WebJob.Helpers;
using Sogeti.Provisioning.WebJob.Sharepoint.Helpers;

namespace Sogeti.Provisioning.WebJob.Sharepoint
{
    public class SiteManager
    {
        private readonly ActionRequest _actionRequest;
        private readonly ClientContext _ctx;
        private readonly SiteTemplate _siteTemplate;

        public SiteManager(ActionRequest actionRequest, ClientContext ctx)
        {
            _actionRequest = actionRequest;
            _ctx = ctx;
            _siteTemplate = GetSiteTemplate();
        }

        public void TryDeleteSite()
        {
         
        }

        private async Task<SiteTemplate> GetSiteTemplateTemplate()
        {
            var siteTemplateService = new SiteTemplateService(new SiteTemplateRepository());
            return await siteTemplateService.Read(_actionRequest.SiteTemplateName);
        }

        public void CreateSite()
        {
            Updatehelper.UpdateProgressView("Creating site", _actionRequest);

            var newWeb = _ctx.Web.CreateWeb(
                new OfficeDevPnP.Core.Entities.SiteEntity()
                {
                    Title = _actionRequest.Name,
                    Url = _actionRequest.Name,
                    Description = _actionRequest.Description,
                    Template = "STS#0"
                });

            Updatehelper.UpdateProgressView("Created", _actionRequest);
        }

        public void ApplyCustomTemplateToSite()
        {
            var provisioningTemplate = _siteTemplate.PnpTemplate;
            _ctx.Web.ApplyProvisioningTemplate(provisioningTemplate);

            var applyingInfo = new ProvisioningTemplateApplyingInformation
            {
                ProgressDelegate =
                    (message, step, total) =>
                    {
                        Updatehelper.UpdateProgressView($"{step}/{total} Provisioning {message}", _actionRequest);
                    }
            };

            _ctx.Web.ApplyProvisioningTemplate(provisioningTemplate, applyingInfo);
        }

        private SiteTemplate GetSiteTemplate()
        {
            var siteTemplateService = new SiteTemplateService(new SiteTemplateRepository());
            return siteTemplateService.Read(_actionRequest.SiteTemplateName).Result;
        }

    }
}
