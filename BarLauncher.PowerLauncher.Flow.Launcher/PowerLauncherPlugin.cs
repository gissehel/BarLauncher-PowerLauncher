using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.EasyHelper.Flow.Launcher;
using BarLauncher.PowerLauncher.Lib.Service;
using FluentDataAccess;
using BarLauncher.EasyHelper.Flow.Launcher.Service;

namespace BarLauncher.PowerLauncher.Flow.Launcher
{
    public class PowerLauncherPlugin : FlowLauncherPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var systemService = new FlowLauncherSystemService("BarLauncher-PowerLauncher", BarLauncherContextService as BarLauncherContextService);
            var dataAccessPowerLauncherService = new DataAccessPowerLauncherService(systemService, "Wox.PowerLauncher");
            var dataAccessService = DataAccessSQLite.GetService(dataAccessPowerLauncherService);
            var helperService = new HelperService();
            var PowerLauncherConfigurationRepository = new PowerLauncherConfigurationRepository(dataAccessService);
            var PowerLauncherItemRepository = new PowerLauncherItemRepository(dataAccessService);
            var fileGeneratorService = new FileGeneratorService();
            var fileReaderService = new FileReaderService();
            var applicationInformationService = new ApplicationInformationService(systemService);
            var PowerLauncherService = new PowerLauncherService(dataAccessService, PowerLauncherItemRepository, PowerLauncherConfigurationRepository, systemService, dataAccessPowerLauncherService, fileGeneratorService, fileReaderService, helperService, applicationInformationService);
            return new PowerLauncherResultFinder(BarLauncherContextService, PowerLauncherService, helperService, applicationInformationService, systemService);
        }
    }
}
