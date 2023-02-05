using AllGreen.Lib;
using System.Collections.Generic;
using System.Linq;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Fixture
{
    public class Command_line_started_fixture : FixtureBase<PowerLauncherContext>
    {
        public class Result
        {
            public string Command { get; set; }
            public string Arguments { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            var result = Context
                .ApplicationStarter
                .SystemService
                .CommandLineStarted
                .Select(commandLine => new Result
                {
                    Command = commandLine.Command,
                    Arguments = commandLine.Arguments
                }).ToList();
            return result.Cast<object>();
        }
    }
}