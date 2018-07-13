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
        /// Attempts to delete the Goal on the system with the given ID
        /// </summary>
        /// <param name="id">The ID of the goal to delete on the system</param>
        /// <returns>The record we have deleted</returns>
        [SqlResource(@"Goal\DeleteGoalWithSelect")]
        public Model.Goal Delete(int id)
        {
            //Collect the SQL and call
            string sql = base.LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters(new { Id = id });

            Model.Goal removedGoal = typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters).First();
            return removedGoal;
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
            string sql = base.LoadSqlResources().Values.FirstOrDefault();
            Dictionary<string, object> parameters = ConvertToParameters(new { Id = goalID });

            //Load the goal
            Model.Goal goal = typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters).FirstOrDefault();
            return goal;
        }

        /// <summary>
        /// Returns the goal for the user with the given goal ID
        /// </summary>
        /// <param name="goaID">The ID of the Goal</param>
        /// <param name="userId">The ID of the User</param>
        /// <returns>The goal of the user</returns
        [SqlResource(@"Goal\GetGoalByIdAndUserId")]
        public Model.Goal GetGoalForUser(int goaID, int userId)
        {
            //Collect the SQL and call
            string sql = base.LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters(new { Id = goaID, UserId = userId });

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

        /// <summary>
        /// Updates the Goal on the system with the matching ID
        /// using the details within the Goal
        /// </summary>
        /// <param name="goalId">The ID of the Goal to update</param>
        /// <param name="goal">The goal containing the new information</param>
        /// <returns>The goal that was saved on the system</returns>
        [SqlResource(@"Goal\UpdateGoal")]
        public Model.Goal UpdateGoal(int goalId, Model.Goal goal)
        {
            string sql = base.LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters
            (
                new
                {
                    Id = goalId,
                    goal.Name,
                    goal.Description,
                    goal.Target,
                    StatusId = GoalStatus.Open,
                    goal.IsDefault
                }
            );

            //Update and return
            return typedDataAccess.ExecuteQuery<Model.Goal>(sql, parameters).First();
        }
    }
}