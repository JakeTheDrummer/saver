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
    /// Provides implementations for the milestone service interface
    /// allowing us to interact and control milestones on the system
    /// </summary>
    public class MilestoneService : ServiceBase, IMilestoneService
    {
        private IMilestoneRepository milestoneRepository;

        /// <summary>
        /// Creates a new Milestone service that uses the Milestone Repository
        /// to access information from the repository layer
        /// </summary>
        /// <param name="milestoneRepository">The repository to use for interacting with milestones</param>
        public MilestoneService(IMilestoneRepository milestoneRepository)
        {
            this.milestoneRepository = milestoneRepository;
        }

        /// <summary>
        /// Returns all milestones on the system
        /// </summary>
        /// <returns>All milestones on the system</returns>
        public IEnumerable<Milestone> GetAllMilestones()
        {
            //Return the ordered collection
            return base.ExecuteThenOrderBy(milestoneRepository.GetAll, m => m.Id);
        }

        /// <summary>
        /// Returns the milestones assigned to
        /// the goal with the matching ID
        /// </summary>
        /// <param name="goalId">The ID of the Goal</param>
        /// <returns>The collection of milestones for the goal</returns>
        public IEnumerable<Milestone> GetMilestonesForGoal(int goalId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Milestone with the matching ID
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>The milestone with the given ID</returns>
        public Milestone GetMilestone(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates the milestones on the system for the given
        /// goal (if none exist), equally spaced to the target
        /// </summary>
        /// <param name="goal">The Goal for which we are creating the milestones</param>
        /// <returns>The milestones that were created</returns>
        public IEnumerable<Milestone> GenerateMilestones(Goal goal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the milestone on the system
        /// </summary>
        /// <param name="milestone">The milestone to be created for the goal</param>
        /// <param name="goal">The Goal for which we are creating the milestone</param>
        /// <returns>The goal that was created</returns>
        public Milestone CreateMilestoneForGoal(Milestone milestone, Goal goal)
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
        public Milestone UpdateMilestone(int id, Milestone milestone)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the Milestone from the system with the
        /// given ID and returns the record that was removed
        /// </summary>
        /// <param name="id">The ID of the Milestone to be deleted</param>
        /// <returns>The ID of the record to delete</returns>
        public Milestone DeleteMilestone(int id)
        {
            throw new NotImplementedException();
        }
    }
}
