using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Classifieds.DataAccess;
using Classifieds.Models;
using Microsoft.Ajax.Utilities;

namespace Classifieds.Services
{
    /// <summary>
    /// Service to handle the Listing object
    /// </summary>
    public static class ListingService
    {
        /// <summary>
        /// Create a new listing
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="newListing"></param>
        /// <returns></returns>
        public static string PostListing(string sessionId, ListingNew newListing)
        {
            var sessionUser = SessionDataAccess.FindSessionUser(sessionId);

            //If session is not valid, throw an Unauthorized exception back to client
            if (sessionUser.UserEmail.IsNullOrWhiteSpace())
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                throw new HttpResponseException(response);
            }

            //Create the new listing
            var listingId = ListingDataAccess.CreateListing(sessionUser, newListing);
            return listingId;
        }

        /// <summary>
        /// Gets the full list of listings without any filtering or sorting
        /// </summary>
        /// <returns></returns>
        public static string GetAllListings()
        {
            //Get all listings
            var listings = ListingDataAccess.ReadAllListings();

            //Return all the listings
            return listings;
        }

        /// <summary>
        /// Get any listings that match the supplied listing id. (Should always be one or zero entries)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Listing</returns>
        public static string GetListing(string id)
        {
            var filteredListings = ListingDataAccess.FilterListingsById(id);

            //Return all the listings
            return filteredListings;
        }

        /// <summary>
        /// Updates the listing specified by the id
        /// </summary>
        /// <param name="listingId">The Listing Id</param>
        /// <param name="sessionId">The Session Id</param>
        /// <param name="listing">The updated listing values - populate all fields</param>
        /// <returns><c>true</c> if the listing was updated, <c>false</c> if the update failed.</returns>
        public static bool PutListing(string listingId, string sessionId, ListingNew listing)
        {
            var sessionUser = SessionDataAccess.FindSessionUser(sessionId);

            var result = ListingDataAccess.UpdateListing(listingId, listing, sessionUser);

            return result;
        }

        /// <summary>
        /// Deletes the listing matching the Listing Id (if it exists)
        /// </summary>
        /// <param name="listingId"></param>
        /// <returns><c>true</c> if the listing was deleted, <c>false</c> if the delete failed.</returns>
        public static bool DeleteListing(string listingId, string sessionId)
        {
            var sessionUser = SessionDataAccess.FindSessionUser(sessionId);

            var result = ListingDataAccess.DeleteListing(listingId, sessionUser);

            return result;
        }
    }
}