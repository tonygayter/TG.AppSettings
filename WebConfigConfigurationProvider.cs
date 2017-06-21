using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using TG.ConfigSettings.Interfaces;

namespace TG.ConfigSettings
{
    /// <summary>
    /// A class that gets config values from the web config.
    /// </summary>
    public class WebConfigConfigurationProvider : IConfigurationProvider
    {
        #region Implementation of IConfigurationProvider

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Returns the configuration value as a T.
        /// </returns>
        public T GetConfigurationValue<T>(string key)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                var value = ConfigurationManager.AppSettings[key];

                try
                {
                    var tc = TypeDescriptor.GetConverter(typeof(T));
                    return (T) tc.ConvertFrom(value);
                }
                catch
                {
                    return default (T);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets the enumerable configuration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IEnumerable<T> GetEnumerableConfigurationValue<T>(string key)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                var valueList = new List<T>();
                string rawValue = ConfigurationManager.AppSettings[key];
                string[] values = rawValue.Split(',');
                foreach (string value in values)
                {
                    try
                    {
                        var tc = TypeDescriptor.GetConverter(typeof(T));
                        valueList.Add((T)tc.ConvertFrom(value));
                    }
                    catch
                    {
                        //TODO: Add any logging code here
                    }
                }

                return valueList;
            }

            return Enumerable.Empty<T>();
        }

        #endregion
    }
}