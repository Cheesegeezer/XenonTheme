using MediaBrowser.Library;
using Microsoft.MediaCenter.UI;

namespace Xenon.Actors
{
    public class ActorCreditItem : ModelItem
    {
        private string _character;
        private bool _have = false;
        private static string _id;
        private Item _item;
        private string _release = "";
        private string _title;

        public string Character
        {
            get
            {
                return this._character;
            }
            set
            {
                this._character = value;
                base.FirePropertyChanged("Character");
            }
        }

        public bool Have
        {
            get
            {
                return this._have;
            }
            set
            {
                this._have = value;
                base.FirePropertyChanged("Have");
            }
        }

        public static string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                
            }
        }

        public MediaBrowser.Library.Item Item
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
                base.FirePropertyChanged("Item");
            }
        }

        public string Release
        {
            get
            {
                return this._release;
            }
            set
            {
                this._release = value;
                base.FirePropertyChanged("Release");
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                base.FirePropertyChanged("Title");
            }
        }
    }
}
