using Saver.Model;
using Saver.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Saver.Server.Controllers
{
    /// <summary>
    /// Provides the entry point for (most) of the milestones
    /// interactivity exposed through the web server via REST
    /// </summary>
    [RoutePrefix("api")]
    [Route("users/{userId:int:min(1)}/goals/{goalId:int:min(1)}/milestones")]
    public class MilestonesController : ApiController
    {
        private IGoalService goalService;
        private IMilestoneService milestoneService;

        /// <summary>
        /// Creates a new Milestone Service that allows interactivity
        /// with milestones directly through the web service
        /// </summary>
        /// <param name="goalService">The goal service through which we will interact with goals</param>
        /// <param name="milestoneService">The service we will use to interact with milestone logic</param>
        public MilestonesController(IGoalService goalService, IMilestoneService milestoneService)
        {
            this.goalService = goalService;
            this.milestoneService = milestoneService;
        }

        /// <summary>
        /// Returns all milestones on the system
        /// </summary>
        /// <returns>All milestones on the system</returns>
        /// <remarks>Remove this before we go live!</remarks>
        [HttpGet]
        [Route("milestones", Name = "GetAllMilestones")]
        public IEnumerable<Milestone> Get()
        {
            return milestoneService.GetAllMilestones();
        }

        /// <summary>
        /// Returns the milestone on the system with the given Id
        /// </summary>
        /// <param name="id">The ID of the Milestone</param>
        /// <returns>All milestone from the system</returns>
        [HttpGet]
        [Route("milestones/{id:int:min(1)}", Name = "GetMilestone")]
        public Milestone Get(int id)
        {
            return milestoneService.GetMilestone(id);
        }

        /// <summary>
        /// Returns all milestones on the system for the goal
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="goalId">The goal ID of the goal</param>
        /// <returns>All milestones created for that goal</returns>
        [HttpGet]
        public IEnumerable<Milestone> Get(int userId, int goalId)
        {
            return milestoneService.GetMilestonesForGoal(goalId);
        }
        
        /// <summary>
        /// Generates (if possible) the milestones for the goal
        /// with the given ID
        /// </summary>
        /// <param name="userId">The user ID of the user</param>
        /// <param name="goalId">The ID of the goal</param>
        /// <returns>The milestones that were generated</returns>
        [HttpPatch]
        public IEnumerable<Milestone> Patch(int userId, int goalId)
        {
            //Generate the milestones for the goal
            Goal goal = goalService.GetGoal(goalId);
            return milestoneService.GenerateMilestones(goal);
        }

        /// <summary>
        /// Attempts to create the milestone on the system with the given 
        /// details for the goal ID provided
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="goalId">The ID of the goal</param>
        /// <param name="milestone">The milestone details</param>
        /// <returns>The milestone on the system that was created</returns>
        [HttpPost]
        public Milestone Post(int userId, int goalId, [FromBody]Milestone milestone)
        {
            //Generate the milestones for the goal
            Goal goal = goalService.GetGoal(goalId);
            return milestoneService.CreateMilestoneForGoal(milestone, goal);
        }

        /// <summary>
        /// Attempts to update the milestone on the system with the
        /// given ID and returns the now persisted value
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="goalId">The ID of the goal</param>
        /// <param name="milestoneId">The ID of the milestone</param>
        /// <param name="milestone">The milestone details</param>
        /// <returns>The milestone that was created</returns>
        [HttpPut]
        [Route("users/{userId:int:min(1)}/goals/{goalId:int:min(1)}/milestones/{milestoneId:int:min(1)}")]
        public Milestone Put(int userId, int goalId, int milestoneId, [FromBody]Milestone milestone)
        {
            return milestoneService.UpdateMilestone(milestoneId, milestone);
        }
        
        /// <summary>
        /// Attempts to delete the record on the system with the given milestone Id
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="goalId">The ID of the goal</param>
        /// <param name="milestoneId">The ID of the milestone to delete</param>
        /// <returns>The milestone that was deleted</returns>
        [HttpDelete]
        [Route("users/{userId:int:min(1)}/goals/{goalId:int:min(1)}/milestones/{milestoneId:int:min(1)}")]
        public Milestone Delete(int userId, int goalId, int milestoneId)
        {
            return milestoneService.DeleteMilestone(milestoneId);
        }
    }
}
