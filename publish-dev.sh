#!/usr/bin/env bash

rm -rf ./*/bin ./*/obj ./build

VERSION=$(cat VERSION)-$(date +%s)

dotnet.exe publish BarLauncher.PowerLauncher.Wox/BarLauncher.PowerLauncher.Wox.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.PowerLauncher.Wox/bin/Debug/net48/publish; zip -r ../../../../../../../BarLauncher-PowerLauncher-${VERSION}.wox .)

dotnet.exe publish BarLauncher.PowerLauncher.Flow.Launcher/BarLauncher.PowerLauncher.Flow.Launcher.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.PowerLauncher.Flow.Launcher/bin/Debug/net5.0-windows/publish; zip -r ../../../../../../../BarLauncher-PowerLauncher-${VERSION}.zip .)
