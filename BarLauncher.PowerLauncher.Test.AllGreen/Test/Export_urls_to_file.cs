using AllGreen.Lib;
using BarLauncher.PowerLauncher.Test.AllGreen.Fixture;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Test
{
    public class Export_urls_to_file : TestBase<PowerLauncherContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("pla exp"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "export")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Export configuration to a file")
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(f => f.Command, f => f.Arguments)
            .Check(@".\ExportDirectory", "")
            .EndUsing()

            .Using<Last_file_generated_fixture>()
            .DoCheck(f => f.The_filename_is(), @".\ExportDirectory\BarLauncher-PowerLauncher-Save-UID.txt")
            .EndUsing()

            .UsingList<Last_file_generated_fixture>()
            .With<Last_file_generated_fixture.Result>(f => f.Line)
            .Check("configure exe with name ( edge ) with path ( c:\\Program Files(x86)\\Microsoft\\Edge\\Application\\msedge.exe ) with keywords ( browser web )")
            .Check("configure exe with name ( irfanview ) with path ( c:\\Program Files\\IrfanView\\i_view64.exe ) with keywords ( image viewer )")
            .Check("configure exe with name ( notepad ) with path ( c:\\Windows\\System32\\notepad.exe ) with keywords ( editor text )")
            .Check("configure exe with name ( totalcommander ) with path ( c:\\Program Files\\TOTALCMD64.EXE ) with keywords ( file manager )")
            .Check("configure exe with name ( vscode ) with path ( c:\\Users\\Grut\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe ) with keywords ( editor text ide )")
            .Check("configure resource with name ( hosts ) with path ( c:\\Windows\\System32\\drivers\\etc\\hosts ) with keywords ( dns ip )")
            .Check("configure resource with name ( plaicon ) with url ( https://raw.githubusercontent.com/gissehel/BarLauncher-PowerLauncher/master/res/BarLauncher-PowerLauncher-128.png ) with keywords ( image logo powerlauncher )")
            .Check("configure resource with name ( winlogo ) with path ( c:\\Windows\\System32\\@WLOGO_48x48.png ) with keywords ( image logo windows )")
            .Check("configure resource with name ( winver ) with path ( c:\\windows-version.txt ) with keywords ( version windows )")
            .Check("configure tool with name ( hosts ) with exe ( vscode ) with resource ( hosts )")
            .Check("configure tool with name ( launchericon ) with exe ( edge ) with resource ( plaicon ) with keywords ( logo )")
            .Check("configure tool with name ( tcleft ) with exe ( totalcommander ) with keywords ( tc left panel ) with pattern ( /O /A /T /L=\"%{resource}\" ) ")
            .Check("configure tool with name ( tcright ) with exe ( totalcommander ) with keywords ( tc right panel ) with pattern ( /O /A /T /R=\"%{resource}\" ) ")
            .Check("configure tool with name ( windowsversion ) with exe ( notepad ) with resource ( winver )")
            .EndUsing()

            .EndTest();
    }
}