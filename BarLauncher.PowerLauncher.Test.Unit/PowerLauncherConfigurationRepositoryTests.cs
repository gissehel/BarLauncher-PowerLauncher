using FluentDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using BarLauncher.PowerLauncher.Lib.Core.Service;
using BarLauncher.PowerLauncher.Lib.DomainModel;
using BarLauncher.PowerLauncher.Lib.Service;
using BarLauncher.PowerLauncher.Test.Mock.Service;
using BarLauncher.EasyHelper.Core.Service;
using BarLauncher.EasyHelper.Test.Mock.Service;
using Xunit;

namespace BarLauncher.PowerLauncher.Test.Unit
{

    [SetContext]
    public class PowerLauncherConfigurationRepositoryTests 
    {
        private ISystemService SystemService { get; set; }
        private IDataAccessPowerLauncherService DataAccessPowerLauncherService { get; set; }

        private IDataAccessService DataAccessService { get; set; }

        private IPowerLauncherConfigurationRepository PowerLauncherConfigurationRepository { get; set; }


        private void SetUp()
        {
            SystemService = new SystemServiceMock
            {
                ApplicationDataPath = Helper.GetTestPath(),
                ApplicationName = "TestDatabase",
            };
            DataAccessPowerLauncherService = new DataAccessPowerLauncherServiceMock(SystemService);
            DataAccessService = DataAccessSQLite.GetService(DataAccessPowerLauncherService);
            PowerLauncherConfigurationRepository = new PowerLauncherConfigurationRepository(DataAccessService);
        }

        private void TearDown()
        {
            if (DataAccessService != null)
            {
                DataAccessService.Dispose();
            }

            PowerLauncherConfigurationRepository = null;
            DataAccessService = null;
            SystemService = null;
        }

        private void Init()
        {
            DataAccessService.Init();
            PowerLauncherConfigurationRepository.Init();
        }

        private class ResultStruct
        {
            public string Data { get; set; }
        }

        private IEnumerable<PowerLauncherConfiguration> GetPowerLauncherConfigurations() => DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration;")
                .Returning<PowerLauncherConfiguration>()
                .Reading("profile", (PowerLauncherConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (PowerLauncherConfiguration configuration, string value) => configuration.PowerLauncherLauncher = value)
                .Reading("pattern", (PowerLauncherConfiguration configuration, string value) => configuration.PowerLauncherArgumentPattern = value)
                .Execute();

        private void EnsureSchema()
        {
            var schema = Helper.GetSchemaForTable(DataAccessService, "configuration");
            Assert.NotNull(schema);
            Assert.Equal("CREATE TABLE configuration (id integer primary key, profile text, launcher text, pattern text)", schema);
        }

        private void CreateOldSchema()
        {
            DataAccessService.GetQuery("create table if not exists configuration (id integer primary key, launcher text, pattern text);").Execute();
        }

        private void CreateNewSchema()
        {
            DataAccessService.GetQuery("create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);").Execute();
        }

        [Fact]
        public void UpgradeFromScratch()
        {
            SetUp();
            Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Empty(configurations);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithoutData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            PowerLauncherConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Empty(configurations);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithoutData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            PowerLauncherConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Empty(configurations);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'launcher', 'args');").Execute();
            PowerLauncherConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Single(configurations);
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().PowerLauncherLauncher);
            Assert.Equal("args", configurations.First().PowerLauncherArgumentPattern);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'default', 'launcher', 'args');").Execute();
            PowerLauncherConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Single(configurations);
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().PowerLauncherLauncher);
            Assert.Equal("args", configurations.First().PowerLauncherArgumentPattern);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithManyData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'launcher', 'args');").Execute();
            DataAccessService.GetQuery("insert into configuration values (2, 'launcher2', 'args2');").Execute();
            DataAccessService.GetQuery("insert into configuration values (3, 'launcher3', 'args2');").Execute();
            PowerLauncherConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Single(configurations);
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().PowerLauncherLauncher);
            Assert.Equal("args", configurations.First().PowerLauncherArgumentPattern);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithManyData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'default', 'launcher', 'args');").Execute();
            DataAccessService.GetQuery("insert into configuration values (2, 'profile2', 'launcher2', 'args2');").Execute();
            DataAccessService.GetQuery("insert into configuration values (3, 'profile3', 'launcher3', 'args3');").Execute();
            PowerLauncherConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetPowerLauncherConfigurations();
            Assert.Equal(3, configurations.Count());
            Assert.Equal("default", configurations.First().Profile);
            Assert.Equal("launcher", configurations.First().PowerLauncherLauncher);
            Assert.Equal("args", configurations.First().PowerLauncherArgumentPattern);
            Assert.Equal("profile2", configurations.ElementAt(1).Profile);
            Assert.Equal("launcher2", configurations.ElementAt(1).PowerLauncherLauncher);
            Assert.Equal("args2", configurations.ElementAt(1).PowerLauncherArgumentPattern);
            Assert.Equal("profile3", configurations.ElementAt(2).Profile);
            Assert.Equal("launcher3", configurations.ElementAt(2).PowerLauncherLauncher);
            Assert.Equal("args3", configurations.ElementAt(2).PowerLauncherArgumentPattern);
            TearDown();
        }
    }
}