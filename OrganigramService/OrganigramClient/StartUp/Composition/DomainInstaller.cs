using Autofac;
using OrganigramEssentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramClient.StartUp.Composition
{
    /// <summary>
    /// Static Class where all Instances can be registered and build
    /// </summary>
    public static class DomainInstaller
    {
        /// <summary>
        /// Register all Instances
        /// </summary>
        /// <param name="builder"></param>
        public static void Install(ContainerBuilder builder)
        {
            builder.RegisterType<TcpIpClient>().As<ITcpIpCommunication>().SingleInstance();
            builder.RegisterType<OrganizationDataBase>().As<OrganizationDataBase>().SingleInstance();
        }

        /// <summary>
        /// Resolve the Instances
        /// </summary>
        public static void Start()
        { 
            Bootstrapper.Resolve<ITcpIpCommunication>();
            Bootstrapper.Resolve<OrganizationDataBase>();
        }
    }
}
