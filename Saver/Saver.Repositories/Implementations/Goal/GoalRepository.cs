using Saver.DataAccess.Interfaces;
using Saver.Model;
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
        /// Creates the goal on the system for the given user
        /// and returns the persisted value
        /// </summary>
        /// <param name="userID">The ID of the user for whom this has been created</param>
        /// <param name="goal">The goal containing the information to persist</param>
        /// <returns>The goal that was persisted</returns>
        [SqlResource(@"Goal\CreateGoalForUser")]
        public Model.Goal CreateGoalForUser(int userID, Model.Goal goal)
        {
            string sql = base.LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters
            (
                new
                {
                    UserId = userID,
                    goal.Name,
                    goal.Description,
                    goal.Target,
                    StatusId = GoalStatus.Open,
                    goal.IsDefault
                }
            );

            //Execute and return the goal
            Model.Goal savedGoal = typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters).First();
            return savedGoal;
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
            Dictionary<string, object> parameters = ConvertToParameters(new { Id = goalID });

            //Load the goal
            Model.Goal goal = typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters).First();
            return goal;
        }

        /// <summary>
        /// Returns all the Goals on the system
        /// </summary>
        /// <returns>All the goals on the system</returns>
        [SqlResource(@"Goal\GetAllGoals")]
        public IEnumerable<Model.Goal> GetGoals()
        {
            string sql = LoadSqlResources().Values.First();
            return typedDataAccess.ExecuteQuery<Model.Goal>(sql);
        }

        /// <summary>
        /// Returns all the goals for the user
        /// </summary>
        /// <param name="userID">The ID of the User</param>
        /// <returns>The goals of the user</returns>
        [SqlResource(@"Goal\GetGoalsForUser")]
        public IEnumerable<Model.Goal> GetGoalsForUser(int userID)
        {
            string sql = LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters(new { UserId = userID });

            //Load the goal
            return typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters);
        }
    }
}