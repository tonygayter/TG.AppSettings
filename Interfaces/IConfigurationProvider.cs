

using System.Collections.Generic;

namespace TG.ConfigSettings.Interfaces
{
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Returns the configuration value as a T.
        /// </returns>
        T GetConfigurationValue<T>(string key);

        /// <summary>
        /// Gets the enumerable configuration value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IEnumerable<T> GetEnumerableConfigurationValue<T>(string key);
    }
}