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
        /// Returns the milestones assigned to
        /// the goal with the matching ID
        /// </summary>
        /// <param name="goalId">The ID of the Goal</param>
        /// <returns>The collection of milestones for the goal</returns>
        public IEnumerable<Model.Milestone> GetForGoal(int goalId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Returns the Milestone with the matching ID
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>The milestone with the given ID</returns>
        public Model.Milestone Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the milestone on the system
        /// </summary>
        /// <param name="milestone">The milestone to be created for the goal</param>
        /// <param name="goalId">The Goal ID for which we are creating the milestone</param>
        /// <returns>The goal that was created</returns>
        public Model.Milestone CreateForGoal(Model.Milestone milestone, int goalId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a number of milestones for the goal
        /// </summary>
        /// <param name="milestone">The milestone(s) to be created</param>
        /// <param name="goalId">The ID for the goal</param>
        /// <returns></returns>
        public IEnumerable<Model.Milestone> CreateMultipleForGoal(IEnumerable<Model.Milestone> milestone, int goalId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Updates the Milestone on the system with the given ID
        /// using the details of the milestone provided
        /// </summary>
        /// <param name="id">The ID of the milestone</param>
        /// <param name="milestone">The milestone containing new information</param>
        /// <returns>The milestone that was saved on the system</returns>
        public Model.Milestone Update(int id, Model.Milestone milestone)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the Milestone from the system with the
        /// given ID and returns the record that was removed
        /// </summary>
        /// <param name="id">The ID of the Milestone to be deleted</param>
        /// <returns>The ID of the record to delete</returns>
        public Model.Milestone Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
