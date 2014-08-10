using System;
using System.Collections.Generic;
using System.IO;
using MediaBrowser.Library.Localization;
using MediaBrowser.Library.Plugins;
using MediaBrowser.Library.Logging;
using MediaBrowser.Library;
using MediaBrowser.Library.Util;
using MediaBrowser.Util;
using Xenon.Fonts;

namespace Xenon
{
    class Plugin : BasePlugin
    {
        static readonly Guid XenonGuid = new Guid("2d137802-544d-44b4-9d01-af2acc6e8b6c");
        private FontManager _fontManager;
        public static List<string> AvailableStyles = new List<string>();
        public static MyConfig config = null;
        public static List <string>ExtraViewsList = new List<string>();
        //private ActorInfo _actorInfo;

        public Plugin()
        {
            Logger.ReportInfo("Xenon - Creating Theme");

            using (new Profiler("Xenon - Theme Creation"))
            {
                bool isMC = AppDomain.CurrentDomain.FriendlyName.Contains("ehExtHost");
                if (isMC)
                {
                     MyConfig.InitDisplayPrefs();
                     CustomStyles.InitStyles();

                    if (config == null)
                        config = new MyConfig();
                }
            }
        }

        /// <summary>
        /// The Init method is called when the plug-in is loaded by MediaBrowser.  You should perform all your specific initializations
        /// here - including adding your theme to the list of available themes.
        /// </summary>
        /// <param name="kernel"></param>
        public override void Init(Kernel kernel)
        {
            try
            {
                _fontManager = new FontManager();
                _fontManager.FontCollection();
                kernel.AddTheme("Xenon", "resx://Xenon/Xenon.Resources/Page#PageXenon", "resx://Xenon/Xenon.Resources/DetailMovieView#XenonMovieView", config);
                
                bool isMC = AppDomain.CurrentDomain.FriendlyName.Contains("ehExtHost");
                if (isMC)
                {       
                        if(config == null)
                        config = new MyConfig();
                        //_actorInfo = new ActorInfo();
                        //_actorInfo.Init();

                    //If you want to add any context menus they need to be inside this logic as well.
                    kernel.AddConfigPanel("Xenon Options", "resx://Xenon/Xenon.Resources/ConfigPanel#ConfigPanel", config);
                    //Check the current active style applied
                    config.CheckActiveStyle();
                    //Append styles with custom fonts
                    CustomResourceManager.AppendFonts("Xenon", Resources.Fonts, Resources.Fonts);
                    CustomResourceManager.AppendStyles("Xenon", Resources.Colors, Resources.Colors);
                    
                    
                    //CustomStrings Editable by user - need to implement
                    kernel.StringData.AddStringData(MyStrings.FromFile(LocalizedStringData.GetFileName("Xenon-")));
                }
                else Logger.ReportInfo("Not creating menus for Xenon.  Appear to not be in MediaCenter.  AppDomain is: " + AppDomain.CurrentDomain.FriendlyName);
                
                //Tell the log we loaded.
                Logger.ReportInfo("Xenon Theme Loaded.");
            }
            catch (Exception ex)
            {
                Logger.ReportException("Error adding theme - probably incompatable MB version", ex);
            }

        }

        public override string Name
        {
            //provide a short name for your theme - this will display in the configurator list box
            get { return "Xenon Theme"; }
        }

        public override string Description
        {
            //provide a longer description of your theme - this will display when the user selects the theme in the plug-in section
            get { return "A Basic Theme with rich content, perfect for XBox Extenders"; }
        }

        //Only un-comment this if you have a rich description resource
        //public override string RichDescURL
        //{
        //    //You can return a fully-qualified URI to a resource that displays a rich description of your plugin here
        //    get { return "http://www.mysite.com/XenonDesc.htm"; }
        //}

        
    }
}
