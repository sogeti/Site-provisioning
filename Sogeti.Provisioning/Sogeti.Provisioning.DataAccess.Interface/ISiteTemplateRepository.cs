using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.Interface
{
    public interface ISiteTemplateRepository
    {
        Task<IEnumerable<SiteTemplate>> GetSiteTemplates();

        Task Insert(SiteTemplate template);
        Task Update(SiteTemplate oldTemplate, SiteTemplate newTemplate);
        Task Delete(SiteTemplate template);

    }
}
