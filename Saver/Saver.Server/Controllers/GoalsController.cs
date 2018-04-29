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
    [Authorize]
    public class GoalsController : ApiController
    {
        // GET api/user/{userID}/goals
        /// <summary>
        /// Returns the collection of goals for the user
        /// with the given ID
        /// </summary>
        /// <param name="userID">The ID of the user for which we are returning goals</param>
        /// <returns>A collection of goals for the user</returns>
        public IEnumerable<string> Get(int userID)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/user/{userID}/goals/{id}
        public string Get(int userID, int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
