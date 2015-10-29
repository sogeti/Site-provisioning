using Sogeti.Provisioning.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Services
{
    public class NotificationService : INotificationService
    {
        public IEnumerable<NotificationEntity> GetAllNotifications()
        {
            var notifications = new NotificationTable();
            return notifications.GetAllActionProcesCloudServices();
        }

        public void SendNotification(ProgressState request)
        {
            var notifications = new NotificationTable();
            notifications.AddNotificationToTable(request);
        }
    }
}
