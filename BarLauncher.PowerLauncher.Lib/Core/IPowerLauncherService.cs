using System;
using System.Collections.Generic;
using BarLauncher.PowerLauncher.Lib.DomainModel;

namespace BarLauncher.PowerLauncher.Lib.Core.Service
{
    public interface IPowerLauncherService : IDisposable
    {
        /// <summary>
        /// Initialize the instance of the service
        /// </summary>
        void Init();

        /// <summary>
        /// Search a powerlauncher given terms
        /// </summary>
        /// <param name="terms">The terms to search</param>
        /// <returns>An enumerable of PowerLauncherItem</returns>
        IEnumerable<PowerLauncherItem> Search(IEnumerable<string> terms);

        /// <summary>
        /// Add a new PowerLauncherItem using an url and keywords
        /// </summary>
        /// <param name="url">The url of the web app</param>
        /// <param name="keywords">The keywords used for search</param>
        /// <param name="profile">The profile to use</param>
        void AddPowerLauncherItem(string url, string keywords, string profile);

        /// <summary>
        /// Update the application to use to browse the powerlauncher
        /// </summary>
        /// <param name="launcher">The executable to use</param>
        /// <param name="argumentPattern">The command line arguments to pass (used a template)</param>
        /// <param name="profile">The profile to use</param>
        void UpdateLauncher(string launcher, string argumentPattern, string profile);

        /// <summary>
        /// Start a web app given it's url and it's profile
        /// </summary>
        /// <param name="url">The url of the web app to start</param>
        /// <param name="profile">The profile to use</param>
        void StartUrl(string url, string profile);

        /// <summary>
        /// Get the configuration for a profile
        /// </summary>
        /// <param name="profile">The profile to use</param>
        /// <returns>A PowerLauncherConfigration instance</returns>
        PowerLauncherConfiguration GetConfiguration(string profile);

        /// <summary>
        /// Get or create the configuration for a profile
        /// </summary>
        /// <param name="profile">The profile to use</param>
        /// <returns>A PowerLauncherConfigration instance</returns>
        PowerLauncherConfiguration GetOrCreateConfiguration(string profile);

        /// <summary>
        /// Get the list of all available profiles
        /// </summary>
        /// <returns>A list of profile names</returns>
        IEnumerable<string> GetProfiles();

        /// <summary>
        /// Remove an existing web app given it's url.
        /// </summary>
        /// <param name="url">The url of the web app to remove</param>
        void RemoveUrl(string url);

        /// <summary>
        /// Export the current configration to a file
        /// </summary>
        void Export();

        /// <summary>
        /// Returns true if a file exists
        /// </summary>
        /// <param name="path">The file name</param>
        /// <returns></returns>
        bool FileExists(string path);

        /// <summary>
        /// Import a configuration previously exported
        /// </summary>
        /// <param name="path">The file name of the configuration file to export</param>
        void Import(string path);

        /// <summary>
        /// Get information on an url, or null if no URL match the criteria
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The informations about the url</returns>
        PowerLauncherItem GetUrlInfo(string url);

        /// <summary>
        /// Edit informations about a web app.
        /// </summary>
        /// <param name="url">The url of the current powerlauncher item</param>
        /// <param name="newUrl">The new url for the powerlauncher item</param>
        /// <param name="newKeywords">The new keywords to use</param>
        /// <param name="newProfile">The new profile to use</param>
        void EditPowerLauncherItem(string url, string newUrl, string newKeywords, string newProfile);
    }
}