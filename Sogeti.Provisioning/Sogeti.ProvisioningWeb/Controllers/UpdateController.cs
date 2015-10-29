using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using Sogeti.Provisioning.Business.SignalRHubs;
using Sogeti.Provisioning.Business;
using Sogeti.Provisioning.Domain;
using System.IO;
using Newtonsoft.Json;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Business.Services;

namespace Sogeti.ProvisioningWeb.Controllers
{
    public class UpdateController : ApiController
    {
        private readonly ProgressHub _progressHub = new ProgressHub();
        private INotificationService _notificationService;

        public HttpResponseMessage Post([FromBody]JToken jsonbody)
        {
            // Process the jsonbody
            var requestStream = HttpContext.Current.Request.InputStream;

            Stream req = requestStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);

            string json = jsonbody.ToString();

            ActionRequest ar = null;
            try
            {
                // assuming JSON.net/Newtonsoft library from http://json.codeplex.com/
                ar = JsonConvert.DeserializeObject<ActionRequest>(json);
            }
            catch
            {
                // Try and handle malformed POST body
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _notificationService = new NotificationService();
            ProgressState ps = ar.ToProgressObject(ar, ar.StateString);
            _notificationService.SendNotification(ps);
           
            _progressHub.sendProgressUpdate(ps);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}