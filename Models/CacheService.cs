using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrangeGame.Models
{
    public class InMemoryCache : ICacheService
    {
        public T Get<T>(string cacheID, Func<T> getItemCallback) where T : class
        {
            T item = HttpRuntime.Cache.Get(cacheID) as T;
            if (item == null)
            {
                item = getItemCallback();
                HttpContext.Current.Cache.Insert(cacheID, item);
            }
            return item;
        }

        public object Remove(string cacheID)
        {
            return HttpContext.Current.Cache.Remove(cacheID);
        }
    }

    interface ICacheService
    {
        T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;
    }

    //usage  cacheProvider.Get("cache id", (delegate method if cache is empty));
}