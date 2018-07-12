using Saver.Model;
using Saver.Repositories.Interfaces;
using Saver.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Implementations
{
    /// <summary>
    /// Implements the IGoalService to provide
    /// interactivity with the database and provides
    /// simple logic to perform actions
    /// </summary>
    public class GoalService : ServiceBase, IGoalService
    {
        IGoalRepository goalRepository = null;

        /// <summary>
        /// Creates a new Goal Repository allowing us to
        /// interact with the system and control business logic flow
        /// </summary>
        /// <param name="goalRepository">The goal repository we will use</param>
        public GoalService(IGoalRepository goalRepository)
        {
            this.goalRepository = goalRepository ?? throw new ArgumentNullException(nameof(goalRepository), "Please provide a goal repository");
        }

        /// <summary>
        /// Deletes the goal on the system with the given
        /// ID. This will delete milestones associated with
        /// the goal also.
        /// </summary>
        /// <param name="id">The ID of the Goal to delete</param>
        /// <returns>Whether we have deleted the goal</returns>
        public bool DeleteGoal(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Goal with the given ID from the system
        /// </summary>
        /// <param name="id">The ID of the Goal</param>
        /// <returns>The goal on the system with the ID</returns>
        public Goal GetGoal(int id)
        {
            return goalRepository.GetGoal(id);
        }

        /// <summary>
        /// Returns all goals on the system
        /// </summary>
        /// <returns>The goals on the system</returns>
        public IEnumerable<Goal> GetGoals()
        {
            return ExecuteThenOrderBy(goalRepository.GetGoals, g => g.Id);
        }

        /// <summary>
        /// Returns all goals on the system for the given user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <returns>The goals on the system the user</returns>
        public IEnumerable<Goal> GetGoalsForUser(int userID)
        {
            return ExecuteThenOrderBy(() => goalRepository.GetGoalsForUser(userID), g => g.Id);
        }

        /// <summary>
        /// Attempts to save the goal on the system
        /// and returns the value stored in the database
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <param name="goal">The goal to be persisted</param>
        /// <returns>The goal that was saved on the system</returns>
        public Goal CreateGoal(int? userId, Goal goal)
        {
            //Ensure we have valid data
            if (!userId.HasValue)
                throw new ArgumentOutOfRangeException(nameof(userId), "Please ensure the user is given when creating the goal");
            if (goal.Id > 0)
                throw new ArgumentOutOfRangeException(nameof(goal.Id), "Please ensure the goal has no identifying information when creating");

            //Create the goal
            Goal savedGoal = goalRepository.CreateGoalForUser(userId.Value, goal);
            return savedGoal;
        }

        public Goal UpdateGoal(Goal goal)
        {
            throw new NotImplementedException();
        }
    }
}
