using Autofac;
using OrganigramEssentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.StartUp.Composition
{
    public static class DomainInstaller
    {
        /// <summary>
        /// Register all instances
        /// </summary>
        /// <param name="builder"></param>
        public static void Install(ContainerBuilder builder)
        {
            builder.RegisterType<TcpIpServer>().As<ITcpIpCommunication>().SingleInstance();
            builder.RegisterType<DataBaseHandler>().As<IDataBaseHandler>().SingleInstance();
            builder.RegisterType<OrganizationDatabase>().As<OrganizationDatabase>().SingleInstance();
        }

        /// <summary>
        /// Resolve all instances
        /// </summary>
        public static void Start()
        {
            Bootstrapper.Resolve<ITcpIpCommunication>();
            Bootstrapper.Resolve<IDataBaseHandler>();
            Bootstrapper.Resolve<OrganizationDatabase>();
        }
    }
}
