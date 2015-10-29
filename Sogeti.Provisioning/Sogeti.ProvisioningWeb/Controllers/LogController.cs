using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Domain;
using Sogeti.Provisioning.Business;
using Microsoft.AspNet.SignalR;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Business.Services;

namespace Sogeti.ProvisioningWeb.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _logService;
        private INotificationService _notificationService;

        public LogController(ILogService logService, INotificationService notificationService)
        {
            _logService = logService;
        }


        [SharePointContextFilter]
        public ActionResult Queue()
        {
            _notificationService = new NotificationService();

            var model = _logService.GetAllLogs().ToList().OrderByDescending(d => d.Timestamp);
            var notification = _notificationService.GetAllNotifications().ToList().OrderByDescending(d => d.Timestamp);

            var evm = new EntityViewModel
            {
                logEntities = model,
                notificationEntities = notification
            };
            return View(evm);
        }

        [HttpPost]
        public ActionResult ProgressUpdate()
        {

            return new HttpStatusCodeResult(HttpStatusCode.OK);

        }
        [SharePointContextFilter]
        public ActionResult RequestProgress(ActionRequest request)
        {
            return View(request);
        }
    }
}