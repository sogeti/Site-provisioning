using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Services
{
    public class PnpFileService : IPnpFileService
    {
        private readonly IPnpFileRepository _pnpFileRepository;

        public PnpFileService(IPnpFileRepository pnpFileRepository)
        {
            _pnpFileRepository = pnpFileRepository;
        }

        public async Task<PnpFile> Read(Guid fileGuid)
        {
            var allPnpFiles = await _pnpFileRepository.GetPnpFiles();
            var pnpFile = allPnpFiles.FirstOrDefault(v => v.Id == fileGuid);

            return pnpFile;
        }

        public async Task<IEnumerable<PnpFile>> Read()
        {
            var allPnpFiles = await _pnpFileRepository.GetPnpFiles();
            return allPnpFiles;
        }

        public async Task Insert(PnpFile pnpFile)
        {
            await _pnpFileRepository.Insert(pnpFile);
        }

        public async Task Update(PnpFile newPnpFile, PnpFile oldPnpFile)
        {
            await _pnpFileRepository.Update(newPnpFile, oldPnpFile);
        }

        public async Task Delete(PnpFile pnpFile)
        {
            await _pnpFileRepository.Delete(pnpFile);
        }

        public async Task<KeyValuePair<string, string>> Validate(object value)
        {
            var fileName = value.ToString();
            var resultList = new List<KeyValuePair<string, string>>();
            var allowedFileExtensions = new[] {".xml"};

            if (!allowedFileExtensions.Contains(fileName.Substring(fileName.LastIndexOf('.'))))
            {
                return new KeyValuePair<string, string>("NOK", "Please upload an .xml file");
            }

            var file = await Read();

            var result = file.Any(l => l.Filename.Equals(fileName));
            if (result)
            {
                return new KeyValuePair<string, string>("NOK", "filename already existis");
            }
            return new KeyValuePair<string, string>("OK","");
        }
    }
}
