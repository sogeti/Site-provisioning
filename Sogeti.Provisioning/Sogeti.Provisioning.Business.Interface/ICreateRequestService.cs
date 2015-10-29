using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface ICreateRequestService
    {
        void PutCreateRequestInQueue(ActionRequest request);
    }
}
