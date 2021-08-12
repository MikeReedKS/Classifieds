using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.DataAccess;
using Classifieds.Models;
using Microsoft.Ajax.Utilities;

namespace Classifieds.Services
{
    /// <summary>
    /// Simple services to handle the session object. Could use ISessionManager, but this is easier and better fit for this demo
    ///
    /// Note that while we could easily have handled the work here, there is no external data source,
    /// instead we followed the pattern to stay consistent and to support changing the session persistence to another form than a static collection.
    /// Having the session list in memory, like many of the design choices, is based on the small scale design here. We'd do a lot of things differently
    /// if this was a project meant to scale. I cut corners for clarity and for time savings.
    /// </summary>
    public static class SessionService
    {

        /// <summary>
        /// Create a new session for the user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns>Session Id</returns>
        public static string PostSession(string userName, string userPassword)
        {
            //Get the userId for the user
            var user = UserDataAccess.ReadUser(userName, userPassword);

            //Exit if no user was found
            if (user.UserEmail.IsNullOrWhiteSpace()) return null;

            var sessionId = SessionDataAccess.CreateUserSession(user);

            //Some will debate that I should combine the call and the return, but I typically do not, so that I can more easily place a breakpoint and add code if needed.
            return sessionId;
        }

        /// <summary>
        /// Deletes a session based on user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public static void DeleteSession(string userName, string userPassword)
        {
            //Get the userId for the user
            var user = UserDataAccess.ReadUser(userName, userPassword);

            //Exit if no user was found
            if (user.UserEmail.IsNullOrWhiteSpace()) return;

            SessionDataAccess.DeleteSession(user);
        }

        /// <summary>
        /// Checks the session to assure it is valid
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <exception cref="HttpResponseException"></exception>
        public static void SessionCheck(string sessionId)
        {
            //Check for a valid session
            var sessionIsValid = SessionDataAccess.SessionCheck(sessionId);

            //If we are in a valid session, then return
            if (sessionIsValid) return;

            //If session is not valid, throw an Unauthorized exception back to client
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            throw new HttpResponseException(response);
        }
    }
}