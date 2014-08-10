using System.Collections.Generic;
using MediaBrowser.Library;

namespace Xenon
{
    public class Cache  
    {
        private Dictionary<string, List<Item>> _cache = new Dictionary<string, List<Item>>();

        public List<Item> GetCacheItems(string key)
        {
            if (this._cache.ContainsKey(key))
            {
                return this._cache[key];
            }
            return null;
        }

        public void PersistCacheItems(string key, List<Item> items)
        {
            if (!this._cache.ContainsKey(key))
            {
                this._cache.Add(key, items);
            }
            else
            {
                this._cache[key] = items;
            }
        }
    }
}