using Autofac;
using Autofac.Core;
using OrganigramService.StartUp.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganigramService.StartUp
{
    public static class Bootstrapper
    {
        private static MainWindow visual;
        private static ILifetimeScope lifeScope;
        public static IContainer AppContainer { get; set; }

        public static MainWindow Visual
        {
            get
            {
                visual = lifeScope.Resolve<MainWindow>();
                return visual;
            }
        }

        /// <summary>
        /// Register all instances
        /// </summary>
        /// <returns></returns>
        private static IContainer BuildContext()
        {
            var builder = new ContainerBuilder();

            UserResourcesInstaller.Install(builder);
            DomainInstaller.Install(builder);

            var container = builder.Build();

            return container;
        }

        /// <summary>
        /// Starts the app and begin the lifetimescope
        /// </summary>
        public static void StartApp()
        {
            AppContainer = BuildContext();

            lifeScope = AppContainer.BeginLifetimeScope();

            StartServices();
        }

        /// <summary>
        /// Resolve all Instances
        /// </summary>
        public static void StartServices()
        {
            DomainInstaller.Start();
        }

        /// <summary>
        /// Resolve the instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            if (lifeScope == null)
            {
                throw new SystemException("Boostrapper not started");
            }

            return lifeScope.Resolve<T>(Array.Empty<Parameter>());
        }

        /// <summary>
        /// Resolve the instance according to the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Resolve(Type type)
        {
            if (lifeScope == null)
            {
                throw new SystemException("Boostrapper not started");
            }

            return lifeScope.Resolve(type);
        }
    }
}
