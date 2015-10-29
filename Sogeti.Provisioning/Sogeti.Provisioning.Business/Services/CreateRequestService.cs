using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Services
{
    public class CreateRequestService: ICreateRequestService
    {
        private readonly ICreationRequestQueue _creationRequestQueue;

        public CreateRequestService(ICreationRequestQueue creationRequestQueue)
        {
            _creationRequestQueue = creationRequestQueue;
        }

        public void PutCreateRequestInQueue(ActionRequest request)
        {
            _creationRequestQueue.PutRequestInQueue(request);

        }
    }
}
