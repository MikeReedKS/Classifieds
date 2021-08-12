using System;

namespace Classifieds.Models
{
    /// <summary>
    /// The User object.
    /// The user logs in to post classified ads or to search for them.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The GUID used to describe the user.
        /// </summary>
        public Guid UserId;

        /// <summary>
        /// The user name selected by the user as a unique id representing them.
        /// </summary>
        public string UserName;

        /// <summary>
        /// The users email address.
        /// </summary>
        public string UserEmail;

        /// <summary>
        /// The users password.
        /// </summary>
        public string UserPassword;
    }
}