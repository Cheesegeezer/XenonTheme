using System;
using System.Collections.Generic;
using System.IO;
using MediaBrowser;
using MediaBrowser.Library;
using MediaBrowser.Library.Entities;
using MediaBrowser.Library.Extensions;
using MediaBrowser.Library.Localization;
using MediaBrowser.Library.Logging;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;
using Xenon.APICalls;
using Application = MediaBrowser.Application;

namespace Xenon
{
    public class XenonHelper : ModelItem
    {

        private readonly Timer OverviewTimer;
        private int NavCount;
        private Item _currentItem;
        public bool _showOverveiew = new bool();
        private static Image defaultBackdrop;
        private static Image currentBackdrop;
        private FolderModel _currentTopParent;
        private FolderModel _currentParent;
        private static string currentPage = "Page";

        public XenonHelper()
        {
            OverviewTimer = new Timer();
            setupHelper();
        }

        public XenonHelper(Item Item)
        {
            OverviewTimer = new Timer();
            CurrentItem = Item;
            setupHelper();
        }


        public MediaBrowser.Library.Item CurrentItem
        {
            get
            {
                return this._currentItem;
            }
            set
            {
                this._currentItem = value;
                if (this.NavCount > 1)
                {
                    if (this.ShowOverview)
                    {
                        this.ShowOverview = false;
                    }
                    this.NavCount = 0;
                }
                this.NavCount++;
                this.OverviewTimer.Stop();
                this.OverviewTimer.Start();
                base.FirePropertyChanged("EndTime");
                base.FirePropertyChanged("CurrentItem");
                base.FirePropertyChanged("GenreString");
                base.FirePropertyChanged("PercentWatched");
                this.FireMusicChanges();
                this.FireGameChanges();
            }
        }

        public FolderModel CurrentFolder
        {
            get { return Application.CurrentInstance.CurrentFolder; }
        }

        public FolderModel SelectedChild
        {
            get { return Application.CurrentInstance.CurrentFolder.SelectedChild as FolderModel; }
        }

        public bool IsTVShowFolder
        {
            get
            {
                return SelectedChild != null && SelectedChild.CollectionType == "tvshows";
            }
        }

        public bool IsMusicFolder
        {
            get
            {
                return SelectedChild != null && SelectedChild.CollectionType == "music";
            }
        }

        public bool IsMovieFolder
        {
            get
            {
                return SelectedChild != null && SelectedChild.CollectionType == "movie";
            }
        }

        public bool IsGameFolder
        {
            get
            {
                return SelectedChild != null && SelectedChild.CollectionType == "game";
            }
        }

        public string CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
            }
        }

        public string EndTime
        {
            get
            {
                string endTime = string.Empty;

                if (!string.IsNullOrEmpty(_currentItem.EndTimeString))
                {
                    endTime = _currentItem.EndTimeString.Replace(LocalizedStrings.Instance.GetString("EndsStr") + " ","");
                }

                return endTime;
            }
        }

        public string CurrentTime
        {
            get
            {
                DateTime time = DateTime.Now;
                if (this.Config.Enable24hrTime)
                {
                    return time.ToString("HH:mm");
                }
                return time.ToString("h:mm tt");
            }
        }

        public string CurrentDate
        {
            get
            {
                DateTime date = DateTime.Now;
                if (CurrentFolder.IsRoot)
                {
                    return date.ToLongDateString();
                }
                return null;
            }
        }

        public MyConfig ThemeConfig
        {
            get
            {
                if (Plugin.config == null)
                    Plugin.config = new MyConfig();

                return Plugin.config;
            }
        }

        //private string calculateEndTime()
        //{
        //    string runtime = CurrentItem.RunningTimeString;
        //    if (runtime == "" && CurrentItem.PhysicalParent != null)
        //        runtime = CurrentItem.PhysicalParent.RunningTimeString;
        //    if(runtime != "")
        //    {
        //        runtime = runtime.Replace(" mins", "");
        //        int minutes = Int32.Parse(runtime);
        //        DateTime dt = DateTime.Now;
        //        dt = dt.AddMinutes(minutes);

        //        int Hour = dt.Hour;
        //        if(dt.Hour > 12)
        //        {
        //            Hour = Hour - 12;
        //        }

        //        if (dt.Minute > 9)
        //        {
        //            return Hour.ToString() + ":" + dt.Minute.ToString();
        //        }
        //        else
        //        {
        //            return Hour.ToString() + ":0" + dt.Minute.ToString();
        //        }


        //    }
        //    return "";
        //}

        private static bool navigatingForward = true;
        public bool NavigatingForward
        {
            get
            {
                return navigatingForward;
            }
            set
            {
                navigatingForward = value;
                base.FirePropertyChanged("NavigatingForward");
            }
        }

        public Image DefaultBackdrop
        {
            get
            {
                return defaultBackdrop;
            }
            set
            {
                defaultBackdrop = value;
            }
        }

        public Image CurrentBackdrop
        {
            get
            {
                return currentBackdrop;
            }
            set
            {
                currentBackdrop = value;
            }
        }

        public bool IsMenuOpen
        {
            get { return isMenuOpen; }
            set
            {
                isMenuOpen = value;
                base.FirePropertyChanged("IsMenuOpen");
            }
        }

        public MyConfig Config { get; set; }

        public bool ShowOverview
        {
            get { return _showOverveiew; }
            set
            {
                _showOverveiew = value;
                FirePropertyChanged("ShowOverview");
            }
        }

        public string GenreString
        {
            get
            {
                string genrestring = "";
                CurrentItem.Genres.ForEach(delegate(string item) { genrestring = genrestring + item + ", "; });
                if (genrestring.Length > 0)
                {
                    genrestring = genrestring.Substring(0, genrestring.Length - 2);
                }
                return genrestring;
            }
        }

        public string WritersString
        {
            get
            {
                string writersstring = "";
                CurrentItem.Writers.ForEach(delegate(string item) { writersstring = writersstring + item + ", "; });
                if (writersstring.Length > 0)
                {
                    writersstring = writersstring.Substring(0, writersstring.Length - 2);
                }
                return writersstring;
            }
        }

        public float PercentWatched
        {
            get { return CalculatePercentWatched(); }
        }

        private bool _ralHasFocus;
        public bool RALHasFocus
        {
            get
            {
                return this._ralHasFocus;
            }
            set
            {
                this._ralHasFocus = value;
                base.FirePropertyChanged("RALHasFocus");
            }
        }

        public int SelectedIndex { get; set; }

        public bool IsSongBackdropPlaying
        {
            get
            {
                if (Application.CurrentInstance.IsPlayingVideo)
                {
                    return false;
                }
                return true;
            }
        }

        public bool VideoBackdropApplicable
        {
            get
            {
                var conf = new MyConfig();
                if (!conf.EnableVideoBackdrop)
                {
                    return false;
                }
                Type pc = Application.CurrentInstance.PlaybackController.GetType();
                if (pc != typeof (PlaybackController))
                {
                    return false;
                }
                if (!Application.CurrentInstance.PlaybackController.IsPlaying)
                {
                    return false;
                }
                return true;
            }
        }

        #region New Item Notification

        private string _dateStr;
        private Item _newItem;
        private bool _showNewItemPopout;
        private bool isMenuOpen;
                

        public bool ShowNewItemPopout
        {
            get { return _showNewItemPopout; }
            set
            {
                if (_showNewItemPopout != value)
                {
                    _showNewItemPopout = value;
                    FirePropertyChanged("ShowNewItemPopout");
                }
            }
        }

        public Item NewItem
        {
            get { return _newItem; }
            set
            {
                if (_newItem != value)
                {
                    _newItem = value;
                    FirePropertyChanged("NewItem");
                }
            }
        }

        public string Date
        {
            get { return _dateStr; }
            set
            {
                if (_dateStr != value)
                {
                    _dateStr = value;
                    base.FirePropertyChanged("Date");
                }
            }
        }

        public bool Display24Hr { get; set; }

        #endregion

        #region Prevent Quit from EHS

        public void AskToQuit()
        {
            MediaCenterEnvironment mediaCenterEnvironment = AddInHost.Current.MediaCenterEnvironment;
            const string text = "Are you sure you want to quit MediaBrowser?";
            const string caption = "Quit MediaBrowser";
            if (mediaCenterEnvironment.Dialog(text, caption, DialogButtons.Cancel | DialogButtons.Ok, 0, true) ==
                DialogResult.Ok)
            {
                Application.CurrentInstance.BackOut();
            }
        }

        #endregion

        #region Music Metadata

        public string Duration
        {
            get
            {
                string duration = GetDynamicProperty("Duration");

                if (duration != null)
                {
                    return duration;
                }
                return "";
            }
        }

        public string Album
        {
            get
            {
                string album = GetDynamicProperty("Album");

                if (album != null)
                {
                    return album;
                }
                return "";
            }
        }

        public string Artist
        {
            get
            {
                string artist = GetDynamicProperty("Artist");

                if (artist != null)
                {
                    return artist;
                }
                return "";
            }
        }


        public string ProductionYear
        {
            get
            {
                string productionYear = GetDynamicProperty("ProductionYear");


                if (productionYear != null)
                {
                    return productionYear;
                }
                return "";
            }
        }

        private void FireMusicChanges()
        {
            FirePropertyChanged("Album");
            FirePropertyChanged("Artist");
            FirePropertyChanged("ProductionYear");
            FirePropertyChanged("Duration");
        }


        private string GetDynamicProperty(string PropertyName)
        {
            try
            {
                return CurrentItem.DynamicProperties[PropertyName] as string;
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion

        #region Game Metadata

        public string Players
        {
            get
            {
                string players = GetDynamicProperty("Players");

                if (players != null)
                {
                    return players;
                }
                return "";
            }
        }


        public Single TgdbRating
        {
            get
            {
                string rating = GetDynamicProperty("TgdbRating");

                if (!string.IsNullOrEmpty(rating))
                {
                    return Convert.ToSingle(rating);
                }
                return 0;
            }
        }


        public string EsrbRating
        {
            get
            {
                string rating = GetDynamicProperty("EsrbRating");

                if (rating != null)
                {
                    return rating;
                }

                return "";
            }
        }

        public string ConsoleReleaseYear
        {
            get
            {
                int year = GetDynamicPropertyAsInt("ReleaseYear");

                if (year != 0)
                {
                    return year.ToString();
                }

                return "";
            }
        }

        public string Company
        {
            get
            {
                string company = GetDynamicProperty("Manufacturer");

                if (company != null)
                {
                    return company;
                }

                return "";
            }
        }

        private void FireGameChanges()
        {
            FirePropertyChanged("Players");
            FirePropertyChanged("TgdbRating");
            FirePropertyChanged("ProductionYear");
            FirePropertyChanged("EsrbRating");
            FirePropertyChanged("Company");
            FirePropertyChanged("ConsoleReleaseYear");
        }

        private int GetDynamicPropertyAsInt(string PropertyName)
        {
            try
            {
                return Convert.ToInt32(CurrentItem.DynamicProperties[PropertyName]);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        public void setupHelper()
        {
            SelectedIndex = 0;
            Config = new MyConfig();
            ShowOverview = false;
            OverviewTimer.AutoRepeat = false;
            OverviewTimer.Interval = 1000;
            OverviewTimer.Tick += delegate
            {
                if ((((this.CurrentItem.Overview != "") && !this.ShowOverview) && 
                    ((this.CurrentItem.ItemTypeString != "Season") && 
                    (this.CurrentItem.ItemTypeString != "Album"))) && 
                    (((this.CurrentItem.ItemTypeString != "ArtistAlbum") && 
                    (this.CurrentItem.ItemTypeString != "Folder")) && 
                    (this.CurrentItem.ItemTypeString != "MusicFolder")))

                    ShowOverview = true;
                NavCount = 0;
            };
        }

        public bool getProperty(string propertyname)
        {
            return ShowOverview;
        }

        private float CalculatePercentWatched()
        {
            float num = 0f;
            if (!string.IsNullOrEmpty(CurrentItem.RunningTimeString))
            {
                int num2 =
                    int.Parse(CurrentItem.RunningTimeString.Substring(0, CurrentItem.RunningTimeString.IndexOf(' ')));
                var totalMinutes = (int) CurrentItem.WatchedTime.TotalMinutes;
                if ((num2 > 0) && (totalMinutes > 0))
                {
                    num = totalMinutes/((float) num2);
                }
            }
            return num;
        }

        public Rotation getRotation(int Index)
        {
            int indexDifference = SelectedIndex - Index;
            var rot = new Rotation();
            if (indexDifference == 0)
            {
                rot.AngleDegrees = 0;
                return rot;
            }
            if (indexDifference <= 0)
            {
                if (Math.Abs(indexDifference) == 1)
                {
                    rot.AngleDegrees = 15;
                }
                else if (Math.Abs(indexDifference) == 2)
                {
                    rot.AngleDegrees = 30;
                }
                else if (Math.Abs(indexDifference) == 3)
                {
                    rot.AngleDegrees = 45;
                }
                else
                {
                    rot.AngleDegrees = 55;
                }
            }
            else
            {
                rot.AngleDegrees = 0;
            }
            return rot;
        }

        public int getIndexdif(int Index)
        {
            return SelectedIndex - Index;
        }

        public Vector3 getScale(int Index)
        {
            int indexDifference = SelectedIndex - Index;
            var scale = new Vector3();
            if (indexDifference == 0)
            {
                scale.X = 1.6f;
                scale.Y = 1.6f;
                scale.Z = 1.6f;
                return scale;
            }
            if (indexDifference <= 0)
            {
                if (Math.Abs(indexDifference) == 1)
                {
                    scale.X = 1.4f;
                    scale.Y = 1.4f;
                    scale.Z = 1.4f;
                }
                else if (Math.Abs(indexDifference) == 2)
                {
                    scale.X = 1.25f;
                    scale.Y = 1.25f;
                    scale.Z = 1.25f;
                }
                else if (Math.Abs(indexDifference) == 3)
                {
                    scale.X = 1.1f;
                    scale.Y = 1.1f;
                    scale.Z = 1.1f;
                }
                else
                {
                    scale.X = 1f;
                    scale.Y = 1f;
                    scale.Z = 1f;
                }
            }
            else
            {
                scale.X = 1.4f;
                scale.Y = 1.4f;
                scale.Z = 1.4f;
            }
            return scale;
        }

        public float getPosition(int Index)
        {
            int indexDifference = SelectedIndex - Index;
            if (Math.Abs(indexDifference) > 1)
            {
                return 60*indexDifference;
            }
            return 0;
        }


        public Inset getMargin(int Index)
        {
            int indexDifference = SelectedIndex - Index;

            indexDifference = Math.Abs(indexDifference);
            if (indexDifference > 1)
            {
                return new Inset(-15, 0, -15, 0);
            }
            return new Inset(0, 0, 0, 0);
        }


        #region FolderConfig Helper

        public int Negate(int number)
        {
            return (-number);
        }

        public Guid GetFolderPrefsId(FolderModel folder)
        {
            Guid id = Guid.Empty;

            if (folder != null)
            {
                id = folder.Id;

                if (MediaBrowser.Config.Instance.EnableSyncViews)
                {
                    if (folder.BaseItem is Folder && folder.BaseItem.GetType() != typeof (Folder))
                    {
                        id = folder.BaseItem.GetType().FullName.GetMD5();
                    }
                }

            }

            return (id);
        }

        public FolderModel CurrentParent
        {
            get { return (_currentParent); }
            set
            {
                _currentParent = value;
                _currentTopParent = null;

                if (_currentParent != null)
                {
                    FolderModel curParent = _currentParent;

                    while (true)
                    {
                        if ((curParent.PhysicalParent == null) || curParent.PhysicalParent.IsRoot)
                        {
                            _currentTopParent = curParent;
                            break;
                        }
                        curParent = curParent.PhysicalParent;
                    }
                }

                FirePropertyChanged("CurrentTopParent");
            }
        }

        public FolderModel CurrentTopParent
        {
            get { return _currentTopParent; }
        }

        public void LoadDisplayPrefs(FolderModel folder)
        {
            if (folder != null)
            {
                // Referencing folder.DisplayPrefs causes it to be loaded if
                // it doesn't already exist
                Guid id = folder.DisplayPrefs.Id;
            }
        }
        #endregion
        
        #region MediaInfo

        public string Resolution
        {
            get { return CurrentItem.MediaInfo.Width + "x" + CurrentItem.MediaInfo.Height; }
        }

        public string AudioBitRate
        {
            get { return (CurrentItem.MediaInfo.AudioBitRate/1000) + "Kbps"; }
        }

        public string VideoBitRate
        {
            get { return (CurrentItem.MediaInfo.VideoBitRate/1000) + "Kbps"; }
        }

        public string VideoCodec
        {
            get { return CurrentItem.MediaInfo.VideoCodec; }
        }

        public string AudioFormat
        {
            get { return CurrentItem.MediaInfo.AudioFormat; }
        }

        public string AudioStreamCount
        {
            get { return CurrentItem.MediaInfo.AudioStreamCount.ToString(); }
        }

        public string AudioChannelCount
        {
            get { return CurrentItem.MediaInfo.AudioChannelCount; }
        }

        public string VideoFPS
        {
            get { return CurrentItem.MediaInfo.VideoFPS; }
        }

        #endregion

        #region Custom Views
        public string LayoutRoot
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutRoot#LayoutRoot"; }
        }

        /*public string LayoutCoverFlow2
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutCV2#XenonLayoutCV2"; }
        }

        public string LayoutDetailsQuickList
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutDetailsQuickList#XenonLayoutDetailsQuickList"; }
        }

        public string LayoutBannerView
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutBannerView#XenonLayoutBannerView"; }
        }*/

        public string LayoutCoverflow
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutCoverflow#XenonLayoutCoverflow"; }
        }

        public string LayoutDetails
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutDetails#XenonLayoutDetails"; }
        }

        public string LayoutPoster
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutPoster#XenonLayoutPoster"; }
        }

        public string LayoutThumb
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutThumb#XenonLayoutThumb"; }
        }

        public string LayoutThumbStrip
        {
            get { return "resx://Xenon/Xenon.Resources/LayoutThumbStrip#XenonLayoutThumbStrip"; }
        }

        #endregion


        public Item GetQuickListItem(FolderModel folder, int index)
        {
            Item item = null;

            if ((folder != null) && (index >= 0) && (index < folder.QuickListItems.Count))
                item = folder.QuickListItems[index];

            return (item);
        }


        public ArrayListDataSet GetActorLocalCredits(Item item)
        {
            ArrayListDataSet itemSet = new ArrayListDataSet();
            ActorItemWrapper aiw = GetAPIItems.GetPersonDtoStream(item).Item;
            
            Item credItem = aiw.Item;
            itemSet.Add(credItem);
            return itemSet;
        }
        
        #region Oh Yeah

        public ArrayListDataSet GetNextUpEpisodes()
        {
            return GetAPIItems.GetNextUpSet();
        }

        public ArrayListDataSet GetUpcomingTV()
        {
            return GetAPIItems.GetUpcomingItemsSet();
        }

        public ActorItemWrapper GetActor(Item item, int index)
        {
            ActorItemWrapper wrapper = null;
            if (((item != null) && (index >= 0)) && (index < item.Actors.Count))
            {
                wrapper = item.Actors[index];
            }
            return wrapper;
        }

        public void OpenActorPage(Item item)
        {
            ActorInfo pInfo = GetAPIItems.GetPersonDtoStream(item);
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Application", Application.CurrentInstance);
            properties.Add("Item", item);
            properties.Add("Person", pInfo);
            Application.CurrentInstance.OpenMCMLPage("resx://Xenon/Xenon.Resources/ActorPopup#ActorPopup", properties);
        }

        /*public ArrayListDataSet GetTVFolderQuickListOption
        {
            get
            {
                FolderConfigData config = null;
                if (config.TvRalOption == "UpcomingTV")
                {
                    return GetUpcomingTV();
                }
                else if (config.TvRalOption == "NextUp")
                {
                    return GetNextUpEpisodes();
                }
                else if (config.TvRalOption == "added")
                {
                    return new ArrayListDataSet {CurrentFolder.NewestItems};
                }
                else if (config.TvRalOption == "watched")
                {
                    return new ArrayListDataSet {CurrentFolder.LastWatchedItem};
                }
                else if (config.TvRalOption == "unwatched")
                {
                    return new ArrayListDataSet {CurrentFolder.UnwatchedItems};
                }
                return new ArrayListDataSet {CurrentFolder.NewestItems};

            }
        }

        public ArrayListDataSet GetNonTVFolderQuickListOption
        {
            get
            {
                FolderModel folder = CurrentFolder;
                FolderConfigData config = null;
                
                if (config.NonTVRalOption == "added")
                {
                    return new ArrayListDataSet { folder.NewestItems };
                }
                if (config.NonTVRalOption == "watched")
                {
                    return new ArrayListDataSet { folder.LastWatchedItem };
                }
                if (config.NonTVRalOption == "unwatched")
                {
                    return new ArrayListDataSet { folder.UnwatchedItems};
                }
                return new ArrayListDataSet { folder.NewestItems };

            }
        }*/

        protected FolderModel gameFolder;
        private  Item _lastPlayedGame;
        public ArrayListDataSet GetLastPlayedGames()
        {
            ArrayListDataSet set = new ArrayListDataSet();
            if (IsGameFolder && Application.CurrentInstance.Config.TreatWatchedAsInProgress)
            {
                Item item = gameFolder.LastWatchedItem;
                set.Add(item);
                return set;
            }
            return null;
        }
        #endregion
    }
}