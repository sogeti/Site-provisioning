using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.SharePoint.Client;
using Sogeti.Provisioning.Business.Services;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;
using Sogeti.Provisioning.WebJob.Helpers;
using Sogeti.Provisioning.WebJob.Sharepoint;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Converters;
using System.Net.Http.Formatting;
using System.Security;
using System.Threading;

namespace Sogeti.Provisioning.WebJob
{
  
    public class Functions
    {
        private static readonly LogTable Log = new LogTable();

        public static void ProcessQueueMessage([QueueTrigger("siteprovisioningqueue")] ActionRequest actionRequest, TextWriter log)
        {
            try
            {
             //   Updatehelper.UpdateDashboard("Request receiveded.");

                Log.AddlogToTable(actionRequest, State.Provisioning);
                
                if (actionRequest.IsSiteCollection)
                {
                    CreateSiteCollection(actionRequest);
                    Updatehelper.UpdateProgressView("Provisioning", actionRequest);
                }
                else
                {
                    CreateSite(actionRequest);
                    Updatehelper.UpdateProgressView("Create site", actionRequest);
                }

                Log.AddlogToTable(actionRequest, State.Created);
                Console.WriteLine(actionRequest.Url + "/" + actionRequest.Name, actionRequest.Name, actionRequest.User);
                //var mail = new NotificationMail();
                //mail.SendMailNotification(actionRequest.Url + "/" + actionRequest.Name, actionRequest.Name, actionRequest.User);
                Updatehelper.UpdateProgressView("Done", actionRequest);
            }
            catch (Exception ex)
            {
                Log.AddlogToTable(actionRequest, State.Failed);
                Updatehelper.UpdateProgressView("Failed", actionRequest);

                Console.WriteLine(ex.ToString(), "Failed creating site: " + actionRequest.Url + "/" + actionRequest.Name,
                    actionRequest.User);

                //var mail = new NotificationMail();
                //mail.SendMailNotification(ex.ToString(), "Failed creating site: " + actionRequest.Url + "/" + actionRequest.Name, actionRequest.User);

                if (actionRequest.IsSiteCollection)
                {
                    var tenantAdminUri = new Uri($"https://{actionRequest.TenantName}-admin.sharepoint.com");
                    var realm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);
                    var token =
                        TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, tenantAdminUri.Authority,
                            realm).AccessToken;
                    using (var ctx = TokenHelper.GetClientContextWithAccessToken(tenantAdminUri.ToString(), token))
                    {
                        var manager = new SiteCollectionManager(actionRequest, ctx);
                        manager.TryDeleteSiteCollection();
            }
                }
                else
                {
                    var tenantAdminUri = new Uri(actionRequest.Url);
                    var realm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);
                    var token =
                        TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, tenantAdminUri.Authority,
                            realm).AccessToken;
                    using (var ctx = TokenHelper.GetClientContextWithAccessToken(tenantAdminUri.ToString(), token))
            {
                        var manager = new SiteManager(actionRequest, ctx);
                        manager.TryDeleteSite();
                    }
                }
            }
        }


        private static void CreateSiteCollection(ActionRequest actionRequest)
        {
            string siteCollectionUrl;

            var tenantAdminUri = new Uri($"https://{actionRequest.TenantName}-admin.sharepoint.com");
            var realm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);
            var token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, tenantAdminUri.Authority, realm).AccessToken;
            using (var ctx = TokenHelper.GetClientContextWithAccessToken(tenantAdminUri.ToString(), token))
            {
             
                    var manager = new SiteCollectionManager(actionRequest, ctx);
                    siteCollectionUrl = manager.CreateSiteCollection();
            }
             
            var newWebUri = new Uri(siteCollectionUrl);
            token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, newWebUri.Authority, TokenHelper.GetRealmFromTargetUrl(newWebUri)).AccessToken;
            using (var ctx = TokenHelper.GetClientContextWithAccessToken(siteCollectionUrl, token))
            {
                
                var pwd = ConfigurationManager.AppSettings["SharePointOnlineCredentials.Password"];
                var username = ConfigurationManager.AppSettings["SharePointOnlineCredentials.Username"];

                using (var secureString = new SecureString())
                {
                    foreach (var chr in pwd.ToCharArray())
                    {
                        secureString.AppendChar(chr);
                    }
                    secureString.MakeReadOnly();

                    ctx.Credentials = new SharePointOnlineCredentials(username, secureString);
                    ctx.RequestTimeout = Timeout.Infinite;

                    var manager = new SiteCollectionManager(actionRequest, ctx);
                    manager.ApplyCustomTemplateToSite();
                }
            }
        }


        private static void CreateSite(ActionRequest actionRequest)
        {
            var tenantAdminUri = new Uri(actionRequest.Url); 
            var realm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);
            var token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, tenantAdminUri.Authority, realm).AccessToken;
            using (var ctx = TokenHelper.GetClientContextWithAccessToken(tenantAdminUri.ToString(), token))
            {
                var manager = new SiteManager(actionRequest, ctx);
                manager.CreateSite();
            }

             tenantAdminUri = new Uri(actionRequest.Url + "/" + actionRequest.Name);
             realm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);
             token = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, tenantAdminUri.Authority, realm).AccessToken;
            using (var ctx = TokenHelper.GetClientContextWithAccessToken(tenantAdminUri.ToString(), token))
            {
                var manager = new SiteManager(actionRequest, ctx);
                manager.ApplyCustomTemplateToSite();
            }
        }

    }
}
