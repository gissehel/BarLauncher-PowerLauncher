﻿using System;
using System.IO;
using System.Linq;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.PowerLauncher.Lib.Core.Service;

namespace BarLauncher.PowerLauncher.Lib.Service
{
    public class DataAccessPowerLauncherService : IDataAccessPowerLauncherService
    {
        private ISystemService SystemService { get; set; }

        public DataAccessPowerLauncherService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        private string GetDatabaseName(string applicationDataPath, string applicationName) => Path.Combine(applicationDataPath, applicationName + ".sqlite");

        public DataAccessPowerLauncherService(ISystemService systemService, params string[] oldApplicationNames) : this(systemService)
        {
            var currentDatabaseName = GetDatabaseName(ApplicationDataPath, ApplicationName);

            if (!File.Exists(currentDatabaseName))
            {
                foreach (var oldApplicationName in oldApplicationNames.AsEnumerable().Reverse())
                {
                    string oldDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), oldApplicationName);

                    var oldDatabaseName = GetDatabaseName(oldDataPath, oldApplicationName);
                    if (File.Exists(oldDatabaseName))
                    {
                        File.Move(oldDatabaseName, currentDatabaseName);
                        return;
                    }
                }
            }
        }

        private string ApplicationDataPath => SystemService.ApplicationDataPath;

        private string ApplicationName => SystemService.ApplicationName;

        public string DatabasePath => ApplicationDataPath;

        public string DatabaseName => ApplicationName;

        public string GetExportPath() => ApplicationDataPath;

        public string GetUID() => string.Format("{0:yyyyMMdd-HHmmss-fff}", DateTime.Now);
    }
}