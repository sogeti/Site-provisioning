using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.SignalRHubs
{
    [HubName("ProgressHub")]
    public class ProgressHub : Hub, IProgressHub
    {
        private readonly IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

        public void sendProgressUpdate(ProgressState ps)
        {
            _context.Clients.All.updateProgress(ps);
        }

        public void changePhoto(string siteTemplateName)
        {
            _context.Clients.All.changePhoto(siteTemplateName);
        }
    }
}
