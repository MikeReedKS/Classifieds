using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Extensions;
using Microsoft.Ajax.Utilities;

namespace Classifieds.Helpers
{
    /// <summary>
    /// Routines to simplify security
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Super simple API Key check against a hard coded key
        /// </summary>
        /// <param name="request"></param>
        /// <returns><c>true</c> if API Key matches, <c>false</c> if it does not match.</returns>
        public static bool CheckApiKey(HttpRequestMessage request)
        {
            var headers = request.Headers;
            var apiKey = "";
            if (headers.Contains("APIKey"))
            {
                apiKey = headers.GetValues("APIKey").FirstOrDefault();
            }

            if (apiKey.IsNullOrWhiteSpace())
            {
                return false;
            }


            if (apiKey=="1234567")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks the API Key to assure it matches
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="HttpResponseException"></exception>
        public static void ApiKeyCheck(HttpRequestMessage request)
        {
            //Check API Key to make sure request is valid
            if (request.ValidateApiKey()) return;

            //If we didn't get a valid API Key, throw an Unauthorized exception back to client
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            throw new HttpResponseException(response);
        }
    }
}