using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Diagnostics;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using Sogeti.Provisioning.Business.Services;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;
using Sogeti.Provisioning.WebJob.Helpers;
using Sogeti.Provisioning.WebJob.Sharepoint.Helpers;

namespace Sogeti.Provisioning.WebJob.Sharepoint
{
    internal class SiteCollectionManager
    {
        private readonly ActionRequest _actionRequest;
        private readonly ClientContext _ctx;
        private readonly SiteTemplate _siteTemplate;
        private readonly SiteCollectionRequest _siteCollectionRequest;

        public SiteCollectionManager(ActionRequest actionRequest, ClientContext ctx)
        {
            _actionRequest = actionRequest;
            _siteCollectionRequest = actionRequest.SiteCollectionRequest;
            _ctx = ctx;
            _siteTemplate =  GetSiteTemplate();
        }

        public void TryDeleteSiteCollection()
        {
            var tenant = new Tenant(_ctx);

            var webUrl =
                $"https://{_actionRequest.TenantName}.sharepoint.com/{_siteCollectionRequest.ManagedPath}/{_actionRequest.Name}";

            if (!tenant.SiteExists(webUrl))
            {
                tenant.DeleteSiteCollection(webUrl, false);
            }
        }

        public string CreateSiteCollection()
        {
            var user = GetSpUser.ResolveUserById(_ctx, _actionRequest.User);
            var tenant = new Tenant(_ctx);

            var webUrl =
                $"https://{_actionRequest.TenantName}.sharepoint.com/{_siteCollectionRequest.ManagedPath}/{_actionRequest.Name}";
            if (tenant.SiteExists(webUrl))
            {
                // "Site already existed. Used URL - {0}"
            }
            else
            {
               
                var newsite = new SiteCreationProperties
                {
                    Title = _actionRequest.Name,
                    Url = webUrl,
                    Owner = user.Email,
                    Template = "STS#0",
                    Lcid = _siteCollectionRequest.Lcid,
                    TimeZoneId = _siteCollectionRequest.TimeZoneId,
                    StorageMaximumLevel = _siteCollectionRequest.StorageMaximumLevel,
                    StorageWarningLevel = _siteCollectionRequest.StorageMaximumLevel,
                    UserCodeMaximumLevel = 0,
                    UserCodeWarningLevel = 0
                };
                
                SpoOperation op = tenant.CreateSite(newsite);
                _ctx.Load(tenant);
                _ctx.Load(op, i => i.IsComplete);
                _ctx.ExecuteQuery();

                var step = 0;
                while (!op.IsComplete)
                {
                    step = step + 1;
                    Updatehelper.UpdateProgressView($"{step} Creating site collection", _actionRequest);
                    Thread.Sleep(10000);
                    op.RefreshLoad();
                    _ctx.ExecuteQuery();
                }
            }
            var site = tenant.GetSiteByUrl(webUrl);
                var web = site.RootWeb;
                web.Description = _actionRequest.Description;
                web.Update();
                _ctx.Load(web);
            _ctx.ExecuteQuery();

            return web.Url;
        }

        private SiteTemplate GetSiteTemplate()
        {
            var siteTemplateService = new SiteTemplateService(new SiteTemplateRepository());
            return siteTemplateService.Read(_actionRequest.SiteTemplateName).Result;
        }

        public void ApplyCustomTemplateToSite()
        {
            var provisioningTemplate = _siteTemplate.PnpTemplate;
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
    }
}
