using Saver.Repositories.Services.Exceptions;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Services
{
    /// <summary>
    /// Ensures that we are able to load the embedded resources
    /// from the SQL resource at the given paths
    /// </summary>
    public class EmbeddedResourceSqlStringService : ISqlStringService
    {
        private string rootPath;
        private string _manifestModule;
        private readonly Assembly assemblyContainingResources;

        /// <summary>
        /// Creates a new embedded resource SQL reader
        /// that allows us to read from embedded SQL files
        /// </summary>
        /// <param name="rootPath">The root path to the SQL string directory</param>
        public EmbeddedResourceSqlStringService(string rootPath)
            : this(rootPath, Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Creates a new embedded resource SQL reader
        /// that allows us to read from embedded SQL files
        /// </summary>
        /// <param name="resourceRoutePath">The root path to the SQL string directory</param>
        /// <param name="assemblyContainingResources">The assembly that contains the resources</param>
        public EmbeddedResourceSqlStringService(string resourceRoutePath, Assembly assemblyContainingResources)
        {
            //Ensure we have the trailing '\'
            if (resourceRoutePath != null && !resourceRoutePath.EndsWith(@"\"))
                resourceRoutePath = $@"{resourceRoutePath}\";

            this.rootPath = resourceRoutePath;
            this.assemblyContainingResources = assemblyContainingResources;
            this._manifestModule = assemblyContainingResources.ManifestModule.Name.Replace(".dll", string.Empty);
            this.loadedResources = new Dictionary<string, string>();
        }
        #region Properties

        private Dictionary<string, string> loadedResources;

        /// <summary>
        /// Returns the cached resources for the embedded resource loader
        /// </summary>
        public Dictionary<string, string> ResourceCache
        {
            get { return loadedResources; }
            private set { loadedResources = value; }
        }


        #endregion

        /// <summary>
        /// Returns the SQL that is stored within the resources
        /// at the root path and resource name
        /// </summary>
        /// <param name="resourceName">The name of the resource we are finding</param>
        /// <returns>The SQL string that should be used</returns>
        /// <remarks>Loads of SQL will be cached</remarks>
        public string GetSqlFromResource(string resourceName)
        {
            //Collect the full resource path and attempt to load from dictionary
            string fullPathToEmbeddedResource = $"{(rootPath ?? string.Empty)}{resourceName}.sql";
            if (TryLoadFromCache(fullPathToEmbeddedResource, out string sql))
                return sql;

            //Load and store
            sql = ReadAllTextFromEmbeddedResource(fullPathToEmbeddedResource);
            TryAddToCache(fullPathToEmbeddedResource, sql);

            return sql;
        }

        #region Private Methods

        /// <summary>
        /// Reads all text from the embedded resource at the given location
        /// </summary>
        /// <param name="fullPathToEmbeddedResource">The resource to which we are being directed</param>
        /// <returns>The text from this location</returns>
        private string ReadAllTextFromEmbeddedResource(string fullPathToEmbeddedResource)
        {
            //Collect the appropriate location
            string resourceDirectory = fullPathToEmbeddedResource.Replace(@"\", ".");
            string resourceDirectoryPath = $"{_manifestModule}.{resourceDirectory}";

            //Read from the resource
            string result = null;
            using (Stream stream = assemblyContainingResources.GetManifestResourceStream(resourceDirectoryPath))
            {
                //Throw an exception stating we could not 
                //load this resource when attempting to read
                if (stream == null)
                    throw new ResourceNotFoundException(fullPathToEmbeddedResource);

                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the SQL we have loaded for this resource already.
        /// If we haven't added this yet, false will be returned
        /// </summary>
        /// <param name="resourceKey">The full file path of the resource</param>
        /// <param name="sql">The Sql parameter to which we will return</param>
        /// <returns>Whether we have a match for this Sql Resource Key</returns>
        private bool TryLoadFromCache(string resourceKey, out string sql)
        {
            return ResourceCache.TryGetValue(resourceKey, out sql);
        }

        /// <summary>
        /// Attempts to add the resource to the cache if required
        /// </summary>
        /// <param name="resourceKey">The resource to which we are pointing</param>
        /// <param name="sql">The SQL loaded for that file</param>
        private void TryAddToCache(string resourceKey, string sql)
        {
            if (!ResourceCache.ContainsKey(resourceKey))
                ResourceCache.Add(resourceKey, sql);
        }

        #endregion
    }
}