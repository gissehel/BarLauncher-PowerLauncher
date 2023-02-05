using FluentDataAccess;
using FluentDataAccess.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public class PowerLauncherItemRepositoryTests
    {
        private ISystemService SystemService { get; set; }
        private IDataAccessPowerLauncherService DataAccessPowerLauncherService { get; set; }

        private IDataAccessService DataAccessService { get; set; }

        private IPowerLauncherItemRepository PowerLauncherItemRepository { get; set; }

        private void SetUp()
        {
            SystemService = new SystemServiceMock
            {
                ApplicationDataPath = Helper.GetTestPath(),
                ApplicationName = "TestDatabase",
            };
            DataAccessPowerLauncherService = new DataAccessPowerLauncherServiceMock(SystemService);
            DataAccessService = DataAccessSQLite.GetService(DataAccessPowerLauncherService);
            PowerLauncherItemRepository = new PowerLauncherItemRepository(DataAccessService);
        }

        private void TearDown()
        {
            if (DataAccessService != null)
            {
                DataAccessService.Dispose();
            }

            PowerLauncherItemRepository = null;
            DataAccessService = null;
            SystemService = null;
        }

        private void Init()
        {
            DataAccessService.Init();
            PowerLauncherItemRepository.Init();
        }

        private IEnumerable<PowerLauncherItem> GetPowerLauncherItems() => DataAccessService
                .GetQuery("select id, url, keywords, profile from powerlauncher_item order by id;")
                .Returning<PowerLauncherItem>()
                .Reading("id", (PowerLauncherItem item, long value) => item.Id = value)
                .Reading("url", (PowerLauncherItem item, string value) => item.Url = value)
                .Reading("keywords", (PowerLauncherItem item, string value) => item.Keywords = value)
                .Reading("profile", (PowerLauncherItem item, string value) => item.Profile = value)
                .Execute();

        private void EnsureSchema()
        {
            var schema = Helper.GetSchemaForTable(DataAccessService, "powerlauncher_item");
            Assert.NotNull(schema);
            Assert.Equal("CREATE TABLE powerlauncher_item (id integer primary key, url text, keywords text, search text, profile text)", schema);
        }

        private void CreateOldSchema()
        {
            DataAccessService.GetQuery("create table if not exists powerlauncher_item (id integer primary key, url text, keywords text, search text);").Execute();
        }
        private void CreateNewSchema()
        {
            DataAccessService.GetQuery("create table if not exists powerlauncher_item (id integer primary key, url text, keywords text, search text, profile text);").Execute();
        }

        [Fact]
        public void UpgradeFromScratch()
        {
            SetUp();
            Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Empty(items);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithoutData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            PowerLauncherItemRepository.Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Empty(items);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithoutData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            PowerLauncherItemRepository.Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Empty(items);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into powerlauncher_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1');").Execute();
            PowerLauncherItemRepository.Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Single(items);
            Assert.Equal(1, items.First().Id);
            Assert.Equal("https://url1.dom/x1", items.First().Url);
            Assert.Equal("keywords1", items.First().Keywords);
            Assert.Equal("default", items.First().Profile);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into powerlauncher_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1', 'mank');").Execute();
            PowerLauncherItemRepository.Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Single(items);
            Assert.Equal(1, items.First().Id);
            Assert.Equal("https://url1.dom/x1", items.First().Url);
            Assert.Equal("keywords1", items.First().Keywords);
            Assert.Equal("mank", items.First().Profile);
            TearDown();
        }

        [Fact]
        public void UpgradeFromOldVersionWithManyData()
        {
            SetUp();
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into powerlauncher_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1');").Execute();
            DataAccessService.GetQuery("insert into powerlauncher_item values (2, 'https://url2.dom/x2', 'keywords2', 'search2');").Execute();
            DataAccessService.GetQuery("insert into powerlauncher_item values (3, 'https://url3.dom/x3', 'keywords3', 'search3');").Execute();
            PowerLauncherItemRepository.Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Equal(3, items.Count());
            Assert.Equal(1, items.First().Id);
            Assert.Equal("https://url1.dom/x1", items.First().Url);
            Assert.Equal("keywords1", items.First().Keywords);
            Assert.Equal("default", items.First().Profile);
            Assert.Equal(2, items.ElementAt(1).Id);
            Assert.Equal("https://url2.dom/x2", items.ElementAt(1).Url);
            Assert.Equal("keywords2", items.ElementAt(1).Keywords);
            Assert.Equal("default", items.ElementAt(1).Profile);
            Assert.Equal(3, items.ElementAt(2).Id);
            Assert.Equal("https://url3.dom/x3", items.ElementAt(2).Url);
            Assert.Equal("keywords3", items.ElementAt(2).Keywords);
            Assert.Equal("default", items.ElementAt(2).Profile);
            TearDown();
        }

        [Fact]
        public void UpgradeFromNewVersionWithManyData()
        {
            SetUp();
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into powerlauncher_item values (1, 'https://url1.dom/x1', 'keywords1', 'search1', 'mank');").Execute();
            DataAccessService.GetQuery("insert into powerlauncher_item values (2, 'https://url2.dom/x2', 'keywords2', 'search2', 'default');").Execute();
            DataAccessService.GetQuery("insert into powerlauncher_item values (3, 'https://url3.dom/x3', 'keywords3', 'search3', 'shon');").Execute();
            PowerLauncherItemRepository.Init();
            EnsureSchema();
            var items = GetPowerLauncherItems();
            Assert.Equal(3, items.Count());
            Assert.Equal(1, items.First().Id);
            Assert.Equal("https://url1.dom/x1", items.First().Url);
            Assert.Equal("keywords1", items.First().Keywords);
            Assert.Equal("mank", items.First().Profile);
            Assert.Equal(2, items.ElementAt(1).Id);
            Assert.Equal("https://url2.dom/x2", items.ElementAt(1).Url);
            Assert.Equal("keywords2", items.ElementAt(1).Keywords);
            Assert.Equal("default", items.ElementAt(1).Profile);
            Assert.Equal(3, items.ElementAt(2).Id);
            Assert.Equal("https://url3.dom/x3", items.ElementAt(2).Url);
            Assert.Equal("keywords3", items.ElementAt(2).Keywords);
            Assert.Equal("shon", items.ElementAt(2).Profile);
            TearDown();
        }
    }
}