using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Model
{
    /// <summary>
    /// Represents a milestone for a goal.
    /// This will be a total amount of money for a goal
    /// to be reached. This can only be reached once
    /// </summary>
    public class Milestone
    {
        /// <summary>
        /// To be used by Dapper ONLY
        /// </summary>
        public Milestone()
            : this(-1, 0, null, null)
        {
        }

        /// <summary>
        /// Creates a new Milestone
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <param name="target">The target of the milestone in currency amount</param>
        /// <param name="description">The description of the milestone</param>
        /// <param name="dateMet">The date the milestone was met</param>
        public Milestone(int id, double target, string description, DateTime? dateMet)
        {
            Id = id;
            Target = target;
            Description = description;
            DateMet = dateMet;
        }

        /// <summary>
        /// Gets or Sets the ID of the Milestone
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets the target amount
        /// </summary>
        public double Target { get; set; }

        /// <summary>
        /// Gets or Sets the Description of the Milestone
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the date on which this milestone was met
        /// </summary>
        public DateTime? DateMet { get; set; }
    }
}
