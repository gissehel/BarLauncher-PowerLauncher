using AllGreen.Lib;
using System.Collections.Generic;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Fixture
{
    public class Url_opened_fixture : FixtureBase<PowerLauncherContext>
    {
        public override IEnumerable<object> OnQuery()
        {
            foreach (var url in Context.ApplicationStarter.SystemService.UrlOpened)
            {
                yield return new Result { Url = url };
            }
        }

        public class Result
        {
            public string Url { get; set; }
        }
    }
}