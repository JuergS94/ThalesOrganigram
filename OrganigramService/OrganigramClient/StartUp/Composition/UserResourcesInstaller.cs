using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramClient.StartUp.Composition
{
    public static class UserResourcesInstaller
    {
        /// <summary>
        /// Register all Instances
        /// </summary>
        /// <param name="builder"></param>
        public static void Install(ContainerBuilder builder)
        {
            _ = builder.RegisterType<MainWindow>().AsSelf();
            var assembly = Assembly.GetExecutingAssembly();
            _ = builder.RegisterAssemblyTypes(assembly)
                .InNamespace("OrganigramClient.ViewModel")
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}
