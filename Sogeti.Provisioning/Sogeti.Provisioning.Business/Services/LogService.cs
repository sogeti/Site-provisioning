using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Services
{
    public class LogService: ILogService   
    {
        public IEnumerable<LogEntity> GetAllLogs()
        {
            var logs = new LogTable();
            return logs.GetAllActionProcesCloudServices();
        }

        public void SendLog(ActionRequest request, State state)
        {
            var logs = new LogTable();
            logs.AddlogToTable(request, state);
        }
    }
}
