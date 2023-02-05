using FluentDataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarLauncher.PowerLauncher.Lib.Core.Service;
using BarLauncher.PowerLauncher.Lib.DomainModel;

namespace BarLauncher.PowerLauncher.Lib.Service
{
    public class PowerLauncherItemRepository : IPowerLauncherItemRepository
    {
        private IDataAccessService DataAccessService { get; set; }

        public PowerLauncherItemRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        private void UpgradeForProfile()
        {
            try
            {
                DataAccessService.GetQuery("select id from powerlauncher_item").Execute();
                try
                {
                    DataAccessService.GetQuery("select profile from powerlauncher_item").Execute();
                }
                catch (System.Exception)
                {
                    DataAccessService
                        .GetQuery(
                            "create temp table powerlauncher_item_update (id integer primary key, url text, keywords text, search text, profile text);" +
                            "insert into powerlauncher_item_update (id, url, keywords, search, profile) select id, url, keywords, search, 'default' from powerlauncher_item order by id;" +
                            "drop table powerlauncher_item;" +
                            "create table if not exists powerlauncher_item (id integer primary key, url text, keywords text, search text, profile text);" +
                            "insert into powerlauncher_item (id, url, keywords, search, profile) select id, url, keywords, search, profile from powerlauncher_item_update order by id;" +
                            "drop table powerlauncher_item_update;"
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
                .GetQuery("create table if not exists powerlauncher_item (id integer primary key, url text, keywords text, search text, profile text)")
                .Execute();
            DataAccessService
                .GetQuery("create unique index if not exists powerlauncher_item_url on powerlauncher_item (url)")
                .Execute();
            // BUGFIX : If older version had generated a null field for keywords, replace it by an empty string to prevent bugs.
            DataAccessService
                .GetQuery("update powerlauncher_item set keywords='' where keywords is null")
                .Execute();
        }

        private string GetSearchField(string url, string keywords) => string.Format("{0} {1}", url, keywords).ToLower();

        private string NormalizeKeywords(string keywords) => keywords != null ? keywords : "";

        private string GetProfile(string profile) => profile ?? "default";

        public void AddItem(PowerLauncherItem item)
        {
            DataAccessService
                .GetQuery("insert or replace into powerlauncher_item (url, keywords, search, profile) values (@url, @keywords, @search, @profile)")
                .WithParameter("url", item.Url)
                .WithParameter("keywords", NormalizeKeywords(item.Keywords))
                .WithParameter("search", GetSearchField(item.Url, item.Keywords))
                .WithParameter("profile", GetProfile(item.Profile))
                .Execute();
        }

        public void RemoveItem(string url)
        {
            DataAccessService
                .GetQuery("delete from powerlauncher_item where url=@url")
                .WithParameter("url", url)
                .Execute();
        }

        public IEnumerable<PowerLauncherItem> SearchItems(IEnumerable<string> terms)
        {
            var builder = new StringBuilder("select id, url, keywords, profile from powerlauncher_item ");
            int index = 0;
            foreach (var term in terms)
            {
                if (index == 0)
                {
                    builder.Append("where ");
                }
                else
                {
                    builder.Append("and ");
                }
                builder.Append("search like @param");
                builder.Append(index.ToString());
                builder.Append(" ");
                index++;
            }
            builder.Append("order by id");
            var dataAccessQuery = DataAccessService.GetQuery(builder.ToString());
            index = 0;
            foreach (var term in terms)
            {
                var parameterName = string.Format("param{0}", index);
                var parameterValue = string.Format("%{0}%", term.ToLower());
                dataAccessQuery = dataAccessQuery.WithParameter(parameterName, parameterValue);
                index++;
            }
            return dataAccessQuery
                .Returning<PowerLauncherItem>()
                .Reading("id", (PowerLauncherItem item, long value) => item.Id = value)
                .Reading("url", (PowerLauncherItem item, string value) => item.Url = value)
                .Reading("keywords", (PowerLauncherItem item, string value) => item.Keywords = value)
                .Reading("profile", (PowerLauncherItem item, string value) => item.Profile = value)
                .Execute()
                ;
        }

        public PowerLauncherItem GetItem(string url)
        {
            var query = "select id, url, keywords, profile from powerlauncher_item where url=@url order by id";
            var results = DataAccessService
                .GetQuery(query)
                .WithParameter("url", url)
                .Returning<PowerLauncherItem>()
                .Reading("id", (PowerLauncherItem item, long value) => item.Id = value)
                .Reading("url", (PowerLauncherItem item, string value) => item.Url = value)
                .Reading("keywords", (PowerLauncherItem item, string value) => item.Keywords = value)
                .Reading("profile", (PowerLauncherItem item, string value) => item.Profile = value)
                .Execute()
                ;
            try
            {
                return results.First();
            }
            catch
            {
                return null;
            }

        }

        public void EditPowerLauncherItem(string url, string newUrl, string newKeywords, string newProfile)
        {
            var query = "update powerlauncher_item set url=@url, keywords=@keywords, search=@search, profile=@profile where url=@oldurl";
            DataAccessService
                .GetQuery(query)
                .WithParameter("oldurl", url)
                .WithParameter("url", newUrl)
                .WithParameter("keywords", NormalizeKeywords(newKeywords))
                .WithParameter("search", GetSearchField(newUrl, newKeywords))
                .WithParameter("profile", GetProfile(newProfile))
                .Execute()
            ;
        }
    }
}