@echo off
cls
".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
".nuget\NuGet.exe" "Install" "NUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion"

SET TARGET="Default"
IF NOT [%1]==[] (set TARGET=%~1)

"packages\FAKE\tools\Fake.exe" %TARGET%

SET TARGET=""