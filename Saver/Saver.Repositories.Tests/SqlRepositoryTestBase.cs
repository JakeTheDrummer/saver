using Moq;
using Saver.DataAccess.Interfaces;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Tests
{
    /// <summary>
    /// Provides a base class for all SQL repository
    /// test classes, allowing mock objects to be accessed
    /// </summary>
    public abstract class SqlRepositoryTestBase
    {
        protected readonly Mock<ITypedDataAccess> mockDataAccess;
        protected readonly Mock<ISqlStringService> mockSqlStringService;
        private readonly string defaultReturnSQLStatement;
        /// <summary>
        /// Creates a new repository test base class and initializes
        /// commonly used mock objects
        /// </summary>
        public SqlRepositoryTestBase() : this(null) { }

        /// <summary>
        /// Creates a new repository test base class and initializes
        /// commonly used mock objects
        /// </summary>
        /// <param name="defaultReturnSQLStatement">The default SQL statement to return from the Sql Resource Service when asked</param>
        public SqlRepositoryTestBase(string defaultReturnSQLStatement)
        {
            mockDataAccess = new Mock<ITypedDataAccess>();
            mockSqlStringService = new Mock<ISqlStringService>();
            this.defaultReturnSQLStatement = defaultReturnSQLStatement;

            mockSqlStringService.Setup(ss => ss.GetSqlFromResource(It.IsAny<string>())).Returns(defaultReturnSQLStatement);
        }
    }
}