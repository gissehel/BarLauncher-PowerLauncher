using AllGreen.Lib;
using BarLauncher.PowerLauncher.Test.AllGreen.Fixture;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Test
{
    public class Configure_resource : TestBase<PowerLauncherContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context>()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Write_query("pla con"))
            .DoCheck(f => f.The_number_of_results_is(), "1")
            .DoCheck(f => f.The_title_of_result__is(1), "configure [type] [...]")
            .DoCheck(f => f.The_subtitle_of_result__is(1), "Configure a resource/exe/tool/...")

            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( [resource name] ) [...]", "Configure a resource")
            .Check("configure exe ( [exe name] ) [...]", "Configure an exe")
            .Check("configure tool ( [tool name] ) [...]", "Configure a tool")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f=>f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( [resource name] ) [...]", "You must enter a resource name")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( [resource name] ) [...]", "You must enter a resource name")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("se"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( se[resource name] ) [...]", "You must enter a resource name")
            .Check("configure resource ( se ) [...]", "Create resource named 'se'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( se")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("cicon"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon[resource name] ) [...]", "You must enter a resource name")
            .Check("configure resource ( secicon ) [...]", "Create resource named 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( [path name] ) [...]", "Set the path of a local resource for name 'secicon'")
            .Check("configure resource ( secicon ) with url ( [url name] ) [...]", "Set the url of a remote resource for name 'secicon'")
            .Check("configure resource ( secicon ) with keywords ( [keywords] ) [...]", "Add keywords for name 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\[...] ) [...]", "Set the path to start by c:\\")
            .Check("configure resource ( secicon ) with path ( d:\\[...] ) [...]", "Set the path to start by d:\\")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\ ) [...]", "Set the path to be c:\\")
            .Check("configure resource ( secicon ) with path ( c:\\Program Files ) [...]", "Set the path to start by c:\\Program Files")
            .Check("configure resource ( secicon ) with path ( c:\\Users ) [...]", "Set the path to start by c:\\Users")
            .Check("configure resource ( secicon ) with path ( c:\\Windows ) [...]", "Set the path to start by c:\\Windows")
            .Check("configure resource ( secicon ) with path ( c:\\Windows-version.txt ) [...]", "Set the path to be c:\\Windows-version.txt")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(4))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\Windows")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\Windows ) [...]", "Set the path to be c:\\Windows")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32 ) [...]", "Set the path to start by c:\\Windows\\System32")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\SysWOW64 ) [...]", "Set the path to start by c:\\Windows\\SysWOW64")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\Windows\\System32")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32 ) [...]", "Set the path to be c:\\Windows\\System32")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\notepad.exe ) [...]", "Set the path to be c:\\Windows\\System32\\notepad.exe")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\drivers ) [...]", "Set the path to start by c:\\Windows\\System32\\drivers")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) [...]", "Set the path to be c:\\Windows\\System32\\WindowsSecurityIcon.png")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\@WLOGO_48x48.png ) [...]", "Set the path to be c:\\Windows\\System32\\@WLOGO_48x48.png")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(4))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png )", "Create resource named 'secicon'")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( [keywords] ) [...]", "Add keywords for name 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( ", "You must enter keywords")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( ")
            .DoAction(f => f.Append__on_query("shield"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( shield[...] ) [...]", "You must enter keywords")
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( shield ) [...]", "Create resource named 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( shield )")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("configure resource ( secicon ) with path ( c:\\Windows\\System32\\WindowsSecurityIcon.png ) with keywords ( shield )", "Create resource named 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .Comment("Now check that the resource can be used")

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Display_bar_launcher())
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoAction(f => f.Write_query("pla start "))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with tool ( [tool name] ) [...]", "Start a tool using a tool name")
            .Check("start with exe ( [exe name] ) [...]", "Start a tool using an exe name")
            .Check("start with exepath ( [path] ) [...]", "Start a tool using an exe's path")
            .Check("start with resource ( [resource name] ) [...]", "Start a tool on a resource using a resource name")
            .Check("start with resourcepath ( [path] ) [...]", "Start a tool on a resource using a resource's path")
            .Check("start with pattern ( [pattern] ) [...]", "Start a tool using a pattern")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla start with exe ( ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( vscode ) [...]", "Start exe 'vscode'")
            .Check("start with exe ( notepad ) [...]", "Start exe 'notepad'")
            .Check("start with exe ( edge ) [...]", "Start exe 'edge'")
            .Check("start with exe ( irfanview ) [...]", "Start exe 'irfanview'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("w"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) [...]", "Start exe 'edge'")
            .Check("start with exe ( irfanview ) [...]", "Start exe 'irfanview'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(2))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla start with exe ( edge ) ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) with resource ( [resource name] ) [...]", "Start exe 'edge' on a resource using a resource name")
            .Check("start with exe ( edge ) with resourcepath ( [path] ) [...]", "Start exe 'edge' on a resource using a resource's path")
            .Check("start with exe ( edge ) pattern ( [pattern] ) [...]", "Start exe 'edge' using a pattern")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla start with exe ( edge ) with resource ( ")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) with resource ( hosts ) [...]", "Start exe 'edge' on resource 'hosts'")
            .Check("start with exe ( edge ) with resource ( plaicon ) [...]", "Start exe 'edge' on resource 'plaicon'")
            .Check("start with exe ( edge ) with resource ( secicon ) [...]", "Start exe 'edge' on resource 'secicon'")
            .Check("start with exe ( edge ) with resource ( winlogo ) [...]", "Start exe 'edge' on resource 'winlogo'")
            .Check("start with exe ( edge ) with resource ( winver ) [...]", "Start exe 'edge' on resource 'winver'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("s"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) with resource ( hosts ) [...]", "Start exe 'edge' on resource 'hosts'")
            .Check("start with exe ( edge ) with resource ( plaicon ) [...]", "Start exe 'edge' on resource 'plaicon'")
            .Check("start with exe ( edge ) with resource ( secicon ) [...]", "Start exe 'edge' on resource 'secicon'")
            .Check("start with exe ( edge ) with resource ( winlogo ) [...]", "Start exe 'edge' on resource 'winlogo'")
            .Check("start with exe ( edge ) with resource ( winver ) [...]", "Start exe 'edge' on resource 'winver'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("e"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) with resource ( plaicon ) [...]", "Start exe 'edge' on resource 'plaicon'")
            .Check("start with exe ( edge ) with resource ( secicon ) [...]", "Start exe 'edge' on resource 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Append__on_query("c"))
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) with resource ( secicon ) [...]", "Start exe 'edge' on resource 'secicon'")
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoAccept(f => f.Bar_launcher_is_displayed())
            .DoCheck(f => f.The_current_query_is(), "pla start with exe ( edge ) with resource ( secicon )")
            .EndUsing()

            .UsingList<BarLauncher_results_fixture>()
            .With<BarLauncher_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("start with exe ( edge ) with resource ( secicon )", "Start exe 'edge' on resource 'secicon'")
            .Check("start with exe ( edge ) with resource ( secicon ) with pattern ( [pattern] )", "Start exe 'edge' on resource 'secicon' using a pattern")
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(r => r.Command, r => r.Arguments)
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())
            .EndUsing()

            .UsingList<Command_line_started_fixture>()
            .With<Command_line_started_fixture.Result>(r => r.Command, r => r.Arguments)
            .Check("c:\\Program Files(x86)\\Microsoft\\Edge\\Application\\msedge.exe", "\"c:\\Windows\\System32\\WindowsSecurityIcon.png\"")
            .EndUsing()

            .EndTest();
    }
}