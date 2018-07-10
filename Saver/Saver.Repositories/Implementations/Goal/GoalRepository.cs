using Saver.DataAccess.Interfaces;
using Saver.Repositories.Attributes;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Returns the goal on the system with the
        /// given goal ID
        /// </summary>
        /// <param name="goalID">The ID of the Goal</param>
        /// <returns>The Goal with the given ID</returns>
        [SqlResource(@"Goal\GetGoalById")]
        public Model.Goal GetGoal(int goalID)
        {
            //Collect the SQL and call
            string sql = base.LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "Id", goalID }
            };

            //Load the goal
            Model.Goal goal = typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters).First();
            return goal;
        }

        /// <summary>
        /// Returns all the goals for the user
        /// </summary>
        /// <param name="userID">The ID of the User</param>
        /// <returns>The goals of the user</returns>
        public IEnumerable<Model.Goal> GetGoalsForUser(int userID)
        {
            throw new NotImplementedException();
        }
    }
}