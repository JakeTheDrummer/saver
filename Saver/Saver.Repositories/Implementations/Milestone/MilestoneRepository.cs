using Saver.DataAccess.Interfaces;
using Saver.Repositories.Attributes;
using Saver.Repositories.Interfaces;
using Saver.Repositories.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Saver.Repositories.Implementations.Milestone
{
    /// <summary>
    /// Provides an implementation of the milestone repository
    /// </summary>
    public class MilestoneRepository : SqlRepositoryBase, IMilestoneRepository
    {
        private readonly ITypedDataAccess typedDataAccess = null;
        
        /// <summary>
        /// Creates a new Milestone Repository using the typed data access
        /// and SQL String Service providing SQL resources
        /// </summary>
        /// <param name="dataAccess">The data access (typed) we shall use</param>
        /// <param name="sqlStringService">The service containing SQL strings</param>
        public MilestoneRepository(ITypedDataAccess dataAccess, ISqlStringService sqlStringService) 
            : base(dataAccess, sqlStringService)
        {
            typedDataAccess = dataAccess;
        }

        /// <summary>
        /// Returns all milestones on the system
        /// </summary>
        /// <returns>All milestones on the system</returns>
        [SqlResource(@"Milestone\GetAll")]
        public IEnumerable<Model.Milestone> GetAll()
        {
            string sql = LoadSqlResources().Values.First();
            return typedDataAccess.ExecuteQuery<Model.Milestone>(sql);
        }
        
        /// <summary>
        /// Returns the Milestone with the matching ID
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>The milestone with the given ID</returns>
        [SqlResource(@"Milestone\Get")]
        public Model.Milestone Get(int id)
        {
            string sql = LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters(new { Id = id });
            return typedDataAccess.ExecuteQuery<Model.Milestone>(sql, parameters).FirstOrDefault();
        }

        /// <summary>
        /// Returns the milestones assigned to
        /// the goal with the matching ID
        /// </summary>
        /// <param name="goalId">The ID of the Goal</param>
        /// <returns>The collection of milestones for the goal</returns>
        [SqlResource(@"Milestone\GetForGoal")]
        public IEnumerable<Model.Milestone> GetForGoal(int goalId)
        {
            string sql = LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters(new { GoalId = goalId });
            return typedDataAccess.ExecuteQuery<Model.Milestone>(sql, parameters);
        }

        /// <summary>
        /// Creates the milestone on the system
        /// </summary>
        /// <param name="milestone">The milestone to be created for the goal</param>
        /// <param name="goalId">The Goal ID for which we are creating the milestone</param>
        /// <returns>The goal that was created</returns>
        [SqlResource(@"Milestone\CreateForGoal")]
        public Model.Milestone CreateForGoal(Model.Milestone milestone, int goalId)
        {
            string sql = LoadSqlResources().Values.First();
            Dictionary<string, object> parameters = ConvertToParameters
            (
                new
                {
                    milestone.Target,
                    milestone.Description,
                    milestone.DateMet,
                    GoalId = goalId
                }
            );

            return typedDataAccess.ExecuteQuery<Model.Milestone>(sql, parameters).FirstOrDefault();
        }

        /// <summary>
        /// Creates a number of milestones for the goal
        /// </summary>
        /// <param name="milestones">The milestone(s) to be created</param>
        /// <param name="goalId">The ID for the goal</param>
        /// <returns>The milestones that were created</returns>
        [SqlResource(@"Milestone\CreateMultipleForGoal")]
        [SqlResource(@"Milestone\GetForGoal")]
        public IEnumerable<Model.Milestone> CreateMultipleForGoal(IEnumerable<Model.Milestone> milestones, int goalId)
        {
            Dictionary<string, string> statements = LoadSqlResources();

            string insertSQL = statements[@"Milestone\CreateMultipleForGoal"];
            var insertParameters = from milestone in milestones select new { milestone.Target, milestone.Description, milestone.DateMet, GoalId = goalId };
            
            //Ensure we have inserted all required
            int affected = typedDataAccess.ExecuteWithGenericParameters(insertSQL, insertParameters);
            if (affected != milestones.Count())
                throw new Exception("Could not create all required milestones on the system for goal");

            //Return all the milestones
            var selectParameters = new { GoalId = goalId };
            string selectSQL = statements[@"Milestone\GetForGoal"];
            return typedDataAccess.ExecuteQueryWithGenericParameterType<Model.Milestone>(selectSQL, selectParameters);
        }

        /// <summary>
        /// Updates the Milestone on the system with the given ID
        /// using the details of the milestone provided
        /// </summary>
        /// <param name="id">The ID of the milestone</param>
        /// <param name="milestone">The milestone containing new information</param>
        /// <returns>The milestone that was saved on the system</returns>
        [SqlResource(@"Milestone\Update")]
        public Model.Milestone Update(int id, Model.Milestone milestone)
        {
            string sql = LoadSqlResources().Values.First();
            milestone.Id = id;

            //Return directly from the update
            return typedDataAccess.ExecuteQueryWithGenericParameterType<Model.Milestone>(sql, milestone).FirstOrDefault();
        }

        /// <summary>
        /// Deletes the Milestone from the system with the
        /// given ID and returns the record that was removed
        /// </summary>
        /// <param name="id">The ID of the Milestone to be deleted</param>
        /// <returns>The ID of the record to delete</returns>
        [SqlResource(@"Milestone\DeleteWithSelect")]
        public Model.Milestone Delete(int id)
        {
            string sql = LoadSqlResources().Values.First();
            var parameters = new { Id = id };

            //Return directly from the delete
            return typedDataAccess.ExecuteQueryWithGenericParameterType<Model.Milestone>(sql, parameters).FirstOrDefault();
        }
    }
}