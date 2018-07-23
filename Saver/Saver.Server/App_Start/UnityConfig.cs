using Saver.DataAccess.Credentials;
using Saver.DataAccess.Factories;
using Saver.DataAccess.Factories.Interfaces;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Implementations.Goal;
using Saver.Repositories.Implementations.Milestone;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services;
using Saver.Repositories.Services.Interfaces;
using Saver.Services.Implementations;
using Saver.Services.Interfaces;
using System.Reflection;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Registration;
using Unity.WebApi;

namespace Saver.Server
{
    /// <summary>
    /// Provides the configuration for the unity component of the web server
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Registers all the required unity components
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all components with the container here
            container.RegisterType<IConnectionStringProvider, SaverDatabaseConnectionStringProvider>();
            container.RegisterType<ISqlStringService, EmbeddedResourceSqlStringService>(new InjectionConstructor(@"Resources\Sql", Assembly.GetAssembly(typeof(EmbeddedResourceSqlStringService))));
            container.RegisterType<ITypedDataAccessFactory, TypedMySQLDataAccessFactory>(new InjectionConstructor(container.Resolve<IConnectionStringProvider>()));

            //Ensure we have a data access type handy
            ITypedDataAccess typedAccess = container.Resolve<ITypedDataAccessFactory>().GetTypedDataAccess("Live");
            container.RegisterInstance(typedAccess);

            //Register repositories and services
            RegisterRepositoryLayers(container);
            RegisterServiceLayers(container);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        /// <summary>
        /// Registers all service layers ready for the application
        /// </summary>
        /// <param name="container">The container to be used for registering</param>
        private static void RegisterServiceLayers(UnityContainer container)
        {
            //Ensure we have the services ready
            container.RegisterType<IGoalService, GoalService>(new InjectionConstructor(container.Resolve<IGoalRepository>()));
            container.RegisterType<IMilestoneService, MilestoneService>(new InjectionConstructor(container.Resolve<IMilestoneRepository>()));
        }

        /// <summary>
        /// Register all repository layer entities within the unity container
        /// </summary>
        /// <param name="container">The unity container in which to register</param>
        private static void RegisterRepositoryLayers(UnityContainer container)
        {
            //Ensure we have repositories to hand
            ITypedDataAccess dataAccess = container.Resolve<ITypedDataAccess>();
            ISqlStringService sqlStringService = container.Resolve<ISqlStringService>();

            container.RegisterType<IGoalRepository, GoalRepository>(new InjectionConstructor(dataAccess, sqlStringService));
            container.RegisterType<IMilestoneRepository, MilestoneRepository>(new InjectionConstructor(dataAccess, sqlStringService));
        }
    }
}