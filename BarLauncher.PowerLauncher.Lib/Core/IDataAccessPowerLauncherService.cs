using BarLauncher.EasyHelper.Core.Service;
using FluentDataAccess;

namespace BarLauncher.PowerLauncher.Lib.Core.Service
{
    public interface IDataAccessPowerLauncherService : IDataAccessConfigurationByPath
    {
        string GetExportPath();

        string GetUID();
    }
}