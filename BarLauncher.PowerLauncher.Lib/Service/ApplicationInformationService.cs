using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.PowerLauncher.Lib.Core.Service;

namespace BarLauncher.PowerLauncher.Lib.Service
{
    public class ApplicationInformationService : IApplicationInformationService
    {
        private ISystemService SystemService { get; set; }

        public ApplicationInformationService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        public string ApplicationName => SystemService.ApplicationName;

        public string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string HomepageUrl => "https://github.com/gissehel/BarLauncher-PowerLauncher";
    }
}
