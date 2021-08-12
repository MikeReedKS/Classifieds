using System;
using System.Web.Http;
using Classifieds.DataAccess;
using Classifieds.Models;

namespace Classifieds.Services
{
    /// <summary>
    /// This Service Layer supports the User object.
    /// 
    /// All business logic belongs at the service layer.
    /// There isn't really any business logic for the user object, so this is just a pass-through layer, but it is still important
    /// and should not be skipped.
    ///
    /// Note that the service layer works to bridge the lexicon from Controller
    /// which uses nomenclature based on HTTP Verbs and the Data Access layer which
    /// uses the nomenclature of CRUD. This allows each domain to maintain a standard
    /// lexicon of terms and centralizes the conversion of terms to this layer.
    /// </summary>
    public static class UserService
    {
        /// <summary>
        /// Creates a new user object
        /// </summary>
        /// <param name="userName">Users User Name</param>
        /// <param name="userEmail">Users Email Address</param>
        /// <param name="userPassword">Users Password</param>
        /// <returns>GUID for User Id</returns>
        public static Guid PostUser(string userName, string userEmail, string userPassword)
        {
            return UserDataAccess.CreateUser(userName, userEmail, userPassword);
        }

        /// <summary>
        /// Gets existing user from user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User object</returns>
        public static User GetUser(Guid userId)
        {
            var user = UserDataAccess.ReadUser(userId);
            return user;
        }

        /// <summary>
        /// Gets an existing user from email address
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>User object</returns>
        public static User GetUser(string userEmail)
        {
            var user = UserDataAccess.ReadUser(userEmail);
            return user;
        }

        /// <summary>
        /// Gets an existing user from user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns>User object</returns>
        public static User GetUser(string userName, [FromBody] string userPassword)
        {
            var user = UserDataAccess.ReadUser(userName, userPassword);
            return user;
        }
    }
}