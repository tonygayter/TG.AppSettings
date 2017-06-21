//  -----------------------------------------------------------------------
//   Project: 0HAN1963
//   Author: Sam Mullins
//   ----------------------------------------------------------------------
//   <copyright file="ApplicationSettingsConfigConfigurationProvider.cs" company="Redweb Ltd.">
//     Copyright © Redweb Ltd.
//   </copyright>
//   <license>
//     This source code file is owned by Redweb Ltd.  All rights reserved.
//   </license>
//  -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using TG.ConfigSettings.Interfaces;

namespace TG.ConfigSettings
{
    public class ApplicationSettingsConfigConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        ///     The in memory cache
        /// </summary>
        protected MemoryCache Cache = MemoryCache.Default;

        private const string CacheKey = "ApplicationSettingsConfigConfigurationProvider_";

        /// <summary>
        ///     The section name defined in Web.config
        /// </summary>
        private const string SectionName = "appSettings";

        #region Implementation of IConfigurationProvider

        /// <summary>
        ///     Gets the configuration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     Returns the configuration value as a T.
        /// </returns>
        public T GetConfigurationValue<T>(string key)
        {
            string cacheKey = CacheKey + key;
            object item = Cache[cacheKey];

            if(item == null)
            {
                //Returns null
                var settingsCollection = (NameValueCollection)ConfigurationManager.GetSection(SectionName);

                if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(settingsCollection[key]))
                {
                    string value = settingsCollection[key];

                    try
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                        item = (T)tc.ConvertFrom(value);

                        var policy = new CacheItemPolicy
                        {
                            SlidingExpiration = new TimeSpan(365, 0, 0, 0)
                        };
                        Cache.Set(cacheKey, item, policy);
                        return (T)item;
                    }
                    catch
                    {
                        return default(T);
                    }
                }

                return default(T);
            }

            return (T)item;
        }

        /// <summary>
        ///     Gets the enumerable configuration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IEnumerable<T> GetEnumerableConfigurationValue<T>(string key)
        {
            string cacheKey = CacheKey + key;
            object item = Cache[cacheKey];

            if(item == null)
            {
                var settingsCollection = (NameValueCollection)ConfigurationManager.GetSection(SectionName);

                if(!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(settingsCollection[key]))
                {
                    var valueList = new List<T>();
                    string rawValue = settingsCollection[key];
                    string[] values = rawValue.Split(',');
                    foreach(string value in values)
                    {
                        try
                        {
                            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                            valueList.Add((T)tc.ConvertFrom(value));
                        }
                        catch
                        {
                            return Enumerable.Empty<T>();
                        }
                    }

                    var policy = new CacheItemPolicy
                    {
                        SlidingExpiration = new TimeSpan(365, 0, 0, 0)
                    };
                    Cache.Set(cacheKey, valueList, policy);
                    return valueList;
                }
            }
            else
            {
                return (IEnumerable<T>)item;
            }

            return Enumerable.Empty<T>();
        }

        #endregion
    }
}