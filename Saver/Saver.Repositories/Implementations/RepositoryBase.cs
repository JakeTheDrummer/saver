using Saver.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Implementations
{
    /// <summary>
    /// The base repository object that will
    /// house the information required for all
    /// repositories to interact with a data source
    /// </summary>
    public abstract class RepositoryBase
    {
        protected readonly IDataAccess data;
        /// <summary>
        /// Creates a new base repository class instance
        /// </summary>
        /// <param name="dataAccess">The data access allowing access to a data source</param>
        public RepositoryBase(IDataAccess dataAccess)
        {
            this.data = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess), "Please ensure we have a data access object from which we will interact with data");
        }
    }
}
