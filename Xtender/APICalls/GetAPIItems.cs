using System;
using System.IO;
using MediaBrowser.Library;
using MediaBrowser.Library.Entities;
using MediaBrowser.Library.Logging;
using MediaBrowser.Library.Util;
using MediaBrowser.Model.Dto;
using Microsoft.MediaCenter.UI;

namespace Xenon.APICalls
{
    internal class GetAPIItems
    {
        //Instantiate the APIQueries Class
        private static readonly APIQueries APIQuery = new APIQueries();

        internal GetAPIItems()
        {
        }

        //
        public static ArrayListDataSet GetNextUpSet()
        {
            //GUID taken from MBC Kernel
            Guid id = Kernel.CurrentUser.Id;
            ArrayListDataSet nextUpSet = new ArrayListDataSet();

            //Enumerate thru the list Items in the API call(add the UserId to allow for custom filtering)
            //
            foreach (BaseItemDto dto in APIQuery.NextUpAPIQuery(id).Items)
            {
                Item item = GetNextUpItem(dto);
                nextUpSet.Add(item);
            }
            return nextUpSet;
        }

        //Gets the NextUp item as long as it's of type episode
        private static Item GetNextUpItem(BaseItemDto dto)
        {
            //Retrieves the item based on the items guid
            BaseItem baseItem = Kernel.Instance.MB3ApiRepository.RetrieveItem(new Guid(dto.Id));
            //If the call to the api returns empty, catch it here.
            if (baseItem == null)
            {
                return null;
            }
            //Ensure that we only return episodes
            //Lets tell the Kernel what we want to return and create that item.
            Item episodeItem = ItemFactory.Instance.Create(baseItem);
            if (episodeItem.BaseItem is Episode)
            {
                TVHelper.CreateEpisodeParents(episodeItem);
            }
            return episodeItem;
        }

        public static ActorInfo GetPersonDtoStream(Item item)
        {
            Logger.ReportInfo("XENON - Getting Actor info for {0}", item.Name);

            string apiUrl = Kernel.ApiClient.DashboardUrl.Split(new[] {"dashboard"}, StringSplitOptions.None)[0];
            //Use the standard API Prefix
            string queryUrl = string.Format("{0}Persons/{1}", apiUrl, Uri.EscapeUriString(item.Name));
            //Query Format taken from Swagger

            BaseItemDto dto;

            //get the Json Stream
            using (Stream stream = Kernel.ApiClient.GetSerializedStream(queryUrl))
            {
                //Deserialize the Json stream and put in local variable
                dto = Kernel.ApiClient.DeserializeFromStream<BaseItemDto>(stream);
            }
            //return the dto
            return SaveActorInfo(dto);
        }


        internal static ActorInfo SaveActorInfo(BaseItemDto personDto)
        {
            ActorInfo info = new ActorInfo
            {
                Id = personDto.Id, //string
                Name = personDto.Name, //string
                Born = personDto.PremiereDate.HasValue ? personDto.PremiereDate.Value.ToString("D") : null,
                Died = personDto.EndDate.HasValue ? personDto.EndDate.Value.ToString("d MMM yyyy") : null,
                Bio = personDto.Overview, //string
                BirthPlaceLocations = personDto.ProductionLocations, //List<string>
                BornDate = personDto.PremiereDate, //DateTime
                DiedDate = personDto.EndDate //DateTime
            };
            info.HasLoaded = true;
            Logger.ReportInfo(
                "Name = {0} || DTO BORN = {6} ||Born = {1} , Aged {4} || Died = {2} || Birthplace = {3} || Biography: {5}",
                info.Name, info.Born, info.Died, info.BirthPlace, info.Age, info.Bio, personDto.PremiereDate);
            return info;
        }

        public static ArrayListDataSet GetUpcomingItemsSet()
        {
            Guid id = Kernel.CurrentUser.Id;
            ArrayListDataSet upcomingSet = new ArrayListDataSet();
            string str = string.Empty;
            foreach (BaseItemDto dto in APIQuery.UpComingQuery(id).Items)
            {
                Item item = GetUpcomingTVItem(dto);
                upcomingSet.Add(item);

            }
            return upcomingSet;
        }

        private static Item GetUpcomingTVItem(BaseItemDto dto)
        {
            //Retrieves the item based on the items guid
            BaseItem baseItem = Kernel.Instance.MB3ApiRepository.RetrieveItem(new Guid(dto.Id));
            //If the call to the api returns empty, catch it here.
            if (baseItem == null)
            {
                return null;
            }
            //Ensure that we only return episodes
            //Lets tell the Kernel what we want to return and create that item.
            Item episodeItem = ItemFactory.Instance.Create(baseItem);
            if (episodeItem.BaseItem is Episode)
            {
                TVHelper.CreateEpisodeParents(episodeItem);
            }
            return episodeItem;
        }

    }
}
