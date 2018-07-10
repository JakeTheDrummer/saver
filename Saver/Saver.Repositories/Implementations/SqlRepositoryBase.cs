using Saver.DataAccess.Interfaces;
using Saver.Repositories.Attributes;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Implementations
{
    /// <summary>
    /// A repository that is based on an SQL access method.
    /// Any repository wishing to use SQL strings should
    /// extend this SQL repository base class as it provides
    /// some handy utilities
    /// </summary>
    public abstract class SqlRepositoryBase : RepositoryBase
    {
        protected readonly ISqlStringService sqlStringService;

        /// <summary>
        /// Creates a new Sql Repository Base class for any deriving classes
        /// </summary>
        /// <param name="dataAccess">The data access to be used</param>
        /// <param name="sqlStringService">The Sql String Provider</param>
        protected SqlRepositoryBase(IDataAccess dataAccess, ISqlStringService sqlStringService)
            : base(dataAccess)
        {
            this.sqlStringService = sqlStringService;
            this.LoadedResources = new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>
        /// Returns the loaded resources that have been loaded for each method name
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> LoadedResources { get; private set; } = null;

        /// <summary>
        /// Returns the Sql Resources determined by the Sql Resource Attributes
        /// associated to the calling method name. This will default to the calling
        /// member method name - so if unsure, leave this as "optional"
        /// </summary>
        /// <param name="methodName">The name of the calling method</param>
        /// <returns>A dictionary of Sql Resources</returns>
        internal Dictionary<string, string> LoadSqlResources([CallerMemberName]string methodName = null)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName), "Please ensure a method name is given for which to load resources");

            if (LoadedResources.TryGetValue(methodName, out Dictionary<string, string> resources))
                return resources;

            //Locate the attributes and resources we're interested in
            MethodInfo methodInforamtion = this.GetType().GetMethod(methodName);
            IEnumerable<SqlResourceAttribute> expectedResourceAttributes = methodInforamtion.GetCustomAttributes<SqlResourceAttribute>();
            if (expectedResourceAttributes == null || !expectedResourceAttributes.Any())
                throw new Exception("There are no Sql Resource Attributes assigned to the associated method. Please ensure at least one is added");

            resources = new Dictionary<string, string>();
            string resourceValue = null;
            foreach (SqlResourceAttribute resourceAttribute in expectedResourceAttributes)
            {
                resourceValue = this.sqlStringService.GetSqlFromResource(resourceAttribute.ResourceName);
                resources.Add(resourceAttribute.ResourceName, resourceValue);
            }

            //Update the dictionary
            LoadedResources.Add(methodName, resources);
            return resources;
        }
    }
}
