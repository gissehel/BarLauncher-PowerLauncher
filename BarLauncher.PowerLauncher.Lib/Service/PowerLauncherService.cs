using FluentDataAccess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.PowerLauncher.Lib.Core.Service;
using BarLauncher.PowerLauncher.Lib.DomainModel;

namespace BarLauncher.PowerLauncher.Lib.Service
{
    public class PowerLauncherService : IPowerLauncherService
    {
        private IDataAccessService DataAccessService { get; set; }

        private IPowerLauncherItemRepository PowerLauncherItemRepository { get; set; }

        private IPowerLauncherConfigurationRepository PowerLauncherConfigurationRepository { get; set; }

        private ISystemService SystemService { get; set; }

        private IDataAccessPowerLauncherService DataAccessPowerLauncherService { get; set; }

        private IFileGeneratorService FileGeneratorService { get; set; }

        private IFileReaderService FileReaderService { get; set; }

        private IHelperService HelperService { get; set; }

        private IApplicationInformationService ApplicationInformationService { get; set; }

        public PowerLauncherService(IDataAccessService dataAccessService, IPowerLauncherItemRepository webAppItemRepository, IPowerLauncherConfigurationRepository webAppConfigurationRepository, ISystemService systemService, IDataAccessPowerLauncherService dataAccessPowerLauncherService, IFileGeneratorService fileGeneratorService, IFileReaderService fileReaderService, IHelperService helperService, IApplicationInformationService applicationInformationService)
        {
            DataAccessService = dataAccessService;
            PowerLauncherItemRepository = webAppItemRepository;
            PowerLauncherConfigurationRepository = webAppConfigurationRepository;
            SystemService = systemService;
            DataAccessPowerLauncherService = dataAccessPowerLauncherService;
            FileGeneratorService = fileGeneratorService;
            FileReaderService = fileReaderService;
            HelperService = helperService;
            ApplicationInformationService = applicationInformationService;
        }

        public void Init()
        {
            DataAccessService.Init();
            PowerLauncherItemRepository.Init();
            PowerLauncherConfigurationRepository.Init();

            var configurations = PowerLauncherConfigurationRepository.GetConfigurations();
            if (configurations == null)
            {
                var configuration = new PowerLauncherConfiguration()
                {
                    Profile = "default",
                    PowerLauncherLauncher = "chrome.exe",
                    PowerLauncherArgumentPattern = "--app=\"{0}\" --profile-directory=\"Default\""
                };
                PowerLauncherConfigurationRepository.SaveConfiguration(configuration);
            }
        }

        public void AddPowerLauncherItem(string url, string keywords, string profile)
        {
            var item = new PowerLauncherItem
            {
                Url = url,
                Keywords = keywords,
                Profile = profile,
            };
            PowerLauncherItemRepository.AddItem(item);
        }

        public IEnumerable<PowerLauncherItem> Search(IEnumerable<string> terms)
        {
            return PowerLauncherItemRepository.SearchItems(terms);
        }

        public void UpdateLauncher(string launcher, string argumentPattern, string profile)
        {
            var configuration = PowerLauncherConfigurationRepository.GetConfiguration(profile);
            if (configuration == null)
            {
                configuration = new PowerLauncherConfiguration() 
                { 
                    Profile = profile,
                };
            }
            configuration.PowerLauncherLauncher = launcher;
            configuration.PowerLauncherArgumentPattern = argumentPattern;

            PowerLauncherConfigurationRepository.SaveConfiguration(configuration);
        }

        public void StartUrl(string url, string profile)
        {
            var configuration = PowerLauncherConfigurationRepository.GetConfiguration(profile);
            if (configuration != null)
            {
                var launcher = configuration.PowerLauncherLauncher;
                var argumentsPattern = configuration.PowerLauncherArgumentPattern;

                var arguments = string.Format(argumentsPattern, url);
                SystemService.StartCommandLine(launcher, arguments);
            }
        }

        public PowerLauncherConfiguration GetConfiguration(string profile) => PowerLauncherConfigurationRepository.GetConfiguration(profile);
        public PowerLauncherConfiguration GetOrCreateConfiguration(string profile)
        {
            var configuration = PowerLauncherConfigurationRepository.GetConfiguration(profile);
            if (configuration == null)
            {
                configuration = new PowerLauncherConfiguration()
                {
                    Profile = profile,
                    PowerLauncherLauncher = "chrome.exe",
                    PowerLauncherArgumentPattern = "--app=\"{0}\" --profile-directory=\"Default\""
                };
                PowerLauncherConfigurationRepository.SaveConfiguration(configuration);
            }
            return PowerLauncherConfigurationRepository.GetConfiguration(profile);
        }

        public IEnumerable<string> GetProfiles() =>
            PowerLauncherConfigurationRepository
            .GetConfigurations()
            .Select((configuration) => configuration.Profile);

        public Dictionary<string, PowerLauncherConfiguration> GetConfigurations() =>
            PowerLauncherConfigurationRepository
            .GetConfigurations()
            .GroupBy((PowerLauncherConfiguration webAppConfiguration) => webAppConfiguration.Profile)
            .ToDictionary(g => g.Key, g => g.ToList().First());

        public void RemoveUrl(string url) => PowerLauncherItemRepository.RemoveItem(url);

        public void Export()
        {
            var exportDirectory = DataAccessPowerLauncherService.GetExportPath();
            var exportFilename = string.Format("{0}-Save-{1}.wap.txt", ApplicationInformationService.ApplicationName, DataAccessPowerLauncherService.GetUID());
            var exportFullFilename = Path.Combine(exportDirectory, exportFilename);
            using (var fileGenerator = FileGeneratorService.CreateGenerator(exportFullFilename))
            {
                var configurations = GetConfigurations();
                if (configurations.ContainsKey("default"))
                {
                    var configuration = configurations["default"];
                    fileGenerator.AddLine(string.Format("# launcher: {0}", configuration.PowerLauncherLauncher));
                    fileGenerator.AddLine(string.Format("# argumentsPattern: {0}", configuration.PowerLauncherArgumentPattern));
                }
                foreach (var configuration in configurations.Values)
                {
                    if (configuration.Profile != "default")
                    {
                        fileGenerator.AddLine(string.Format("# launcher[{1}]: {0}", configuration.PowerLauncherLauncher, configuration.Profile));
                        fileGenerator.AddLine(string.Format("# argumentsPattern[{1}]: {0}", configuration.PowerLauncherArgumentPattern, configuration.Profile));
                    }
                }
                foreach (var webAppItem in PowerLauncherItemRepository.SearchItems(new List<string>()))
                {
                    if (webAppItem.Profile == "default")
                    {
                        fileGenerator.AddLine(string.Format("{0} ({1})", webAppItem.Url, webAppItem.Keywords));
                    }
                    else
                    {
                        fileGenerator.AddLine(string.Format("{0} ({1}) [{2}]", webAppItem.Url, webAppItem.Keywords, webAppItem.Profile));
                    }
                    
                }
                fileGenerator.Generate();
            }
            SystemService.OpenDirectory(exportDirectory);
        }

        public bool FileExists(string path) => FileReaderService.FileExists(path);

        public void Import(string path)
        {
            using (var fileReader = FileReaderService.Read(path))
            {
                var line = fileReader.ReadLine();
                while (line != null)
                {
                    line = line.Trim(' ', '\t', '\r', '\n');
                    if (line.StartsWith("#"))
                    {
                        if (line.Contains(":"))
                        {
                            var indexOfSeperater = line.IndexOf(":");
                            var key = line.Substring(0, indexOfSeperater).Trim(' ', '\t', '\r', '\n');
                            var value = line.Substring(indexOfSeperater + 1, line.Length - indexOfSeperater - 1).Trim(' ', '\t', '\r', '\n');
                            string profile = null;
                            HelperService.ExtractProfile(key, ref key, ref profile);
                            key = key.TrimStart('#').Trim(' ', '\t', '\r', '\n');

                            var configuration = GetOrCreateConfiguration(profile);
                            var changed = false;
                            if (key == "launcher")
                            {
                                configuration.PowerLauncherLauncher = value;
                                changed = true;
                            }
                            if (key == "argumentsPattern")
                            {
                                configuration.PowerLauncherArgumentPattern = value;
                                changed = true;
                            }
                            if (changed)
                            {
                                PowerLauncherConfigurationRepository.SaveConfiguration(configuration);
                            }
                        }
                    }
                    else
                    {
                        var elements = line.Split(' ');
                        var url = elements[0];
                        var keywords = string.Join(" ", elements.Skip(1).Where(e => e.Length > 0).ToArray());
                        keywords = keywords.TrimStart('(', ' ', '\t', '\r', '\n').TrimEnd(')', ' ', '\t', '\r', '\n');
                        string profile = null;
                        if (HelperService.ExtractProfile(keywords, ref keywords, ref profile))
                        {
                            keywords = keywords.TrimStart('(', ' ', '\t', '\r', '\n').TrimEnd(')', ' ', '\t', '\r', '\n');
                        }

                        if (!string.IsNullOrEmpty(url))
                        {
                            AddPowerLauncherItem(url, keywords, profile);
                        }
                    }
                    line = fileReader.ReadLine();
                }
            }
        }

        public void Dispose()
        {
            DataAccessService.Dispose();
        }

        public PowerLauncherItem GetUrlInfo(string url)
        {
            return PowerLauncherItemRepository.GetItem(url);
        }

        public void EditPowerLauncherItem(string url, string newUrl, string newKeywords, string newProfile)
        {
            PowerLauncherItemRepository.EditPowerLauncherItem(url, newUrl, newKeywords, newProfile);
        }
    }
}