using System;

namespace Classifieds.Models
{
    /// <summary>
    /// The Listing Object
    /// </summary>
    public class Listing
    {
        /// <summary>
        /// The unique id for the listing
        /// </summary>
        public Guid ListingId;

        /// <summary>
        /// Title of the listing
        /// </summary>
        public string Title;

        /// <summary>
        /// Description of the item for sale
        /// </summary>
        public string Description;

        /// <summary>
        /// The price to purchase the item
        /// </summary>
        public string Price;

        /// <summary>
        /// When the listing was created
        /// </summary>
        public DateTime ListedDate;

        /// <summary>
        /// When the listing was last updated
        /// </summary>
        public DateTime ListingUpdated;

        /// <summary>
        /// The user who listed the item for sale
        /// </summary>
        public Guid UserId;
    }
}