using Saver.DataAccess.Interfaces;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Saver.Repositories.Implementations.Goal
{
    /// <summary>
    /// The standard goal repository from which we will
    /// return and interact with goal data on the system
    /// </summary>
    public class GoalRepository : SqlRepositoryBase, IGoalRepository
    {
        private readonly ITypedDataAccess typedDataAccess = null;

        /// <summary>
        /// Creates a new Goal Repository which will access
        /// the data store using the data access provided
        /// </summary>
        /// <param name="dataAccess">The data access for the repository</param>
        /// <param name="sqlStringService">The SQL string service that provides the Sql statements to be used</param>
        public GoalRepository(ITypedDataAccess dataAccess, ISqlStringService sqlStringService) 
            : base(dataAccess, sqlStringService)
        {
            typedDataAccess = dataAccess;
        }

        public Model.Goal GetGoal(int goalID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.Goal> GetGoalsForUser(int userID)
        {
            throw new NotImplementedException();
        }
    }
}