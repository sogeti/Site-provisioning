using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sogeti.ProvisioningWeb.Models
{
    public class O365User
    {
        public O365User()
        {
            
        }

        public O365User(int lookupId, string login, string name, string email)
        {
            LookupId = lookupId;
            Login = login;
            Name = name;
            Email = email;

        }

        public int LookupId { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}