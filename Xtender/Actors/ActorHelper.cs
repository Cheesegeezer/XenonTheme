using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaBrowser;
using MediaBrowser.Library;
using MediaBrowser.Library.Entities;
using MediaBrowser.Library.Persistance;
using MediaBrowser.Library.Threading;
using MediaBrowser.Library.Util;
using MediaBrowser.LibraryManagement;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Querying;
using Microsoft.MediaCenter.UI;
using Application = MediaBrowser.Application;

namespace Xenon.Actors
{
    class ActorHelper
    {
        
        private static string UrlEncode(string name)
        {
            return HttpUtility.UrlEncode(name);
        }
    }
}
