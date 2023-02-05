using FluentDataAccess;
using System.Collections.Generic;
using System.Linq;
using BarLauncher.PowerLauncher.Lib.Core.Service;
using BarLauncher.PowerLauncher.Lib.DomainModel;

namespace BarLauncher.PowerLauncher.Lib.Service
{
    public class PowerLauncherConfigurationRepository : IPowerLauncherConfigurationRepository
    {
        private IDataAccessService DataAccessService { get; set; }

        public PowerLauncherConfigurationRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        private void UpgradeForProfile()
        {
            try
            {
                DataAccessService.GetQuery("select launcher from configuration").Execute();
                try
                {
                    DataAccessService.GetQuery("select profile from configuration").Execute();
                }
                catch (System.Exception)
                {
                    DataAccessService
                        .GetQuery(
                            "create temp table configuration_update (id integer primary key, profile text, launcher text, pattern text);" +
                            "insert into configuration_update (id, profile, launcher , pattern ) select id, 'default', launcher, pattern from configuration order by id limit 1;" +
                            "drop table configuration;" +
                            "create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);" +
                            "insert into configuration (id, profile, launcher , pattern ) select id, profile, launcher, pattern from configuration_update order by id;" +
                            "drop table configuration_update;"
                        )
                        .Execute();
                }
            }
            catch (System.Exception)
            {
                // No updagre needed
            }

        }

        public void Init()
        {
            UpgradeForProfile();
            DataAccessService
                .GetQuery("create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);")
                .Execute();
        }
        private string GetProfile(string profile) => profile ?? "default";

        public IEnumerable<PowerLauncherConfiguration> GetConfigurations()
        {
            var configurations = DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration;")
                .Returning<PowerLauncherConfiguration>()
                .Reading("profile", (PowerLauncherConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (PowerLauncherConfiguration configuration, string value) => configuration.PowerLauncherLauncher = value)
                .Reading("pattern", (PowerLauncherConfiguration configuration, string value) => configuration.PowerLauncherArgumentPattern = value)
                .Execute();
            if (configurations.Any())
            {
                return configurations;
            }
            return null;
        }

        public PowerLauncherConfiguration GetConfiguration(string profile)
        {
            var configurations = DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration where profile=@profile;")
                .WithParameter("profile", GetProfile(profile))
                .Returning<PowerLauncherConfiguration>()
                .Reading("profile", (PowerLauncherConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (PowerLauncherConfiguration configuration, string value) => configuration.PowerLauncherLauncher = value)
                .Reading("pattern", (PowerLauncherConfiguration configuration, string value) => configuration.PowerLauncherArgumentPattern = value)
                .Execute();
            return configurations.FirstOrDefault();
        }

        public void SaveConfiguration(PowerLauncherConfiguration configuration)
        {
            var affectedRows = DataAccessService
                .GetQuery("update configuration set launcher=@launcher, pattern=@pattern where profile=@profile")
                .WithParameter("profile", configuration.Profile)
                .WithParameter("launcher", configuration.PowerLauncherLauncher)
                .WithParameter("pattern", configuration.PowerLauncherArgumentPattern)
                .Execute();
            if (affectedRows == 0)
            {
                DataAccessService
                    .GetQuery("insert into configuration (profile, launcher, pattern) values (@profile, @launcher, @pattern)")
                    .WithParameter("profile", configuration.Profile)
                    .WithParameter("launcher", configuration.PowerLauncherLauncher)
                    .WithParameter("pattern", configuration.PowerLauncherArgumentPattern)
                    .Execute();
            }
        }
    }
}