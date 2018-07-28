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
            return ExecuteThenOrderBy(() => milestoneRepository.GetForGoal(goalId), milestone => milestone.Id);
        }

        /// <summary>
        /// Returns the Milestone with the matching ID
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>The milestone with the given ID</returns>
        public Milestone GetMilestone(int id)
        {
            return milestoneRepository.Get(id);
        }

        /// <summary>
        /// Generates the milestones on the system for the given
        /// goal (if none exist), equally spaced to the target
        /// </summary>
        /// <param name="goal">The Goal for which we are creating the milestones</param>
        /// <returns>The milestones that were created</returns>
        public IEnumerable<Milestone> GenerateMilestones(Goal goal)
        {
            //If we have some we should stop
            IEnumerable<Milestone> currentMilestones = milestoneRepository.GetForGoal(goal.Id);
            if (currentMilestones.Any())
                throw new Exception("This goal has milestones already - cannot automatically create additional milestones");

            List<Milestone> milestones = new List<Milestone>(5);
            double targetSteps = goal.Target / 5;
            for (int i = 1; i <= 5; i++)
            {
                milestones.Add(new Milestone(targetSteps * i, $"{goal.Name} - {i}/5 completed!", null));
            }

            //Generate the milestones
            milestones = ExecuteThenOrderBy(() => milestoneRepository.CreateMultipleForGoal(milestones, goal.Id), m => m.Id).ToList();
            return milestones;
        }

        /// <summary>
        /// Creates the milestone on the system
        /// </summary>
        /// <param name="milestone">The milestone to be created for the goal</param>
        /// <param name="goal">The Goal for which we are creating the milestone</param>
        /// <returns>The goal that was created</returns>
        public Milestone CreateMilestoneForGoal(Milestone milestone, Goal goal)
        {
            //Check this is valid
            if (milestone.Target > goal.Target)
                throw new ArgumentException("The target for this milestone exceeds the target for the goal");
            if (milestone.Target <= 0)
                throw new ArgumentException("The target for this milestone cannot be zero or below");

            IEnumerable<Milestone> currentMilestones = milestoneRepository.GetForGoal(goal.Id);
            if (currentMilestones.Any(currentMilestone => currentMilestone.Target == milestone.Target))
                throw new ArgumentException("This goal already has a milestone for the given target amount");

            return milestoneRepository.CreateForGoal(milestone, goal.Id);
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
            //Run the appropriate validations
            ValidateMilestoneExistsAndIsNotCompleted(id);
            return milestoneRepository.Update(id, milestone);
        }

        private void ValidateMilestoneExistsAndIsNotCompleted(int id)
        {
            //Ensure we can update here
            Milestone currentMilestone = milestoneRepository.Get(id);
            if (currentMilestone == null)
                throw new ArgumentException("A milestone with this ID does not exist on the system");

            if (currentMilestone.DateMet.HasValue)
                throw new Exception("This milestone has been completed and cannot be updated");
        }

        /// <summary>
        /// Deletes the Milestone from the system with the
        /// given ID and returns the record that was removed
        /// </summary>
        /// <param name="id">The ID of the Milestone to be deleted</param>
        /// <returns>The ID of the record to delete</returns>
        public Milestone DeleteMilestone(int id)
        {
            //Run the appropriate validations
            ValidateMilestoneExistsAndIsNotCompleted(id);
            return milestoneRepository.Delete(id);
        }
    }
}