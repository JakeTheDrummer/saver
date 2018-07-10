using Saver.DataAccess.Credentials;
using Saver.DataAccess.Factories;
using Saver.DataAccess.Factories.Interfaces;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Implementations.Goal;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services;
using Saver.Repositories.Services.Interfaces;
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

            //Ensure we have repositories to handd
            container.RegisterType<IGoalRepository, GoalRepository>
            (
                new InjectionConstructor(container.Resolve<ITypedDataAccess>(), container.Resolve<ISqlStringService>())
            );
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}