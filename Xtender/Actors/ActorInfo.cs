using System;
using System.Collections.Generic;
using System.Text;
using MediaBrowser.Library;
using Microsoft.MediaCenter.UI;

namespace Xenon
{
    public class ActorInfo
    {
        private string _id;
        private string _name;
        private string _born;
        private string _died;
        private string _bio;
        private List<string> _birthPlaceLocations;
        private DateTime _bornDate;
        private DateTime _diedDate;
        private bool _hasLoaded;
        public ArrayListDataSet _credits = new ArrayListDataSet();

        public string Id
        {
            get
            {
                if (_id == null)
                {
                    _id = "No Id Available";
                }
                return _id;
            }
            set { _id = value; }
        }

        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = "No Name Available";
                }
                return _name;
            }
            set { _name = value; }
        }

        
        public string Born
        {
            get
            {
                if (_born == null)
                {
                    _born = "No Birthdate Available";
                }
                return _born;
            }
            set { _born = value; }
        }

        public string Died
        {
            get { return _died; }
            set { _died = value; }
        }

        public string Bio
        {
            get
            {
                if (_bio == null)
                {
                    _bio = "No Biography Available";
                }
                string noWiki;
                string wiki1 = "From Wikipedia, the free encyclopedia.";
                string wiki2 = "From Wikipedia, the free encyclopedia:";
                string wiki3 = "From Wikipedia, the free encyclopedia";
                string wiki4 = "From Wikipedia, the free encyclopedia ";
                if (_bio.Contains(wiki1))
                {
                    
                    noWiki = _bio.Replace("From Wikipedia, the free encyclopedia.","");
                    return noWiki;
                }
                if (_bio.Contains(wiki2))
                {
                    noWiki = _bio.Replace("From Wikipedia, the free encyclopedia: ", "");
                    return noWiki;
                }
                if (_bio.Contains(wiki3))
                {

                    noWiki = _bio.Replace("From Wikipedia, the free encyclopedia", "");
                    return noWiki;
                }
                if (_bio.Contains(wiki4))
                {

                    noWiki = _bio.Replace("From Wikipedia, the free encyclopedia ", "");
                    return noWiki;
                }
                return _bio;

            }
            set { _bio = value; }
        }

        public List<string> BirthPlaceLocations
        {
            get
            {
                
                return _birthPlaceLocations;
            }
            set { _birthPlaceLocations = value; }
        }

        public string BirthPlace
        {
            get
            {
                if (_birthPlaceLocations == null)
                {
                    _birthPlaceLocations = new List<string> {"No Name Available"};
                }
                StringBuilder builder = new StringBuilder();
                foreach (string bp in _birthPlaceLocations) // Loop through all strings
                {
                    builder.Append(bp).Append(","); // Append string to StringBuilder
                }
                string result = builder.ToString(); // Get string from StringBuilder
                return result;

            }
        }

        public DateTime? BornDate
        {
            get { return _bornDate; }
            set
            {
                if (value != null) _bornDate = (DateTime) value;
            }
        }

        public DateTime? DiedDate
        {
            get { return _diedDate; }
            set
            {
                if (value != null) _diedDate = (DateTime) value;
            }
        }

        public string Age
        {
            get
            {
                var today = DateTime.Today;

                var a = (today.Year * 100 + today.Month) * 100 + today.Day;
                var b = (_bornDate.Year * 100 + _bornDate.Month) * 100 + _bornDate.Day;

                return ((a - b) / 10000).ToString();
                
            }
        }

        public string DeathAge
        {
            get
            {
                DateTime dt = _diedDate; //mm/dd/yyyy
                DateTime dt1 = _bornDate;//mm/dd/yyyy
                TimeSpan ts = dt.Subtract(dt1);
                double deathAgeInDays = ts.TotalDays;
                string totalYears = Math.Truncate(deathAgeInDays / 365).ToString();
                return totalYears;

            }
        }

        public ArrayListDataSet Credits
        {
            get
            {
                return this._credits;
            }
            set
            {
                this._credits = value;
            }
        }

        public bool HasLoaded
        {
            get { return _hasLoaded; }
            set { _hasLoaded = value; }
        }

        public ActorItemWrapper Item { get; set; }

        public ActorInfo() { }



        

        
    }
}