using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Domain;
using Sogeti.ProvisioningWeb.Models;

namespace Sogeti.ProvisioningWeb.Controllers
{
    public class SiteCollectionController : Controller
    {
        private readonly ISiteTemplateService _siteTemplateService;
        private readonly ICreateRequestService _createRequestService;
        private readonly ILogService _logService;

        public SiteCollectionController(ISiteTemplateService siteTemplateService,
            ICreateRequestService createRequestService, ILogService logService)
        {
            _siteTemplateService = siteTemplateService;
            _createRequestService = createRequestService;
            _logService = logService;
        }

        [SharePointContextFilter]
        public async Task<ActionResult> Index()
        {
            var availableSiteTemplates = await _siteTemplateService.Read();
            var model = new SiteCollectionTemplateCreationModel() {SiteCreationTemplates = availableSiteTemplates};

            model.TimeZoneId = 1;
            model.Lcid= 1033;
            model.StorageMaximumLevel = 10;

            return View(model);
        }


        [SharePointContextFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateSiteCollection(SiteCollectionTemplateCreationModel siteCollectionTemplateCreationModel)
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

            var siteCollectionRequest = new SiteCollectionRequest
            {
                Lcid = siteCollectionTemplateCreationModel.Lcid,
                ManagedPath = siteCollectionTemplateCreationModel.ManagedPath,
                ProvisioningType = siteCollectionTemplateCreationModel.ProvisioningType,
                StorageMaximumLevel = siteCollectionTemplateCreationModel.StorageMaximumLevel,
                TemplateId = siteCollectionTemplateCreationModel.TemplateId,
                TimeZoneId = siteCollectionTemplateCreationModel.TimeZoneId,
            };

            var request = new ActionRequest
            {
                Name = siteCollectionTemplateCreationModel.Name,
                Description = siteCollectionTemplateCreationModel.Description,
                SiteTemplateName = siteCollectionTemplateCreationModel.SelectedSiteCreationTemplate,
                User = spUser.Email,
                Url = spSite.Url,
                SiteCollectionRequest = siteCollectionRequest,
                TenantName = new Uri(spSite.Url).Host.Split('.')[0]
            };

            request.IsSiteCollection = true;
            _createRequestService.PutCreateRequestInQueue(request);
            _logService.SendLog(request, State.Queued);

            return RedirectToAction("Queue", "Log", new { SPHostUrl = Request.QueryString["SPHostUrl"] });
        }


        [HttpGet]
        public async Task<JsonResult> GetTemplateDescription(string SelectedSiteCreationTemplate)
        {
            var selectedGuid = Guid.Parse(SelectedSiteCreationTemplate);

            var availableSiteTemplates = await _siteTemplateService.Read();
            var description = availableSiteTemplates.Where(t => t.Id.Equals(selectedGuid)).Select(t => t.Description).FirstOrDefault();

            return Json(description, JsonRequestBehavior.AllowGet);
        }
    }
}
 
