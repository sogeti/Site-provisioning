using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.Business.Interface
{
    public interface IPnpFileService
    {
        Task<PnpFile> Read(Guid fileGuid);
        Task<IEnumerable<PnpFile>> Read();
        Task Insert(PnpFile pnpFile);
        Task Update(PnpFile newPnpFile, PnpFile oldPnpFile);
        Task Delete(PnpFile pnpFile);
        Task<KeyValuePair<string, string>> Validate(object value);
    }
}
