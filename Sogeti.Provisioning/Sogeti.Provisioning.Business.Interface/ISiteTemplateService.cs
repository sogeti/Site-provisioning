using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface ISiteTemplateService
    {
        Task<SiteTemplate> Read(string siteTemplateNaam);
        Task<IEnumerable<SiteTemplate>> Read();
        Task Insert(SiteTemplate siteTemplate);
        Task Update(SiteTemplate newSiteTemplate, SiteTemplate oldSiteTemplate);
        Task Delete(SiteTemplate siteTemplate);
    }
}
