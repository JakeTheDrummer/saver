using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Interfaces
{
    /// <summary>
    /// Provides all methods that a Goal Service should implement
    /// </summary>
    public interface IGoalService
    {
        /// <summary>
        /// Returns all goals on the system
        /// </summary>
        /// <returns>The goals on the system</returns>
        IEnumerable<Goal> GetGoals();

        /// <summary>
        /// Returns the Goal with the given ID from the system
        /// </summary>
        /// <param name="id">The ID of the Goal</param>
        /// <returns>The goal on the system with the ID</returns>
        Goal GetGoal(int id);

        /// <summary>
        /// Attempts to save the goal on the system
        /// and returns the value stored in the database
        /// </summary>
        /// <param name="id">The ID (if existing)</param>
        /// <param name="goal">The goal to be persisted</param>
        /// <returns>The goal that was saved on the system</returns>
        Goal SaveGoal(int? id, Goal goal);

        /// <summary>
        /// Deletes the goal on the system with the given
        /// ID. This will delete milestones associated with
        /// the goal also.
        /// </summary>
        /// <param name="id">The ID of the Goal to delete</param>
        /// <returns>Whether we have deleted the goal</returns>
        bool DeleteGoal(int id);
    }
}
