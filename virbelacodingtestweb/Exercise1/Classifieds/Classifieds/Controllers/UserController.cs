using Classifieds.Models;
using Classifieds.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Helpers;

namespace Classifieds.Controllers
{
    /// <summary>
    /// Controller for the User object
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        ///     Creates a new user.
        /// </summary>
        /// <param name="userName">The new user name.</param>
        /// <param name="userEmail">The email address of the new user.</param>
        /// <param name="userPassword">The password for the new user, passed in body.</param>
        /// <returns>GUID - The new users Id.</returns>
        /// <response code="200">OK - New user was created.</response>
        /// <response code="401">Forbidden - Authorization has been denied for this request. API Key or Session Id failure can cause this response.</response>
        /// <response code="409">Conflict - User already exists.</response>
        /// <response code="5xx">Exception - The message may contain additional exception information.</response>
        /// <remarks>
        ///     Creates a new user.
        /// </remarks>
        [HttpPost]
        public Guid Post(string userName, string userEmail, [FromBody] string userPassword)
        {
            SecurityHelper.ApiKeyCheck(Request);

            //Call the service layer to create the user
            var userId = UserService.PostUser(userName, userEmail, userPassword);

            //If we got a non-error Id, then return it, else return the matching error code
            switch (userId.ToString())
            {
                case "00000000-0000-0000-0000-000000000000":
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                case "00000000-0000-0000-0000-000000000001":
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
                default:
                    return userId;
            }
        }

        /// <summary>
        /// Gets an existing user based on UserId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionId"></param>
        /// <returns>User object</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        public User Get(Guid id, string sessionId)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            //Call to get the requested user
            var user = UserService.GetUser(id);

            //Hide the password so that it is not passed back
            user.UserPassword = "*********";

            //If the requested user does not exist, then return a not found exception with a 1 second delay to fight brute force attacks
            //Using GUID instead of int for the Id makes a huge difference to stop sequential database dumps but it is so critical to stop that we always go further
            if (user.UserId == null)
            {
                System.Threading.Thread.Sleep(1000);
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
            return user;
        }

        /// <summary>
        /// Gets an existing user based on email address
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        /// <remarks>
        ///     This is a non-standard RESTful implementation that I show as an example. REST, like all patterns and practices are guides and not
        ///     hard and fast rules. The engineer must define what is needed and bend the rules as needed to create great systems. As long as the
        ///     rules are known and breaking the rules is a conscious choice, then it is acceptable.
        /// </remarks>
        [HttpGet]
        public User Get([FromUri] string userEmail, string sessionId)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            //Call to get the requested user
            var user = UserService.GetUser(userEmail);

            //Hide the password so that it is not passed back
            user.UserPassword = "*********";

            //If the requested user does not exist, then return a not found exception with a 1 second delay to fight brute force attacks
            //Using GUID instead of int for the Id makes a huge difference to stop sequential database dumps but it is so critical to stop that we always go further
            if (user.UserId == null)
            {
                System.Threading.Thread.Sleep(1000);
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundResponse);
            }
            return user;
        }

        ///// <summary>
        ///// Updates the email address and/or password of an existing user (Not Implemented)
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="userName"></param>
        ///// <param name="userEmail"></param>
        ///// <param name="userPassword"></param>
        ///// <response code="200">OK - New user was created.</response>
        ///// <response code="401">Forbidden - Authorization has been denied for this request. API Key or Session Id failure can cause this response.</response>
        ///// <response code="501">Not Implemented.</response>
        ///// <response code="5xx">Exception - The message may contain additional exception information.</response>
        ///// <remarks>
        /////     User Id and current Password must be known to update the user.
        ///// </remarks>
        //[HttpPut]
        //public void Put(Guid id, string userName, string userEmail, [FromBody] string userPassword)
        //{
        //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));
        //}

        ///// <summary>
        /////     Deletes an existing user. (Not Implemented)
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <param name="sessionId">The session Id. (Always 1234567 in this example)</param>
        ///// <param name="userPassword">The password for the new user, passed in body.</param>
        ///// <returns></returns>
        ///// <response code="200">OK - New user was created.</response>
        ///// <response code="401">Forbidden - Authorization has been denied for this request. API Key or Session Id failure can cause this response.</response>
        ///// <response code="501">Not Implemented.</response>
        ///// <response code="5xx">Exception - The message may contain additional exception information.</response>
        ///// <remarks>
        /////     User Id and Password must be known to delete the user.
        ///// </remarks>
        //[HttpDelete]
        //public void Delete(Guid userid, string sessionId, [FromBody] string userPassword)
        //{
        //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));
        //}
    }
}