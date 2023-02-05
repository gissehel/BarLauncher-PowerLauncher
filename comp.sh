rm -rf */bin */obj */build build

dotnet.exe publish BarLauncher.PowerLauncher.Wox/BarLauncher.PowerLauncher.Wox.csproj -c Debug
dotnet.exe publish BarLauncher.PowerLauncher.Flow.Launcher/BarLauncher.PowerLauncher.Flow.Launcher.csproj -c Debug -r win-x64

