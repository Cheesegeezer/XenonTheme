﻿using System;
using System.IO;
using MediaBrowser.Library.Localization;
using MediaBrowser.Library.Logging;
using MediaBrowser.Library.Persistance; 

//***************************************************************************************************
//  This class is used to extend the string data used by MB.  It is localizable.
//  The most common use will be to provide description strings for config options.  To do this
//  define public string properties that are named the same as the label text of your options on your
//  config panel +desc.  A couple of examples have been generated by the template.
//***************************************************************************************************
namespace Xenon
{
    [Serializable]
    public class MyStrings : LocalizedStringData
    {
        const string VERSION = "1.020"; //this is used to see if we have changed and need to re-save

        // MCML: [Application.LocalStrings.#StringName!cor:String] 

        //these are our strings keyed by property name
        //EHS STRINGS
        public string EHSHeader = "<---------------------EHS LABELS--------------------->";
        public string UpcomingTVBtnStr = "UPCOMING TV";
        public string UpcomingTVLabelStr = "FUTURE EPISODES";
        public string NextUpBtnStr = "NEXTUP";
        public string NextUpLabelStr = "NEXTUP";
        public string NewBtnStr = "NEW";
        public string NewLabelStr = "NEW";
        public string WatchedBtnStr = "WATCHED";
        public string WatchedLabelStr = "WATCHED";
        public string UnwatchedBtnStr = "UNWATCHED";
        public string UnwatchedLabelStr = "UNWATCHED";
        public string LastPlayedLabelStr = "UNWATCHED";
        public string ResumeBtnStr = "RESUME";
        public string ResumeLabelStr = "RESUME";
        public string NoRecentItemsStr = "No Items Found";

        //INFO STRINGS
        public string ItemDetailsUIHeader = "<---------------------ITEM DETAILS LABELS--------------------->";
        public string DirectorStr = "Director: ";
        public string WritersStr = "Writers: ";
        public string GenresStr = "Genres: ";
        public string YearStr = "Year: ";
        public string RunsStr = "Runs ";
        public string EndStr = "Ends ";
        public string AiredStr = "Aired: ";
        public string StatusStr = "Status: ";
        public string PlayersStr = "Players: ";
        public string CompanyStr = "Company: ";
        public string ReleasedStr = "Released: ";

        //ACTOR BIO STRINGS
        public string ActorBioHeader = "<---------------------ACTOR BIO PAGE LABELS--------------------->";
        public string AgeStr = "Age: ";
        public string BornStr = "Born: ";
        public string BirthPlaceStr = "From: ";
        public string DiedStr = "Died: ";
        public string DiedAgedStr = "  Aged ";

        //FINAL DETAILS BUTTONS
        public string DetailsHeader = "<---------------------FINAL DETAILS PAGE LABELS--------------------->";
        public string ResumeStr = "RESUME";
        public string PlayStr = "PLAY";
        public string QuickPlayStr = "QUICKPLAY";
        public string TrailersStr = "TRAILERS";
        public string OverviewStr = "OVERVIEW";
        public string CastStr = "ACTORS";
        public string ChapterStr = "CHAPTERS";
        public string SpecialsStr = "SPECIALS";
        public string ReviewStr = "REVIEWS";
        public string RefreshStr = "REFRESH";
        public string DeleteStr = "DELETE";

        public string NextBtnStr = "NEXT";
        public string PrevBtnStr = "PREV";

        public string NoChaptersStr = "No Chapter Information";
        public string NoSpecialsStr = "No Special Features";

        //MUSIC DETAILS
        public string MusicHeader = "<---------------------MUSIC PAGE LABELS--------------------->";
        public string PlayAllStr = "PLAY ALL";
        public string ShuffleStr = "SHUFFLE";
        public string QueueAlbumStr = "QUEUE ALBUM";

        //MAIN XENON MBC CONFIG
        public string MainConfigHeader = "<---------------------MAIN MBC CONFIG LABELS--------------------->";
        public string ThemeStyleLabel = "Theme Style *";
        public string ThemeStyleLabelDesc = "Choose A Color Scheme that best suits your mood - REQUIRES RESTART";
        public string BackdropTransLabel = "Backdrop Transition Time";
        public string BackdropTransLabelDesc = "Alter the time it takes for Backdrops to change";
        public string BackdropAlphaLabel = "Backdrop Overlay Alpha";
        public string BackdropAlphaLabelDesc = " Set How Dark the Backdrop Overlay is on your collection views";
        public string EnableNPVLabel = "Enable NPV Backdrop";
        public string EnableNPVLabelDesc = "Allow Video Backdrop to play in the background";
        public string Enable24hrLabel = "Enable 24hr Time *";
        public string Enable24hrLabelDesc = "Toggle 12/24hr Time Format - REQUIRES RESTART";
        public string EnableQuickPlayLabel = "Enable QuickPlay Button";
        public string EnableQuickPlayLabelDesc = "Ideal when MB Intros is installed, it allows you to skip all Intros and go straight to the Movie.";
        public string GameDetailPosterLayoutLabel = "Game Poster Layout";
        public string GameDetailPosterLayoutLabelDesc = "Will reposition the Game Poster Image on Details Page,depending if you use Thumbs or Poster images";
        public string CustomDetailPosterLayoutLabel = "Custom Poster Layout";
        public string CustomDetailPosterLayoutLabelDesc = "Will reposition the Poster Image on the Details Page,depending if you use Custom images or not";

        //UI CONFIG MENU
        public string UserUIHeader = "<---------------------USER UI CONFIG LABELS--------------------->";
        public string LockConfigLabel = "Lock Configuration";
        public string UnlockConfigLabel = "Unlock Configuration";
        public string OptionsLabel = "Options";
        public string ConfigureLabel = "Configure";
        public string ViewStyleLabel = "View Style";
        public string ExtraViewStyleLabel = "Extra View Styles";
        public string ViewOptionsLabel = "View Options";
        public string ExtraViewLabel = "Extra View Styles";
        public string SortByLabel = "Sort By";
        public string GroupByLabel = "Group By";
        public string StarRatingStyleLabel = "Rating Style";

        //View Options Strings
        //EHS OPTIONS
        public string EHSUserUIHeader = "<---------------------EHS USER UI CONFIG LABELS--------------------->";
        public string ShowEHSBackdropLabel = "Show Backdrop";
        public string WrapEHSListLabel = "Wrap EHS List";
        public string ShowFullRALLabel = "Expand RAL to Full Width";
        public string ShowRALAlwaysLabel = "Always Show RAL";

        //FOLDER OPTIONS
        public string ViewOptionsUIHeader = "<---------------------VIEW OPTIONS UI CONFIG LABELS--------------------->";
        public string ShowBackdropLabel = "Show Backdrop";
        public string ShowBackdropOverlayLabel = "Show Backdrop Overlay";
        public string DetailsThumbAlphaLabel = "Thumb Art Alpha";
        public string ShowDetailsQuickListLabel = "Show QuickList View";
        public string ShowFlatCoverflowLabel = "Show Flat Coverflow View";
        public string FlatCFPositionLabel = "Flat Coverflow Position";
        public string VerticalScrollLabel = "Vertical Scroll";
        public string UseBannersLabel = "Use Banners";
        public string ShowLabelsLabel = "Show Labels";
        public string ShowBottomInfoLabel = "Show Bottom Info Bar";
        public string WrapItemListLabel = "Wrap View List";
        public string FanartItemListLabel = "Fanart Item Type";
        public string MainArtItemListLabel = "Main Poster Type";
        public string ShowDiscImageLabel = "Show Disc Image";
        
        public string ConfigRestartMessage = "Changes require a restart";

        public string IndexOfCountFormat = "{0} of {1}";

        public MyStrings() //for the serializer
        {
            Version = VERSION;
        }

        public static MyStrings FromFile(string file)
        {
            MyStrings s = new MyStrings();
            XmlSettings<MyStrings> settings = XmlSettings<MyStrings>.Bind(s, file);

            Logger.ReportInfo("Using String Data from " + file);

            if (VERSION != s.Version)
            {
                DateTime now = DateTime.Now;
                string backupFile = Path.Combine(Path.GetDirectoryName(file),
                                                       Path.GetFileNameWithoutExtension(file) +
                                                            "_" +
                                                            now.Year.ToString() +
                                                            now.Month.ToString("D2") +
                                                            now.Day.ToString("D2") +
                                                            now.Hour.ToString("D2") +
                                                            now.Minute.ToString("D2") +
                                                            now.Second.ToString("D2") +
                                                            ".BAK");

                Logger.ReportInfo("Xenon - Backing up strings file to: " + backupFile);

                if (File.Exists(backupFile))
                    File.Delete(backupFile);

                File.Move(file, backupFile);

                s = new MyStrings();
                settings = XmlSettings<MyStrings>.Bind(s, file);
            }
            return s;
        }
    }
}
