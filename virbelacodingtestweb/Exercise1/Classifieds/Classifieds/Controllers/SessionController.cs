using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Helpers;
using Classifieds.Services;
using Microsoft.Ajax.Utilities;

namespace Classifieds.Controllers
{
    /// <summary>
    /// Controller for the session object
    /// </summary>
    public class SessionController : ApiController
    {
        /// <summary>
        /// Creates a new Session
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public string Post(string userName, [FromBody] string userPassword)
        {
            SecurityHelper.ApiKeyCheck(Request);

            //Create a new session for the named user
            var newSessionId = SessionService.PostSession(userName, userPassword);
            if (newSessionId.IsNullOrWhiteSpace())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            return newSessionId;
        }

        /// <summary>
        /// Deletes an existing user's session. Must know user's user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public void Delete(string userName, [FromBody] string userPassword)
        {
            SecurityHelper.ApiKeyCheck(Request);

            SessionService.DeleteSession(userName, userPassword);
        }
    }
}