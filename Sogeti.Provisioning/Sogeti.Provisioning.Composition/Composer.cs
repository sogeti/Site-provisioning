using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Mvc;
using Sogeti.Provisioning.Business.Interface;
using Sogeti.Provisioning.Business.Services;
using Sogeti.Provisioning.DataAccess.Interface;
using Sogeti.Provisioning.DataAccess.Repositories;

namespace Sogeti.Provisioning.Composition
{
    public class Composer
    {
        public static void ComposeIoC(ContainerBuilder container)
        {
            container.RegisterType<SiteTemplateService>().As<ISiteTemplateService>();
            container.RegisterType<TemplateFilesBlobService>().As<ITemplateFilesBlobService>();
            container.RegisterType<PnpFileService>().As<IPnpFileService>();

            container.RegisterType<SiteTemplateRepository>().As<ISiteTemplateRepository>();
            container.RegisterType<TemplateFilesBlobRepository>().As<ITemplateFilesBlobRepository>();
            container.RegisterType<PnpFileRepository>().As<IPnpFileRepository>();

            container.RegisterType<CreateRequestService>().As<ICreateRequestService>();
            container.RegisterType<CreationRequestQueue>().As<ICreationRequestQueue>();

            container.RegisterType<LogService>().As<ILogService>();
            container.RegisterType<NotificationService>().As<INotificationService>();
        }
    }
}
