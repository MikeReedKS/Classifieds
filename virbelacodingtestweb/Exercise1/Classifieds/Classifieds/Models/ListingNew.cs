namespace Classifieds.Models
{
    /// <summary>
    /// Just the parts of a listing supplied by the user
    /// </summary>
    public class ListingNew
    {
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
    }
}