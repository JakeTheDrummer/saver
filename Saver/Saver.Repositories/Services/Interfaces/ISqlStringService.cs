using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Services.Interfaces
{
    /// <summary>
    /// Provides the ability to lookup and return
    /// Sql strings that are available for use
    /// </summary>
    public interface ISqlStringService
    {
        /// <summary>
        /// Returns the SQL stored in the resource at the
        /// given resource name on the system
        /// </summary>
        /// <param name="resourceName">The name of the resource on the system</param>
        /// <returns>The SQL from the resource</returns>
        string GetSqlFromResource(string resourceName);
    }
}
