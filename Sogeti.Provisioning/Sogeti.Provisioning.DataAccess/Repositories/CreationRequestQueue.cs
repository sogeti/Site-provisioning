using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.Repositories
{
    public class CreationRequestQueue: ICreationRequestQueue
    {
        public void PutRequestInQueue(ActionRequest request)
        {
            var queue = GetCloudQueue();

            var message = new CloudQueueMessage(JsonConvert.SerializeObject(request));
            queue.AddMessage(message);
        }

        private static CloudQueue GetCloudQueue()
        {
            var connectionString = ConfigurationManager.AppSettings["AzureWebJobsDashboard"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("siteprovisioningqueue");
            queue.CreateIfNotExists();
            return queue;
        }

        public int MessagesInQueue()
        {
            var queue = GetCloudQueue();
            var cachedMessageCount = queue.ApproximateMessageCount;

            if (cachedMessageCount != null)
                return (short)cachedMessageCount;

            return 0;
        } 
    }
}
