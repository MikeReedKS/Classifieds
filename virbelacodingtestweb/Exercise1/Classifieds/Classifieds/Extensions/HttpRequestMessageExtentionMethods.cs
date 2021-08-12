using System.Linq;
using System.Net.Http;
using Microsoft.Ajax.Utilities;

namespace Classifieds.Extensions
{
    //I'm a huge fan of extension methods, so just had to toss one in :-)

    /// <summary>
    /// Extension methods for the HttpRequestMessage type
    /// </summary>
    public static class HttpRequestMessageExtensionMethods
    {
        /// <summary>
        /// Check the headers to assure that the API Key is valid.
        /// In this case, there is just one API Key hard coded which is 1234567.
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static bool ValidateApiKey(this HttpRequestMessage targetObject)
        {
            //Get the header object 
            var headers = targetObject.Headers;
            
            //Get the Api Key from the header if it exists
            var apiKey = "";
            if (headers.Contains("APIKey"))
            {
                apiKey = headers.GetValues("APIKey").FirstOrDefault();
            }

            //A bit redundant, but would let us throw a more informative exception if we wanted more detail back to the client
            if (apiKey.IsNullOrWhiteSpace())
            {
                return false;
            }

            //Obviously this method is for demo only. We'd need a DB with a list of approved API Keys and would check against it
            //vs. having a hard coded single value, but this made this demo simple while still implementing simple security. 
            return apiKey == "1234567";
        }
    }
}