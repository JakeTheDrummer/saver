using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Interfaces
{
    /// <summary>
    /// Provides a collection of methods that should
    /// be available to all consumers wishing to interact
    /// with Milestones on the saver system
    /// </summary>
    public interface IMilestoneService
    {
        /// <summary>
        /// Returns all milestones on the system
        /// </summary>
        /// <returns>All milestones on the system</returns>
        IEnumerable<Milestone> GetAllMilestones();

        /// <summary>
        /// Returns the milestones assigned to
        /// the goal with the matching ID
        /// </summary>
        /// <param name="goalId">The ID of the Goal</param>
        /// <returns>The collection of milestones for the goal</returns>
        IEnumerable<Milestone> GetMilestonesForGoal(int goalId);

        /// <summary>
        /// Returns the Milestone with the matching ID
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>The milestone with the given ID</returns>
        Milestone GetMilestone(int id);

        /// <summary>
        /// Generates the milestones on the system for the given
        /// goal (if none exist), equally spaced to the target
        /// </summary>
        /// <param name="goal">The Goal for which we are creating the milestones</param>
        /// <returns>The milestones that were created</returns>
        IEnumerable<Milestone> GenerateMilestones(Goal goal);

        /// <summary>
        /// Creates the milestone on the system
        /// </summary>
        /// <param name="milestone">The milestone to be created for the goal</param>
        /// <param name="goal">The Goal for which we are creating the milestone</param>
        /// <returns>The goal that was created</returns>
        Milestone CreateMilestoneForGoal(Milestone milestone, Goal goal);

        /// <summary>
        /// Updates the Milestone on the system with the given ID
        /// using the details of the milestone provided
        /// </summary>
        /// <param name="id">The ID of the milestone</param>
        /// <param name="milestone">The milestone containing new information</param>
        /// <returns>The milestone that was saved on the system</returns>
        Milestone UpdateMilestone(int id, Milestone milestone);

        /// <summary>
        /// Deletes the Milestone from the system with the
        /// given ID and returns the record that was removed
        /// </summary>
        /// <param name="id">The ID of the Milestone to be deleted</param>
        /// <returns>The ID of the record to delete</returns>
        Milestone DeleteMilestone(int id);
    }
}
