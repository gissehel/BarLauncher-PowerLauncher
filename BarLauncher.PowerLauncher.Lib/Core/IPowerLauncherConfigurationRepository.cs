using System.Collections.Generic;
using BarLauncher.PowerLauncher.Lib.DomainModel;

namespace BarLauncher.PowerLauncher.Lib.Core.Service
{
    public interface IPowerLauncherConfigurationRepository
    {
        void Init();

        IEnumerable<PowerLauncherConfiguration> GetConfigurations();

        PowerLauncherConfiguration GetConfiguration(string profile);

        void SaveConfiguration(PowerLauncherConfiguration configuration);
    }
}