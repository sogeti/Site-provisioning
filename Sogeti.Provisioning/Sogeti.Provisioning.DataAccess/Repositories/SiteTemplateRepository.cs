using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using OfficeDevPnP.Core.Framework.Provisioning.Model;



namespace Sogeti.Provisioning.DataAccess.Repositories
{
    public class SiteTemplateRepository : ISiteTemplateRepository
    {
        private const string DocumentDbId = "SogetiSiteProvisioning";
        readonly string _collectionId = ConfigurationManager.AppSettings["CollectionId"];
        readonly string  _endPointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        readonly string _authorizationKKry = ConfigurationManager.AppSettings["AuthorizationKey"];

        private DocumentCollection DocumentCollection { get; set; }

        public async Task<IEnumerable<SiteTemplate>> GetSiteTemplates()
        {
            var client = await GetDocumentDbClient();
            var templates = client.CreateDocumentQuery<SiteTemplate>("dbs/" + DocumentDbId + "/colls/" + _collectionId).AsEnumerable<SiteTemplate>();

            return templates;
        }

        public async Task Insert(SiteTemplate template)
        {
            var client = await GetDocumentDbClient();
            var document = client.CreateDocumentQuery("dbs/" + DocumentDbId + "/colls/" + _collectionId).Where(d => d.Id == template.Id.ToString()).AsEnumerable().FirstOrDefault();

            if (document == null)
            {           
                await client.CreateDocumentAsync("dbs/" + DocumentDbId + "/colls/" + _collectionId, template);
            }
        }
        public async Task Update(SiteTemplate oldTemplate, SiteTemplate newTemplate)
        {
            await Delete(oldTemplate);
            await Insert(newTemplate);
        }
        public async Task Delete(SiteTemplate template)
        {
            var client = await GetDocumentDbClient();
            var templateToDelete = client.CreateDocumentQuery<Document>(DocumentCollection.DocumentsLink).Where(d => d.Id == template.Name).AsEnumerable().FirstOrDefault();

            if (templateToDelete != null)
                await client.DeleteDocumentAsync(templateToDelete.SelfLink);
        }

        private async Task<DocumentClient> GetDocumentDbClient()
        {
            var client = new DocumentClient(new Uri(_endPointUrl), _authorizationKKry);

            var database = client.CreateDatabaseQuery().
                Where(db => db.Id == DocumentDbId).AsEnumerable().FirstOrDefault();

            if (database == null)
            {
                database = await client.CreateDatabaseAsync(
                    new Database
                    {
                        Id = DocumentDbId
                    });    
            }

            DocumentCollection = client.CreateDocumentCollectionQuery
                ("dbs/" + database.Id).Where(c => c.Id == _collectionId).AsEnumerable().FirstOrDefault();

            if (DocumentCollection == null)
            {
                DocumentCollection = await client.CreateDocumentCollectionAsync("dbs/" + DocumentDbId,
                new DocumentCollection
                {
                    Id = _collectionId
                });

            }
           

            return client;
        }

    }
}
