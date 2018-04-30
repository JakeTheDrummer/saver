﻿using Saver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Saver.Server.Controllers
{
    /// <summary>
    /// Provides the controller endpoints for all Goals
    /// </summary>
    [RoutePrefix("api")]
    [Route("users/{userId:int:min(1)}/goals")]
    //[Authorize]
    public class GoalsController : ApiController
    {
        // GET api/users/{userID}/goals
        /// <summary>
        /// Returns the collection of goals for the user
        /// with the given ID
        /// </summary>
        /// <param name="userId">The ID of the user for which we are returning goals</param>
        /// <returns>A collection of goals for the user</returns>
        public IEnumerable<Goal> Get(int userId)
        {
            //Return the goals
            return new Goal[] 
            {
                new Goal(1, "Test Goal A", "A", 150, GoalStatus.Open, false),
                new Goal(2, "Test Goal B", "B", 500, GoalStatus.Complete, false),
            };
        }

        // GET api/users/{userID}/goals/{id}
        /// <summary>
        /// Returns a single goal for the user
        /// with the given ID
        /// </summary>
        /// <param name="userID">The ID of the User</param>
        /// <param name="id">The ID of the goal</param>
        /// <returns>The goal with the matching User ID and Goal ID</returns>
        [HttpGet]
        [Route("{id:int:min(1)}")]
        public Goal Get(int userID, int id)
        {
            return new Goal(1, "Test Goal A", "A", 150, GoalStatus.Open, false);
        }

        // POST api/users/{userID}/goals/{id}
        /// <summary>
        /// Allows the user to create a goal for their account
        /// </summary>
        /// <param name="userId">The ID of the user for the goal</param>
        /// <param name="goal">The goal the user wishes to create</param>
        [HttpPost]
        public Goal Post(int userId, [FromBody]Goal goal)
        {
            throw new NotImplementedException();
        }

        // PUT api/users/1/goals/5
        /// <summary>
        /// Allows the user to update the
        /// </summary>
        /// <param name="userId">The ID of the user for the goal</param>
        /// <param name="id">The ID of the goal we wish to update</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int:min(1)}")]
        public Goal Put(int userId, int id, [FromBody]Goal value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/users/1/goals/5
        /// <summary>
        /// Allows a user to delete the goals and milestones
        /// with the given user ID and Goal ID. This should
        /// deallocate any funds and create transactions as
        /// required
        /// </summary>
        /// <param name="userId">The ID of the User of the goal</param>
        /// <param name="id">The ID of the goal we wish to delete</param>
        [Route("{id:int:min(1)}")]
        public void Delete(int userId, int id)
        {
            throw new NotImplementedException();
        }
    }
}
