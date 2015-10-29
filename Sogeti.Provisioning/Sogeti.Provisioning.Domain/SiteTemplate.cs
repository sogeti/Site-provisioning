using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Sogeti.Provisioning.Domain
{
    public class SiteTemplate
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        public DateTime CreationTimeStamp{ get; set; }

        [Required]
        // because this one isn't in the pnp template??
        public string RootTemplate { get; set; }

        [Required]
        public bool UsesDefaultTemplateFiles { get; set; }

        [Required]
        public ProvisioningTemplate PnpTemplate{ get; set; }

    }
}



