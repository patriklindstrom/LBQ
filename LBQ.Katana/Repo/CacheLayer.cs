using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Nancy.TinyIoc;
namespace LBQ.Katana.Repo
{
    // CacheLayer taken from Article http://deanhume.com/Home/BlogPost/object-caching----net-4/37
    // TODO: Rewrite with no static methods and singleton instead.
        public class CacheLayer : ICacheLayer
        {
            readonly ObjectCache _cache = MemoryCache.Default;
            
            private readonly int _minutesToRefreshCache;
            public CacheLayer(ISettingsProvider settings, ISettingsProvider mySettingsProvider)
            {               
                _minutesToRefreshCache = mySettingsProvider.GetCacheExpireInMin();
            }

            /// <summary>
            /// Retrieve cached item
            /// </summary>
            /// <typeparam name="T">Type of cached item</typeparam>
            /// <param name="key">Name of cached item</param>
            /// <returns>Cached item as type</returns>
            public  T Get<T>(string key) where T : class
            {
                try
                {
                    return (T)_cache[key];
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// Insert value into the cache using
            /// appropriate name/value pairs
            /// </summary>
            /// <typeparam name="T">Type of cached item</typeparam>
            /// <param name="objectToCache">Item to be cached</param>
            /// <param name="key">Name of item</param>
            public  void Add<T>(T objectToCache, string key) where T : class
            {
                _cache.Add(key, objectToCache, DateTime.Now.AddMinutes(_minutesToRefreshCache));
            }

            /// <summary>
            /// Insert value into the cache using
            /// appropriate name/value pairs
            /// </summary>
            /// <param name="objectToCache">Item to be cached</param>
            /// <param name="key">Name of item</param>
            public  void Add(object objectToCache, string key)
            {
                _cache.Add(key, objectToCache, DateTime.Now.AddMinutes(_minutesToRefreshCache));
            }

            /// <summary>
            /// Remove item from cache
            /// </summary>
            /// <param name="key">Name of cached item</param>
            public  void Clear(string key)
            {
                _cache.Remove(key);
            }

            /// <summary>
            /// Check for item in cache
            /// </summary>
            /// <param name="key">Name of cached item</param>
            /// <returns></returns>
            public  bool Exists(string key)
            {
                return _cache.Get(key) != null;
            }

            /// <summary>
            /// Gets all cached items as a list by their key.
            /// </summary>
            /// <returns></returns>
            public  List<string> GetAll()
            {
                return _cache.Select(keyValuePair => keyValuePair.Key).ToList();
            }
        }
    }


