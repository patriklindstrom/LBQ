using System.Collections.Generic;

namespace LBQ.Katana.Repo
{
    public interface ICacheLayer
    {
        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        void Add<T>(T objectToCache, string key) where T : class;

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        void Add(object objectToCache, string key);

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        void Clear(string key);

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// Gets all cached items as a list by their key.
        /// </summary>
        /// <returns></returns>
        List<string> GetAll();
    }
}