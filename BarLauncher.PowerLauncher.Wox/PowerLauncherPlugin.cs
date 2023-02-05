using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Service;
using BarLauncher.EasyHelper.Wox;
using BarLauncher.PowerLauncher.Lib.Service;
using FluentDataAccess;

namespace BarLauncher.PowerLauncher.Wox
{
    public class PowerLauncherPlugin : WoxPlugin
    {
        public override IBarLauncherResultFinder PrepareContext()
        {
            var systemService = new SystemService("BarLauncher-PowerLauncher");
            var dataAccessPowerLauncherService = new DataAccessPowerLauncherService(systemService, "Wox.PowerLauncher");
            var dataAccessService = DataAccessSQLite.GetService(dataAccessPowerLauncherService);
            var helperService = new HelperService();
            var webAppConfigurationRepository = new PowerLauncherConfigurationRepository(dataAccessService);
            var webAppItemRepository = new PowerLauncherItemRepository(dataAccessService);
            var fileGeneratorService = new FileGeneratorService();
            var fileReaderService = new FileReaderService();
            var applicationInformationService = new ApplicationInformationService(systemService);
            var webAppService = new PowerLauncherService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemService, dataAccessPowerLauncherService, fileGeneratorService, fileReaderService, helperService, applicationInformationService);
            return new PowerLauncherResultFinder(BarLauncherContextService, webAppService, helperService, applicationInformationService, systemService);
        }
    }
}
