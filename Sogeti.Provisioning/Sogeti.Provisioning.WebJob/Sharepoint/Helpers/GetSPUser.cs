using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace Sogeti.Provisioning.WebJob.Sharepoint.Helpers
{
    public static class GetSpUser
    {
        public static User ResolveUserById(ClientContext ctx, string userId)
        {
            var user = ctx.Web.EnsureUser(userId);
            ctx.Load(user);
            ctx.ExecuteQuery();
          
            return user;
        }

    }
}
