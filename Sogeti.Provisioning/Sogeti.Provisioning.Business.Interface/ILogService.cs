using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface ILogService
    {
        IEnumerable<LogEntity> GetAllLogs();

        void SendLog(ActionRequest request, State state);
    }
}
