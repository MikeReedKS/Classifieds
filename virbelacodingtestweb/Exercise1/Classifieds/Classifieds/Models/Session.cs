using System;

namespace Classifieds.Models
{
    /// <summary>
    /// A single session object
    /// </summary>
    public class Session
    {
        /// <summary>
        /// user's Id
        /// </summary>
        public Guid UserId;

        /// <summary>
        /// The session Id last assigned to the user
        /// </summary>
        public string SessionId;

        /// <summary>
        /// When the session expires
        /// </summary>
        public DateTime SessionTimeout;
    }
}