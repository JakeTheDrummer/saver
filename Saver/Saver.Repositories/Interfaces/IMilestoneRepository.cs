using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Interfaces
{
    /// <summary>
    /// Provides all database interactivity definition
    /// methods pertaining to milestones
    /// </summary>
    public interface IMilestoneRepository
    {
        /// <summary>
        /// Returns all milestones on the system
        /// </summary>
        /// <returns>All milestones on the system</returns>
        IEnumerable<Milestone> GetAll();

        /// <summary>
        /// Returns the milestones assigned to
        /// the goal with the matching ID
        /// </summary>
        /// <param name="goalId">The ID of the Goal</param>
        /// <returns>The collection of milestones for the goal</returns>
        IEnumerable<Milestone> GetForGoal(int goalId);

        /// <summary>
        /// Returns the Milestone with the matching ID
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>The milestone with the given ID</returns>
        Milestone Get(int id);

        /// <summary>
        /// Creates the milestone on the system
        /// </summary>
        /// <param name="milestone">The milestone to be created for the goal</param>
        /// <param name="goalId">The Goal ID for which we are creating the milestone</param>
        /// <returns>The goal that was created</returns>
        Milestone CreateForGoal(Milestone milestone, int goalId);

        /// <summary>
        /// Creates a number of milestones for the goal
        /// </summary>
        /// <param name="milestone">The milestone(s) to be created</param>
        /// <param name="goalId">The ID for the goal</param>
        /// <returns></returns>
        IEnumerable<Milestone> CreateMultipleForGoal(IEnumerable<Milestone> milestone, int goalId);

        /// <summary>
        /// Updates the Milestone on the system with the given ID
        /// using the details of the milestone provided
        /// </summary>
        /// <param name="id">The ID of the milestone</param>
        /// <param name="milestone">The milestone containing new information</param>
        /// <returns>The milestone that was saved on the system</returns>
        Milestone Update(int id, Milestone milestone);

        /// <summary>
        /// Deletes the Milestone from the system with the
        /// given ID and returns the record that was removed
        /// </summary>
        /// <param name="id">The ID of the Milestone to be deleted</param>
        /// <returns>The ID of the record to delete</returns>
        Milestone Delete(int id);
    }
}
