using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.Repositories
{

    public class PnpFileRepository: IPnpFileRepository
    {
        private const string DocumentDbId = "SogetiSiteProvisioning";
        readonly string _collectionId = ConfigurationManager.AppSettings["CollectionIdPnpFiles"];

        readonly string _endPointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        readonly string _authorizationKKry = ConfigurationManager.AppSettings["AuthorizationKey"];
        private DocumentCollection DocumentCollection { get; set; }

        public async Task<IEnumerable<PnpFile>> GetPnpFiles()
        {
            var client = await GetDocumentDbClient();
            var files = client.CreateDocumentQuery<PnpFile>("dbs/" + DocumentDbId + "/colls/" + _collectionId).AsEnumerable<PnpFile>();

            return files;
        }


        public async Task Insert(PnpFile pnpFile)
        {
            try
            {
                var client = await GetDocumentDbClient();
                var document = client.CreateDocumentQuery("dbs/" + DocumentDbId + "/colls/" + _collectionId).Where(d => d.Id == pnpFile.Id.ToString()).AsEnumerable().FirstOrDefault();

                if (document == null)
                {
                     client.CreateDocumentAsync("dbs/" + DocumentDbId + "/colls/" + _collectionId, pnpFile).Wait();
                }
            }
            catch (Exception exception)
            {
                var t = exception;
                throw;
            }
        }
        public async Task Update(PnpFile oldPnpFile, PnpFile newPnpFile)
        {
            await Delete(oldPnpFile);
            await Insert(newPnpFile);
        }

        public async Task Delete(PnpFile pnpFile)
        {
            var client = await GetDocumentDbClient();
            var pnpFileToDelete = client.CreateDocumentQuery<Document>(DocumentCollection.DocumentsLink).Where(d => d.Id == pnpFile.Id.ToString()).AsEnumerable().FirstOrDefault();

            if (pnpFileToDelete != null)
                await client.DeleteDocumentAsync(pnpFileToDelete.SelfLink);
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

        public Task<bool> IsValid(string value)
        {
            throw new NotImplementedException();
        }
    }
}
