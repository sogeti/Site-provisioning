using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sogeti.Provisioning.Domain;

namespace Sogeti.ProvisioningWeb.Models
{
    public class SiteCollectionTemplateCreationModel: SiteCollectionTemplate
    {
        public IEnumerable<SelectListItem> SiteTemplateList
        {
            get
            {
                var result = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "[Choose Site Creation Template]"}
                };

                result.AddRange(SiteCreationTemplates.Select(t => new SelectListItem
                {
                    Value = t.Name,
                    Text = t.Name
                }));

                return result;
            }
        }

        public IEnumerable<SiteTemplate> SiteCreationTemplates { get; set; }

        [Display(Name = "Site Creation Templates")]
        public string SelectedSiteCreationTemplate { get; set; }
    }
}
