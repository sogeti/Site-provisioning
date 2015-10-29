using Sogeti.Provisioning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface IProgressHub
    {
        void sendProgressUpdate(ProgressState ps);
    }
}
