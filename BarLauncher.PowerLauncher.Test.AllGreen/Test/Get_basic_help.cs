using AllGreen.Lib;
using BarLauncher.PowerLauncher.Test.AllGreen.Fixture;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Test
{
    public class Get_basic_help : TestBase<PowerLauncherContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("pla"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("open [PATTERN] [...]", "Open a tool using its name or keyword")
            .Check("configure [type] [...]", "Configure a resource/exe/tool/...")
            .Check("edit [type] [...]", "Edit a resource/exe/tool/...")
            .Check("remove [type] [...]", "Remove a resource/exe/tool/...")
            .Check("start with [type] [...]", "Start an exe with a resource")
            .Check("export", "Export configuration to a file")
            .Check("import FILENAME", "Import configuration from FILENAME")
            .Check("help", "BarLauncher-PowerLauncher version 0.0 - (Go to BarLauncher-PowerLauncher main web site)")
            .EndUsing()

             .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query(" o"))
            .EndUsing()

             .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("open [PATTERN] [...]", "Open a tool using its name or keyword")
            .Check("configure [type] [...]", "Configure a resource/exe/tool/...")
            .Check("remove [type] [...]", "Remove a resource/exe/tool/...")
            .Check("export", "Export configuration to a file")
            .Check("import FILENAME", "Import configuration from FILENAME")
            .EndUsing()

          .EndTest();
    }
}