using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Model
{
    /// <summary>
    /// Represents a goal that a user can save for
    /// </summary>
    public class Goal
    {
        /// <summary>
        /// Private constructor - required for Serialisation
        /// </summary>
        private Goal()
        {
        }

        /// <summary>
        /// Creates a new Goal
        /// </summary>
        /// <param name="id">The ID of the Goal</param>
        /// <param name="name">The name of the Goal</param>
        /// <param name="description">The description of the goal</param>
        /// <param name="target">The target of the goal</param>
        /// <param name="status">The status of the goal</param>
        /// <param name="isDefault">Whether the goal is the default for the user</param>
        public Goal(string name, string description, double target, GoalStatus status, bool isDefault)
            : this(0, name, description, target, status, isDefault)
        {
        }

        /// <summary>
        /// Creates a new Goal
        /// </summary>
        /// <param name="id">The ID of the Goal</param>
        /// <param name="name">The name of the Goal</param>
        /// <param name="description">The description of the goal</param>
        /// <param name="target">The target of the goal</param>
        /// <param name="status">The status of the goal</param>
        /// <param name="isDefault">Whether the goal is the default for the user</param>
        public Goal(int id, string name, string description, double target, GoalStatus status, bool isDefault)
        {
            Id = id;
            Name = name;
            Description = description;
            Target = target <= 0 ? throw new ArgumentOutOfRangeException(nameof(target), "Please ensure the goal has a target") : target;
            Status = status;
            IsDefault = isDefault;
        }

        /// <summary>
        /// Returns the ID of the Goal
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the Name of the Goal
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the goal
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Target of the goal
        /// </summary>
        public double Target { get; set; }

        /// <summary>
        /// Gets or sets the status of the goal
        /// </summary>
        public GoalStatus Status { get; set; }

        /// <summary>
        /// Returns whether this is the default user goal in the collection
        /// </summary>
        public bool IsDefault { get; private set; }
    }
}
