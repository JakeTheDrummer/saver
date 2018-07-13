using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Repositories.Interfaces
{
    /// <summary>
    /// Goal repositories should implement this interface
    /// </summary>
    public interface IGoalRepository
    {
        /// <summary>
        /// Returns the goal on the system with the
        /// given goal ID
        /// </summary>
        /// <param name="goalID">The ID of the Goal</param>
        /// <returns>The Goal with the given ID</returns>
        Goal GetGoal(int goalID);

        /// <summary>
        /// Returns all the goals on the system
        /// </summary>
        /// <returns>The goals on the system</returns>
        IEnumerable<Goal> GetGoals();

        /// <summary>
        /// Returns all the goals for the user
        /// </summary>
        /// <param name="userID">The ID of the User</param>
        /// <returns>The goals of the user</returns>
        IEnumerable<Goal> GetGoalsForUser(int userID);

        /// <summary>
        /// Returns the goal for the user with the given goal ID
        /// </summary>
        /// <param name="goalId">The ID of the Goal</param>
        /// <param name="userId">The ID of the User</param>
        /// <returns>The goal of the user</returns
        Goal GetGoalForUser(int goalId, int userId);

        /// <summary>
        /// Attempts to delete the Goal on the system with the given ID
        /// </summary>
        /// <param name="id">The ID of the goal to delete on the system</param>
        /// <returns>The record we have deleted from the system</returns>
        Goal Delete(int id);

        /// <summary>
        /// Creates the Goal on the system for the user
        /// with the given ID and using the information
        /// in the goal. The ID of the goal will be ignored
        /// </summary>
        /// <param name="value">The ID of the user to be assigned</param>
        /// <param name="goal">The goal that is being created</param>
        /// <returns>The goal that was created and persisted</returns>
        Goal CreateGoalForUser(int value, Goal goal);

        /// <summary>
        /// Updates the Goal on the system with the matching ID
        /// using the details within the Goal
        /// </summary>
        /// <param name="goalId">The ID of the Goal to update</param>
        /// <param name="goal">The goal containing the new information</param>
        /// <returns>The goal that was saved on the system</returns>
        Goal UpdateGoal(int goalId, Goal goal);
    }
}
