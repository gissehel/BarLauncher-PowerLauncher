using BarLauncher.PowerLauncher.Lib.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using System.IO;
using BarLauncher.EasyHelper.Core.Service;

namespace BarLauncher.PowerLauncher.Test.Mock.Service
{
    public class DataAccessPowerLauncherServiceMock : IDataAccessPowerLauncherService
    {
        ISystemService SystemService { get; set; }
        public DataAccessPowerLauncherServiceMock(ISystemService systemService)
        {
            SystemService = systemService;
        }
        public string DatabaseName => SystemService.ApplicationName;

        public string DatabasePath => SystemService.ApplicationDataPath;

        public string GetExportPath() => @".\ExportDirectory";

        public string GetUID() => "UID";
    }
}