using Sogeti.Provisioning.DataAccess.Repositories;
using Sogeti.Provisioning.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface INotificationService
    {
        IEnumerable<NotificationEntity> GetAllNotifications();

        void SendNotification(ProgressState request);
    }
}
