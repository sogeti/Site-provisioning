using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Model;

namespace Sogeti.Provisioning.Domain
{
    public class PnpFile
    {
        [JsonProperty(PropertyName = "id")]
        [Required]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Filename")]
        [Required]
        public string Filename { get; set; }


        [Required]
        public ProvisioningTemplate PnpTemplate { get; set; }

       
    }
}
