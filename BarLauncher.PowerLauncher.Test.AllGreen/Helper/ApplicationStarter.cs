using FluentDataAccess;
using System;
using System.IO;
using System.Reflection;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using BarLauncher.PowerLauncher.Lib.Core.Service;
using BarLauncher.PowerLauncher.Lib.Service;
using BarLauncher.PowerLauncher.Test.Mock.Service;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public BarLauncherContextServiceMock BarLauncherContextService { get; set; }
        public QueryServiceMock QueryService { get; set; }
        public SystemServiceMock SystemService { get; set; }
        public DataAccessPowerLauncherServiceMock DataAccessPowerLauncherService { get; set; }
        public IBarLauncherResultFinder BarLauncherPowerLauncherResultFinder { get; set; }
        public IPowerLauncherService PowerLauncherService { get; set; }
        public IHelperService HelperService { get; set; }
        public FileGeneratorServiceMock FileGeneratorService { get; set; }
        public FileReaderServiceMock FileReaderService { get; set; }
        public ApplicationInformationServiceMock ApplicationInformationService { get; set; }
        private string TestName { get; set; }

        private string testPath = null;
        public string TestPath => testPath ?? (testPath = GetApplicationDataPath());

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            BarLauncherContextServiceMock barLauncherContextService = new BarLauncherContextServiceMock(queryService);
            SystemServiceMock systemService = new SystemServiceMock();
            DataAccessPowerLauncherServiceMock dataAccessPowerLauncherService = new DataAccessPowerLauncherServiceMock(systemService);
            IDataAccessService dataAccessService = DataAccessSQLite.GetService(dataAccessPowerLauncherService);
            IPowerLauncherItemRepository webAppItemRepository = new PowerLauncherItemRepository(dataAccessService);
            IPowerLauncherConfigurationRepository webAppConfigurationRepository = new PowerLauncherConfigurationRepository(dataAccessService);
            FileGeneratorServiceMock fileGeneratorService = new FileGeneratorServiceMock();
            FileReaderServiceMock fileReaderService = new FileReaderServiceMock();
            IHelperService helperService = new HelperService();
            ApplicationInformationServiceMock applicationInformationService = new ApplicationInformationServiceMock();
            IPowerLauncherService webAppService = new PowerLauncherService(dataAccessService, webAppItemRepository, webAppConfigurationRepository, systemService, dataAccessPowerLauncherService, fileGeneratorService, fileReaderService, helperService, applicationInformationService);
            IBarLauncherResultFinder barLauncherPowerLauncherResultFinder = new PowerLauncherResultFinder(barLauncherContextService, webAppService, helperService, applicationInformationService, systemService);

            systemService.ApplicationDataPath = TestPath;

            BarLauncherContextService = barLauncherContextService;
            QueryService = queryService;
            SystemService = systemService;
            PowerLauncherService = webAppService;
            FileGeneratorService = fileGeneratorService;
            FileReaderService = fileReaderService;
            BarLauncherPowerLauncherResultFinder = barLauncherPowerLauncherResultFinder;
            HelperService = helperService;
            ApplicationInformationService = applicationInformationService;

            BarLauncherContextService.AddQueryFetcher("wap", BarLauncherPowerLauncherResultFinder);
        }

        public void Start()
        {
            BarLauncherPowerLauncherResultFinder.Init();
        }

        private static string GetThisAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var thisAssemblyCodeBase = assembly.Location;
            var thisAssemblyDirectory = Path.GetDirectoryName(new Uri(thisAssemblyCodeBase).LocalPath);

            return thisAssemblyDirectory;
        }

        private string GetApplicationDataPath()
        {
            var thisAssemblyDirectory = GetThisAssemblyDirectory();
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), string.Format("AG_{0:yyyyMMdd-HHmmss-fff}_{1}", DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}