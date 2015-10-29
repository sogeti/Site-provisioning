using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.SharePoint.Client;
using Sogeti.Provisioning.Business;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Domain;
using Sogeti.ProvisioningWeb.Models;
using System.Threading;
using System.Threading.Tasks;


namespace Sogeti.ProvisioningWeb.Controllers
{
    public class SiteController : Controller
    {
        private readonly ISiteTemplateService _siteTemplateService;
        private readonly ICreateRequestService _createRequestService;
        private readonly ILogService _logService;   

        public SiteController(ISiteTemplateService siteTemplateService, ICreateRequestService createRequestService, ILogService logService)
        {
            _siteTemplateService = siteTemplateService;
            _createRequestService = createRequestService;
            _logService = logService;
        }

        [SharePointContextFilter]
        public async Task<ActionResult> Index()
        {
            var availableSiteTemplates = await _siteTemplateService.Read();
            var model = new SiteTemplateCreationModel {SiteCreationTemplates = availableSiteTemplates};


            return View(model);
        }

        [SharePointContextFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateSite(SiteTemplateCreationModel siteTemplateCreationModel)
        {
            User spUser = null;
            Site spSite = null;
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.Email);
                    clientContext.ExecuteQuery();

                    spSite = clientContext.Site;
                    clientContext.Load(spSite, site => site.Url);
                    clientContext.ExecuteQuery();
                }
            }

            var request = new ActionRequest
            {
                RequestID = Guid.NewGuid(),
                Name = siteTemplateCreationModel.Name,
                Description = siteTemplateCreationModel.Description,
                SiteTemplateName = siteTemplateCreationModel.SelectedSiteCreationTemplate,
                User = spUser.Email,
                Url = spSite.Url,
                TenantName = new Uri(spSite.Url).Host.Split('.')[0]
            };

            _createRequestService.PutCreateRequestInQueue(request);
            _logService.SendLog(request, State.Queued);

            return RedirectToAction("Queue", "Log", new { SPHostUrl = Request.QueryString["SPHostUrl"] });
        }
    }
}
 
