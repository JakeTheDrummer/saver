using Saver.DataAccess.Interfaces;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
