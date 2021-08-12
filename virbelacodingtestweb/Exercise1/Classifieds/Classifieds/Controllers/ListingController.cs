using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Classifieds.Helpers;
using Classifieds.Models;
using Classifieds.Services;

namespace Classifieds.Controllers
{
    /// <summary>
    /// This is the controller for the Listing object
    /// </summary>
    public class ListingController : ApiController
    {
        /// <summary>
        /// Creates a new listing
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="listing"></param>
        public string Post(string sessionId, [FromBody] ListingNew listing)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            //Call the service layer to create the user
            var userId = ListingService.PostListing(sessionId, listing);

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
        /// Gets all listings
        /// </summary>
        /// <returns></returns>
        public string Get(string sessionId)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            var listings = ListingService.GetAllListings();

            return listings;
        }

        /// <summary>
        /// Get a specific listing by Listing Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public string Get(string id, string sessionId)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            var listings = ListingService.GetListing(id);

            return listings;
        }

        /// <summary>
        /// Update an existing listing
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="sessionId">Session Id</param>
        /// <param name="listing">Updated listing info. Fill in all fields.</param>
        public void Put(string id, string sessionId, [FromBody] ListingNew listing)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            var result = ListingService.PutListing(id, sessionId, listing);
            if (result != false) return;

            //if there was an error, throw a 404 Not Found
            var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            throw new HttpResponseException(notFoundResponse);
        }

        /// <summary>
        /// Delete an existing listing
        /// </summary>
        /// <param name="id">Listing Id</param>
        /// <param name="sessionId">Session Id</param>
        public void Delete(string id, string sessionId)
        {
            SecurityHelper.ApiKeyCheck(Request);
            SessionService.SessionCheck(sessionId);

            bool result = ListingService.DeleteListing(id, sessionId);

            if (result != false) return;

            //if there was an error, throw a 404 Not Found
            var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            throw new HttpResponseException(notFoundResponse);

        }
    }
}