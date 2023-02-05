using AllGreen.Lib;
using BarLauncher.PowerLauncher.Test.AllGreen.Fixture;
using BarLauncher.PowerLauncher.Test.AllGreen.Helper;

namespace BarLauncher.PowerLauncher.Test.AllGreen.Test
{
    public class Prepare_common_context : TestBase<PowerLauncherContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Application_information_fixture>()
            .DoAction(f => f.Application_name_is("BarLauncher-PowerLauncher"))
            .DoAction(f => f.Application_verison_is("0.0"))
            .DoAction(f => f.Application_url_is("https://github.com/gissehel/BarLauncher-PowerLauncher"))
            .EndUsing()

            .Using<FileSystem_fixture>()
            .DoAction(f => f.ReferenceFile("c:\\Program Files\\GIMP 2\\bin\\gimp-2.10.exe"))
            .DoAction(f => f.ReferenceFile("c:\\Program Files\\IrfanView\\i_view64.exe"))
            .DoAction(f => f.ReferenceFile("c:\\Program Files\\TOTALCMD64.EXE"))
            .DoAction(f => f.ReferenceFile("c:\\Program Files(x86)\\Microsoft\\Edge\\Application\\msedge.exe"))
            .DoAction(f => f.ReferenceFile("c:\\Users\\Grut\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe"))
            .DoAction(f => f.ReferenceFile("c:\\Windows\\System32\\drivers\\etc\\hosts"))
            .DoAction(f => f.ReferenceFile("c:\\Windows\\System32\\notepad.exe"))
            .DoAction(f => f.ReferenceFile("c:\\Windows\\System32\\WindowsSecurityIcon.png"))
            .DoAction(f => f.ReferenceFile("c:\\Windows\\System32\\@WLOGO_48x48.png"))
            .DoAction(f => f.ReferenceFile("c:\\Windows\\SysWOW64\\explorer.exe"))
            .DoAction(f => f.ReferenceFile("c:\\Windows-version.txt"))
            .DoAction(f => f.ReferenceFile("d:\\folder\\test.txt"))
            .DoAction(f => f.ReferenceFile("d:\\folder\\test.png"))
            .DoAction(f => f.ReferenceFile("d:\\readme.md"))
            .EndUsing()

            .Using<BarLauncher_bar_fixture>()
            .DoAction(f => f.Start_the_bar())

            .DoAction(f => f.Display_bar_launcher())
            .DoCheck(f => f.The_current_query_is(), "")
            .DoAction(f => f.Write_query("pla configure exe with name ( vscode ) with path ( c:\\Users\\Grut\\AppData\\Local\\Programs\\Microsoft VS Code\\Code.exe ) with keywords ( editor text ide )"))
            .DoAction(f => f.Select_line(1))
            .DoReject(f => f.Bar_launcher_is_displayed())

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure exe with name ( notepad ) with path ( c:\\Windows\\System32\\notepad.exe ) with keywords ( editor text )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure resource with name ( hosts ) with path ( c:\\Windows\\System32\\drivers\\etc\\hosts ) with keywords ( dns ip )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure resource with name ( winver ) with path ( c:\\windows-version.txt ) with keywords ( windows version )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure tool with name ( hosts ) with exe ( vscode ) with resource ( hosts )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure exe with name ( edge ) with path ( c:\\Program Files(x86)\\Microsoft\\Edge\\Application\\msedge.exe ) with keywords ( browser web )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure exe with name ( irfanview ) with path ( c:\\Program Files\\IrfanView\\i_view64.exe ) with keywords ( viewer image )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure resource with name ( plaicon ) with url ( https://raw.githubusercontent.com/gissehel/BarLauncher-PowerLauncher/master/res/BarLauncher-PowerLauncher-128.png ) with keywords ( image logo powerlauncher )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure resource with name ( winlogo ) with path ( c:\\Windows\\System32\\@WLOGO_48x48.png ) with keywords ( logo image windows )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure tool with name ( windowsversion ) with exe ( notepad ) with resource ( winver )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure tool with name ( launchericon ) with exe ( edge ) with resource ( plaicon ) with keywords ( logo )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure exe with name ( totalcommander ) with path ( c:\\Program Files\\TOTALCMD64.EXE ) with keywords ( file manager )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure tool with name ( tcleft ) with exe ( totalcommander ) with pattern ( /O /A /T /L=\"%{resource}\" ) with keywords ( tc left panel )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query("pla configure tool with name ( tcright ) with exe ( totalcommander ) with pattern ( /O /A /T /R=\"%{resource}\" ) with keywords ( tc right panel )"))
            .DoAction(f => f.Select_line(1))

            .DoAction(f => f.Display_bar_launcher())
            .DoAction(f => f.Write_query(""))
            .EndUsing()

            .EndTest();
    }
}