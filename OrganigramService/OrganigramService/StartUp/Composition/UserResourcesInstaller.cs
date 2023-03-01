using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.StartUp.Composition
{
    public static class UserResourcesInstaller
    {
        public static void Install(ContainerBuilder builder)
        {
            _ = builder.RegisterType<MainWindow>().AsSelf();
            var assembly = Assembly.GetExecutingAssembly();
            _ = builder.RegisterAssemblyTypes(assembly)
                .InNamespace("OrganigramService.ViewModel")
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}
