using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.Interface
{
    public interface ICreationRequestQueue
    {
        void PutRequestInQueue(ActionRequest request);
    }
}
