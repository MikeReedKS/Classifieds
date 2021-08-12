using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Classifieds.Helpers;
using Classifieds.Models;
using Newtonsoft.Json;

namespace Classifieds.DataAccess
{
    /// <summary>
    /// This Data Access object is lame, it uses JSON to avoid the need to include a DB in the project. This will only support really small scale projects,
    /// The layers of Controller->Service->DataAccess is sound and should be followed, and you can replace the Data Access layer to use any data source desired without
    /// the need to rewrite any higher level code. Simplicity was the goal here, sacrificing scalability and security. Data is stored in the program folder in plain text
    /// within JSON files, so anyone with access to the program folder can easily hack the data. Simple two-way hashing is done to the passwords as a simple safeguard.
    /// I just so happened to already had a simple two-way string crypto library, so I used that. In a real DB I'd generate a seed and it would be a 1 way hash with a
    /// separate seed stored for each password stored. There must never be a way to retrieve the original password in a secure password store.
    ///
    /// This level remain public so that we can easily run unit tests, but often I mark this as internal so that the Service layer is the only testing point.
    /// The service layer truly is the public interface, and the controller just drives the UI. The Data Access layer should be protected as it does not include the business logic
    /// to manipulate the data as needed. We may expose private data if we expose this layer publicly. 
    /// </summary>
    public static class UserDataAccess
    {
        /// <summary>
        /// Creates the new user by persisting it within the userDB.json file.
        /// </summary>
        /// <param name="userName">Users User Name</param>
        /// <param name="userEmail">Users Email Address</param>
        /// <param name="userPassword">Users Password</param>
        /// <returns>The new user's Id in the form of a Guid.
        /// The password is encrypted prior to being stored.</returns>
        public static Guid CreateUser(string userName, string userEmail, string userPassword)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = userName,
                UserEmail = userEmail,
                UserPassword = CryptographyHelper.TextEncode(userPassword)
            };

            //Get existing users, if there are any. 
            var dataFolder = StorageHelper.GetDataFolder();
            var userList = new List<User>();
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "userDB.json"));
                userList = JsonConvert.DeserializeObject<List<User>>(rawFileJson);
            }
            catch
            {
                //Intentionally blank, just means there is no userDB.json file yet.
            }

            //Check to make sure user does not already exist
            if (userList.Any(user => user.UserEmail.Equals(newUser.UserEmail)))
            {
                //If the user already exists, return the GUID of 1
                var errorGuid = new Guid("00000000-0000-0000-0000-000000000001");
                return errorGuid;
            }

            //If we have a new user, add it to the collection
            userList.Add(newUser);

            //Save the updated collection to disk
            var updatedFileJson = JsonConvert.SerializeObject(userList);
            File.WriteAllText(Path.Combine(dataFolder, "userDB.json"), updatedFileJson);

            //Return the generated Id for the new user
            return newUser.UserId;
        }

        /// <summary>
        /// Finds user using the user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A user if a match is found, else an empty user object.</returns>
        public static User ReadUser(Guid userId)
        {
            var dataFolder = StorageHelper.GetDataFolder();
            List<User> userList;
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "userDB.json"));
                userList = JsonConvert.DeserializeObject<List<User>>(rawFileJson);
            }
            catch
            {
                return new User();
            }

            foreach (var user in userList)
            {
                if (user.UserId.Equals(userId))
                {
                    return user;
                }
            }

            return new User();
        }

        /// <summary>
        /// Finds user using email address
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>A user if a match is found, else an empty user object.</returns>
        public static User ReadUser(string userEmail)
        {
            var dataFolder = StorageHelper.GetDataFolder();
            List<User> userList;
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "userDB.json"));
                userList = JsonConvert.DeserializeObject<List<User>>(rawFileJson);
            }
            catch
            {
                return new User();
            }

            foreach (var user in userList)
            {
                if (user.UserEmail.Equals(userEmail))
                {
                    return user;
                }
            }

            return new User();
        }

        /// <summary>
        /// Finds user using userName and userPassword
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns>A user if a match is found, else an empty user object.</returns>
        public static User ReadUser(string userName, string userPassword)
        {
            var dataFolder = StorageHelper.GetDataFolder();
            List<User> userList;
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "userDB.json"));
                userList = JsonConvert.DeserializeObject<List<User>>(rawFileJson);
            }
            catch
            {
                return new User();
            }

            foreach (var user in userList)
            {
                if (user.UserName.Equals(userName))
                {
                    //If we found a matching userName, then it is worth the effort to decrypt the password.
                    var decryptedUserPassword = CryptographyHelper.TextDecode(user.UserPassword);

                    //If the password doesn't match, we are done and we didn't find a match
                    if (!userPassword.Equals(decryptedUserPassword)) break;

                    //Restore the decrypted user password
                    user.UserPassword = decryptedUserPassword;

                    //We found a match, so return the user
                    return user;
                }
            }

            return new User();
        }

        /// <summary>
        /// Updates the password of an existing user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userPassword"></param>
        /// <returns><c>true</c> if a match is found and updated, or <c>false</c> if the user is not found or cannot be updated.</returns>
        public static bool UpdateUser(Guid userId, string userPassword)
        {
            var dataFolder = StorageHelper.GetDataFolder();
            List<User> userList;
            try
            {
                var rawFileJson = File.ReadAllText(Path.Combine(dataFolder, "userDB.json"));
                userList = JsonConvert.DeserializeObject<List<User>>(rawFileJson);
            }
            catch
            {
                return false;
            }

            foreach (var user in userList)
            {
                if (user.UserId.Equals(userId))
                {
                    //If we found a matching Id, then it is worth the effort to decrypt the password.
                    var decryptedUserPassword = CryptographyHelper.TextDecode(user.UserPassword);
                    
                    //If the password doesn't match, we are done and we didn't find a match
                    if (!user.UserPassword.Equals(decryptedUserPassword)) break;

                    user.UserPassword = CryptographyHelper.TextEncode(userPassword);

                    //Save the collection with the updated user to disk
                    var updatedFileJson = JsonConvert.SerializeObject(userList);
                    File.WriteAllText(Path.Combine(dataFolder, "userDB.json"), updatedFileJson);
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="userId">The user Id of an existing user.</param>
        /// <param name="userPassword">The password for the user to be deleted.</param>
        /// <returns></returns>
        public static bool DeleteUser(Guid userId, string userPassword)
        {
            return true;
        }

    }
}