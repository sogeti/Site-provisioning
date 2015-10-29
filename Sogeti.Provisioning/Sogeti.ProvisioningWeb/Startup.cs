using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Sogeti.ProvisioningWeb.Startup))]

namespace Sogeti.ProvisioningWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var hubConfig = new HubConfiguration();
            hubConfig.EnableDetailedErrors = true;
            hubConfig.EnableJSONP = true;
            app.MapSignalR("/signalr", hubConfig);
        }
    }
}
