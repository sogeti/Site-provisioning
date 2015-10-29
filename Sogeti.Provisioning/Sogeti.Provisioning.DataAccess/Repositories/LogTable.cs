using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.Repositories
{
    public class LogTable
    {
        public LogTable()
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

            CloudTable table = client.GetTableReference("ActionRequestLog");
            table.CreateIfNotExists();

            return table;
        }

        public IEnumerable<LogEntity> GetAllActionProcesCloudServices()
        {
            TableQuery<LogEntity> query = new TableQuery<LogEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, "###"));

            var allLogs = Table.ExecuteQuery(query);
            return allLogs;
        }

        public void AddlogToTable(ActionRequest request, State state)
        {
            var le = new LogEntity(request, state);

            TableOperation insertOperation = TableOperation.InsertOrReplace(le);
            TableResult result = Table.Execute(insertOperation);
        }
    }
    public class EntityViewModel
    {
        public IOrderedEnumerable<LogEntity> logEntities { get; set; }

        public IOrderedEnumerable<NotificationEntity> notificationEntities { get; set; }
    }

    public class LogEntity : TableEntity
    {
        public LogEntity()
        { }

        public LogEntity(ActionRequest request, State state): base(request.User, request.Name)
        {
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
                    ProvisioningState ="Waiting";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            RequestID = request.RequestID;
            CreatedSiteName = request.Name;
            ByUser = request.User;
            if(request.IsSiteCollection)
                CreatedSiteUrl = $"https://{request.TenantName}.sharepoint.com/{request.SiteCollectionRequest.ManagedPath}/{request.Name}";
            else
                CreatedSiteUrl = request.Url + "/" + request.Name;

            UsedTemplate = request.SiteTemplateName;
        }

        public Guid RequestID { get; set; }

        public string UsedTemplate { get; set; }


        public string ProvisioningState { get; set; }

        public string CreatedSiteUrl { get; set; }

        public string CreatedSiteName { get; set; }

        public string ByUser { get; set; }
    }
}
