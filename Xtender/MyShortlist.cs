using System.Collections.Generic;
using MediaBrowser.Library;
using MediaBrowser.Library.Logging;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;
using Application = MediaBrowser.Application;

namespace Xenon
{
    public class MyShortlist : ModelItem
    {
        public MyShortlist()
        {
        }

        //Adds or Removes the Item from the Shortlist
        public void AddItemToShortlist(Item item)
        {
            if (_shortlist.Contains(item)) //Remove if Item Exists
            {
                
                Logger.ReportInfo("**XENON SHORTLIST** ATTEMPTING TO REMOVE ITEM --{0}-- TO SHORTLIST", item.Name);
                _shortlist.Remove(item);
                Logger.ReportInfo("**XENON SHORTLIST** REMOVED --{0}-- FROM SHORTLIST", item.Name);
                ShortlistOptionText();
                ItemInShortlist();
                this.FirePropertyChanged("AddItemToShortlist");
            }
            else
            {
                //Add if Item does not Exist
                Logger.ReportInfo("**XENON SHORTLIST** ATTEMPTING TO ADD ITEM --{0}-- TO SHORTLIST", item.Name);
                _shortlist.Add(item);
                Logger.ReportInfo("**XENON SHORTLIST** ADDED --{0}-- TO SHORTLIST", item.Name);
                ShortlistOptionText();
                ItemInShortlist();
                this.FirePropertyChanged("AddItemToShortlist");
            }
        }

        //Shortlist to be Displayed
        private static ArrayListDataSet _shortlist = new ArrayListDataSet();
        public ArrayListDataSet Shortlist
        {
            get { return _shortlist; }
            set
            {
                _shortlist = value;
                FirePropertyChanged("Shortlist");
            }
        }

        //Determines if the Shortlist has any Items in it
        public bool HasShortlistItems
        {
            get
            {
                if (_shortlist.Count == 0)
                {
                    return false;
                }
                return true;
            }
        }

        //Determines if the Current Item is in the Shortlist
        private bool _isItemInShortlist;
        public bool IsItemInShortlist
        {
            get
            {
                return _isItemInShortlist;
            }
            set
            {
                _isItemInShortlist = value;
                FirePropertyChanged("IsItemInShortlist");
            }
        }

        public void ItemInShortlist()
        {
            Item currItem = Application.CurrentInstance.CurrentItem;
            if (Shortlist.Contains(currItem))
            {
                //Logger.ReportInfo("+++++++++++++++++++++++++++ item Name = {0}",Application.CurrentInstance.CurrentItem.Name);
                IsItemInShortlist = true;
                this.FirePropertyChanged("ItemInShortlist");
            }
            else
            {
                //Logger.ReportInfo("---------------------------- item Name = {0}",Application.CurrentInstance.CurrentItem.Name);
                IsItemInShortlist = false;
                this.FirePropertyChanged("ItemInShortlist");
            }
        }

        //Gets the Option Text for the Context Menu and changes it when the user add/removes and item.
        private string _shortlistOption;
        public string ShortlistOption
        {
            get
            {
                if (_shortlistOption == null)
                {
                    _shortlistOption = Kernel.Instance.StringData.GetString("AddToShortlistOptionsLabel");
                }
                return _shortlistOption;
            }
            set
            {
                _shortlistOption = value;
                FirePropertyChanged("ShortlistOption");
            }
        }

        

        public void ShortlistOptionText()
        {
            Item currItem = Application.CurrentInstance.CurrentItem;
            if (_shortlist.Contains(currItem))
            {
                ShortlistOption = Kernel.Instance.StringData.GetString("RemoveFromShortlistOptionsLabel");
            }
            else
            {
                ShortlistOption = Kernel.Instance.StringData.GetString("AddToShortlistOptionsLabel");
            }
        }

        public void NoShortlistMessage()
        {
            string text = string.Format("You have NO items in Your Shortlist");
            Application.CurrentInstance.MessageBox(text, true, 0);
        }

        public void ClearShortlist()
        {
            MediaCenterEnvironment mediaCenterEnvironment = AddInHost.Current.MediaCenterEnvironment;
            const string text = "Are you sure you want clear your Shortlist?";
            const string caption = "SHORTLIST";

            if (mediaCenterEnvironment.Dialog(text, caption, DialogButtons.Cancel | DialogButtons.Ok, 0, true) ==
                DialogResult.Ok)
            {
                _shortlist.Clear();
                Application.CurrentInstance.MessageBox("Shortlist has been Emptied", true, 0);
            }
        }
    }
}