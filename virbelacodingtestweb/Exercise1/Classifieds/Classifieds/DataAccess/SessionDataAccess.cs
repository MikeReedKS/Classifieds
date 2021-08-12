using System;
using System.Collections.Generic;
using System.Linq;
using Classifieds.Models;

namespace Classifieds.DataAccess
{
    public class SessionDataAccess
    {
        private static readonly List<Session> Sessions = new List<Session>();

        /// <summary>
        /// Create a new user session, deleting any existing session for the user, if one exists.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string CreateUserSession(User user)
        {
            //Delete any existing session for this user
            Sessions.RemoveAll(x => x.UserId == user.UserId);

            //Create a new session object and store it in the Sessions list
            var newSession = new Session
            {
                UserId = user.UserId,
                SessionId = Guid.NewGuid().ToString(),
                SessionTimeout = DateTime.UtcNow.AddMinutes(60) //Timeout after an hour
            };
            Sessions.Add(newSession);

            return newSession.SessionId;
        }

        /// <summary>
        /// Deletes any sessions for the specified user
        /// </summary>
        /// <param name="user"></param>
        public static void DeleteSession(User user)
        {
            //Delete any existing session for this user
            Sessions.RemoveAll(x => x.UserId == user.UserId);
        }

        /// <summary>
        /// Checks to see if the session id is valid.
        /// It does not correlate it to the user to assure there is a match.
        /// Matching to the user could be added, but it was overkill for this demo.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static bool SessionCheck(string sessionId)
        {
            var isSessionValid = Sessions.Any(session => session.SessionId == sessionId && session.SessionTimeout > DateTime.UtcNow);
            
            return isSessionValid;
        }

        /// <summary>
        /// Finds the user associated with a session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static User FindSessionUser(string sessionId)
        {
            //Find the user associated with the session
            var userSession = Sessions.Find(session => session.SessionId == sessionId && session.SessionTimeout > DateTime.UtcNow);

            //Get the user object for the session
            var user = UserDataAccess.ReadUser(userSession.UserId);

            return user;
        }
    }
}