using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Sogeti.Provisioning.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Sogeti.Provisioning.DataAccess.Repositories
{
    public class NotificationTable
    {
        public NotificationTable()
        {
            Table = GetTable();
        }

        public CloudTable Table { get; set; }

        private static CloudTable GetTable()
        {
            string accountName = WebConfigurationManager.AppSettings["TableStorageAccountName"];
            string accountKey = WebConfigurationManager.AppSettings["TableStorageAccountKey"];

            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

            CloudTableClient client = account.CreateCloudTableClient();

            CloudTable table = client.GetTableReference("ActionRequestNotification");
            table.CreateIfNotExists();

            return table;
        }

        public IEnumerable<NotificationEntity> GetAllActionProcesCloudServices()
        {
            TableQuery<NotificationEntity> query = new TableQuery<NotificationEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, "###"));

            var allLogs = Table.ExecuteQuery(query);
            return allLogs;
        }

        public void AddNotificationToTable(ProgressState request)
        {
            var ne = new NotificationEntity(request);

            TableOperation insertOperation = TableOperation.InsertOrReplace(ne);
            TableResult result = Table.Execute(insertOperation);
        }

    }

    public class NotificationEntity : TableEntity
    {
        public NotificationEntity() { }

        public NotificationEntity(ProgressState request) : base(request.StringTimeStamp, request.SiteTemplateName)
        {
            State state = request.State;
            switch (state)
            {
                case State.Created:
                    ProvisioningState = "Created";
                    break;
                case State.Provisioning:
                    ProvisioningState = "Provisioning";
                    break;
                case State.Failed:
                    ProvisioningState = "Failed";
                    break;
                case State.Queued:
                    ProvisioningState = "Queued";
                    break;
                case State.Waiting:
                    ProvisioningState = "Waiting";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            RequestID = request.RequestID;
            CreatedSiteName = request.Name;
            ByUser = request.User;
            if (request.IsSiteCollection)
                CreatedSiteUrl = $"https://{request.TenantName}.sharepoint.com/{request.SiteCollectionRequest.ManagedPath}/{request.Name}";
            else
                CreatedSiteUrl = request.Url + "/" + request.Name;

            UsedTemplate = request.SiteTemplateName;
            Description = request.Description;
            StringTimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public Guid RequestID { get; set; }

        public string UsedTemplate { get; set; }


        public string ProvisioningState { get; set; }

        public string CreatedSiteUrl { get; set; }

        public string CreatedSiteName { get; set; }

        public string ByUser { get; set; }

        public string Description { get; set; }

        public string StringTimeStamp { get; set; }
    }
}
