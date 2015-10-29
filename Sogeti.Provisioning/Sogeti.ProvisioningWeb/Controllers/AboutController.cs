using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sogeti.Provisioning.Domain;

namespace Sogeti.ProvisioningWeb.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}