using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Attributes
{
    /// <summary>
    /// Represents meta data that allows an SQL resource
    /// name to be specified for a method or class
    /// </summary>
    public class SqlResourceAttribute : Attribute
    {
        /// <summary>
        /// Creates a new Sql Resource attribute
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        public SqlResourceAttribute(string resourceName)
        {
            ResourceName = resourceName;
        }

        /// <summary>
        /// Returns the name of the desired resource
        /// </summary>
        public string ResourceName { get; private set; }
    }
}
