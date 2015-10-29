using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Services;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;



namespace Sogeti.Provisioning.Domain
{

    public enum State { Waiting = 1, Provisioning, Created, Failed, Queued, Done };

    public class ActionRequest
    {
        public Guid RequestID { get; set; }

        public string User { get; set; }

        public string SiteTemplateName { get; set; }

        public string Url  { get; set; }

        public string TenantName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsSiteCollection { get; set; }

        public SiteCollectionRequest SiteCollectionRequest { get; set; }

        public string StateString { get; set; }

        public ProgressState ToProgressObject(ActionRequest request, string update)
        {
            State state = StringToState(update);

            var prg = new ProgressState(request)
            {
                RequestID = request.RequestID,
                Description = request.Description,
                SiteTemplateName = request.SiteTemplateName,
                TenantName = request.TenantName,
                Url = request.Url,
                User = request.User,
                Name = request.Name,
                StringTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                StateString = update,
                State = state
            };

            return prg;
        }

        public State StringToState(string update)
        {
            var state = State.Waiting;
            switch (update)
            {
                case "Created":
                    state = State.Created;
                    break;
                case "Creating Site":
                    state = State.Created;
                    break;
                case "Failed":
                    state = State.Failed;
                    break;
                case "Provisioning":
                    state = State.Provisioning;
                    break;
                case "Queued":
                    state = State.Queued;
                    break;
                case "Done":
                    state = State.Done;
                    break;
            }
            return state;
        }
    }
}
