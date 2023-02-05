using AllGreen.Lib;
using System.Collections.Generic;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Fixture
{
    public class BarLauncher_results_fixture : FixtureBase<PowerLauncherContext>
    {
        public class Result
        {
            public string Title { get; set; }

            public string SubTitle { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            foreach (var result in Context.ApplicationStarter.BarLauncherContextService.Results)
            {
                yield return new Result
                {
                    Title = result.Title,
                    SubTitle = result.SubTitle,
                };
            }
        }
    }
}