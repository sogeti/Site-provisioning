using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using System.ComponentModel.DataAnnotations;

namespace Sogeti.Provisioning.Domain
{
    public class SiteCollectionTemplate :  SiteCollectionRequest
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [AllowHtml]
        public string RootTemplate { get; set; }
        [Required]
        public ProvisioningTemplate PnpTemplate { get; set; }


    }
}
