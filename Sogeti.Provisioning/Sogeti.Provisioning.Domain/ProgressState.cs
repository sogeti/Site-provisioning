using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti.Provisioning.Domain
{
    public class ProgressState : ActionRequest
    {
        public ProgressState(ActionRequest ar)
        {
            this.RequestID = ar.RequestID;
            this.Description = ar.Description;
            this.SiteTemplateName = ar.SiteTemplateName;
            this.TenantName = ar.TenantName;
            this.Url = ar.Url;
            this.User = ar.User;
            this.Name = ar.Name;
            this.TimeStamp = DateTime.Now;
            this.StringTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.StateString = ar.StateString;

        }
        public DateTime TimeStamp { get; set; }

        public String StringTimeStamp { get; set; }

        public State State { get; set; }
    }
}
