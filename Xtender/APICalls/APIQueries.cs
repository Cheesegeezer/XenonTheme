using System;
using System.IO;
using System.Net;
using MediaBrowser.ApiInteraction;
using MediaBrowser.Library;
using MediaBrowser.Library.Entities;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Querying;

namespace Xenon.APICalls
{
    class APIQueries : BaseApiClient
    {
        //Required for BaseApiClient - No requirement for any methods
        protected override void SetAuthorizationHeader(string header)
        {
            //throw new NotImplementedException();
        }

        //The API address - taken from MBC
        private string APIUrl()
        {
                return Kernel.ApiClient.DashboardUrl.Split(new string[] { "dashboard" }, StringSplitOptions.None)[0];
        }
        
        //Mandatory call to GET and deserialize JSON items from the query string(Pretty standard HTTP call)
        private ItemsResult GetItemsResultAPIRespone(string queryUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(queryUrl);
            request.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return DeserializeFromStream<ItemsResult>(response.GetResponseStream());
            }
        }

        //Guid = UserId
        public ItemsResult NextUpAPIQuery(Guid guid)
        {
            string query = "&Limit=10&Fields=Name%2COverview%2CIsEpisode%2COfficialRating%2CStatus%2CPrimaryImageAspectRatio&format=Json"; //must include "&format=Json" in order to allow for the items to be read.
            string queryUrl = string.Format("{0}Shows/NextUp?UserId={1}{2}", APIUrl(), guid, query); //Query Format taken from Swagger
            return GetItemsResultAPIRespone(queryUrl);//Interrogate the API based on the query string.
        }

        public ItemsResult UpComingQuery(Guid guid)
        {
            string query = "&Fields=Name%2CId%2CSeriesName%2CIndexNumber%2C%20ParentIndexNumber%2CAirTime%2CSeriesStudio%2C%20PremiereDate%2CParentBackdropItemId&format=Json"; //must include "&format=Json" in order to allow for the items to be read.
            string queryUrl = string.Format("{0}Shows/Upcoming?UserId={1}{2}", APIUrl(), guid, query); //Query Format taken from Swagger
            return GetItemsResultAPIRespone(queryUrl);//Interrogate the API based on the query string.
        }
    }
}
