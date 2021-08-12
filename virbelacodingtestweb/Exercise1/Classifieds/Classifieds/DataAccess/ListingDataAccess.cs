using System;
using System.Collections.Generic;
using System.IO;
using Classifieds.Helpers;
using Classifieds.Models;
using Newtonsoft.Json;

namespace Classifieds.DataAccess
{
    internal static class ListingDataAccess
    {
        internal static string CreateListing(User sessionUser, ListingNew listing)
        {
            //To assure all timestamps are exactly the same, we get the time once and use it for all entries of the transaction
            var currentTimestamp = DateTime.UtcNow;

            //Create the new listing object
            var newListing = new Listing
            {
                ListingId = Guid.NewGuid(),
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                ListedDate = currentTimestamp,
                ListingUpdated = currentTimestamp,
                UserId = sessionUser.UserId
            };

            //Get existing listings, if there are any. 
            var dataFolder = StorageHelper.GetDataFolder();
            var listingList = new List<Listing>();
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "listingDB.json"));
                listingList = JsonConvert.DeserializeObject<List<Listing>>(rawFileJson);
            }
            catch
            {
                //Intentionally blank, just means there is no userDB.json file yet.
            }

            //Add the new listing to the collection
            listingList.Add(newListing);

            //Save the updated collection to disk
            var updatedFileJson = JsonConvert.SerializeObject(listingList);
            File.WriteAllText(Path.Combine(dataFolder, "ListingDB.json"), updatedFileJson);

            //Return the generated Id for the new user
            return newListing.ListingId.ToString();

        }

        public static string ReadAllListings()
        {
            //Get existing listings, if there are any. 
            var dataFolder = StorageHelper.GetDataFolder();
            var rawFileJson = "";
            try
            {
                rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "listingDB.json"));
            }
            catch
            {
                //Intentionally blank, just means there is no userDB.json file yet.
            }

            return rawFileJson;
        }

        public static string FilterListingsById(string listingId)
        {
            //Get existing listings, if there are any. 
            var dataFolder = StorageHelper.GetDataFolder();
            var listingList = new List<Listing>();
            try
            {
                //Read all listings
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "listingDB.json"));
                listingList = JsonConvert.DeserializeObject<List<Listing>>(rawFileJson);

                //Remove all those that do not match
                listingList.RemoveAll(x => x.ListingId.ToString() != listingId);
            }
            catch
            {
                //Intentionally blank, just means there is no userDB.json file yet.
            }

            //Convert back to Json
            var jsonListing = JsonConvert.SerializeObject(listingList);

            return jsonListing;
        }

        public static bool UpdateListing(string listingId, ListingNew newListing, User sessionUser)
        {
            var dataFolder = StorageHelper.GetDataFolder();
            List<Listing> listingList;
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "listingDB.json"));
                listingList = JsonConvert.DeserializeObject<List<Listing>>(rawFileJson);
            }
            catch
            {
                return false;
            }

            foreach (var listing in listingList)
            {
                if (listing.ListingId.ToString().Equals(listingId) && listing.UserId == sessionUser.UserId)
                {
                    //Update the listing
                    listing.Title = newListing.Title;
                    listing.Description = newListing.Description;
                    listing.Price = newListing.Price;
                    listing.ListingUpdated = DateTime.UtcNow;

                    //Save the collection with the updated user to disk
                    var updatedFileJson = JsonConvert.SerializeObject(listingList);
                    File.WriteAllText(Path.Combine(dataFolder, "listingDB.json"), updatedFileJson);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteListing(string listingId, User sessionUser)
        {
            //Get existing listings, if there are any. 
            var dataFolder = StorageHelper.GetDataFolder();
            try
            {
                //Read all listings
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "listingDB.json"));

                //Convert from Json to a list
                var listingList = JsonConvert.DeserializeObject<List<Listing>>(rawFileJson);

                //Remove all the matching listings (should only be one matching listing)
                listingList.RemoveAll(x => x.ListingId.ToString() != listingId && x.UserId == sessionUser.UserId);

                //Save the collection with the updated user to disk
                var updatedFileJson = JsonConvert.SerializeObject(listingList);
                File.WriteAllText(Path.Combine(dataFolder, "listingDB.json"), updatedFileJson);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}