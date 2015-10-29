using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sogeti.Provisioning.Domain;

namespace Sogeti.Provisioning.DataAccess.Interface
{
    public interface IPnpFileRepository
    {
        Task<IEnumerable<PnpFile>> GetPnpFiles();

        Task Insert(PnpFile pnpFiles);
        Task Update(PnpFile oldPnpFiles, PnpFile newPnpFiles);
        Task Delete(PnpFile pnpFiles);
    }
}
