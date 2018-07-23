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
        private IMilestoneService milestoneService;

        /// <summary>
        /// Creates a new Milestone Service that allows interactivity
        /// with milestones directly through the web service
        /// </summary>
        /// <param name="milestoneService">The service we will use to interact with milestone logic</param>
        public MilestonesController(IMilestoneService milestoneService)
        {
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
    }
}
