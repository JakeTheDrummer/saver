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
        /// Returns all the goals for the user
        /// </summary>
        /// <param name="userID">The ID of the User</param>
        /// <returns>The goals of the user</returns>
        IEnumerable<Goal> GetGoalsForUser(int userID);
    }
}
