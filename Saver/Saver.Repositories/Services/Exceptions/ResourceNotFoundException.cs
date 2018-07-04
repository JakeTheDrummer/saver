using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Services.Exceptions
{
    /// <summary>
    /// A class which informs us that a resource could not
    /// be located from an access method
    /// </summary>
    public class ResourceNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new resource not found exception
        /// </summary>
        /// <param name="resourceName">The name of the resource we attempted to load</param>
        public ResourceNotFoundException(string resourceName)
        {
            ResourceName = resourceName;
        }

        /// <summary>
        /// Returns the resource name we attempted to load
        /// </summary>
        public string ResourceName { get; private set; }
    }
}
