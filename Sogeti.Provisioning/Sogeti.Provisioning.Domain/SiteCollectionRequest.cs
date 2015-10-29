using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti.Provisioning.Domain
{
    public class SiteCollectionRequest
    {
        public SiteProvisioningType ProvisioningType { get; set; }
        public string TemplateId { get; set; }
        public ManagedPathType ManagedPath { get; set; }
        public int TimeZoneId { get; set; }
        public uint Lcid { get; set; }
        public int StorageMaximumLevel { get; set; }
    }

    public enum SiteProvisioningType
    {
        Identity,
        TemplateSite
    }

    public enum ManagedPathType
    {
        Sites,
        Teams
    }
}
