module BuildHelpers

open Fake
open Fake.XamarinHelper
open System
open System.IO
open System.Linq
open System.Text.RegularExpressions

let Exec command args =
    let result = Shell.Exec(command, args)

    if result <> 0 then failwithf "%s exited with error %d" command result

let RestorePackages solutionFile =
    Exec ".nuget/nuget.exe" ("restore " + solutionFile)
    //solutionFile |> RestoreComponents (fun defaults -> {defaults with ToolPath = "tools/xpkg/xamarin-component.exe" })

let RunNUnitUITests dllPath xmlPath =
    Exec ".nuget/NuGet.exe" "install NUnit.Runners"
    !! dllPath |> NUnit (fun p -> p)
//    TeamCityHelper.sendTeamCityNUnitImport xmlPath

let RunUITests appPath solutionFile =
    let testAppFolder = Path.Combine("UITests", "testapps")
    
    if Directory.Exists(testAppFolder) then Directory.Delete(testAppFolder, true)
    Directory.CreateDirectory(testAppFolder) |> ignore

    let testAppPath = Path.Combine(testAppFolder, DirectoryInfo(appPath).Name)

    Directory.Move(appPath, testAppPath)

    RestorePackages solutionFile

    MSBuild "UITests/bin/Debug" "Build" [ ("Configuration", "Debug"); ("Platform", "Any CPU") ] [ "UITests/Automate.UITests.csproj" ] |> ignore

    RunNUnitUITests "UITests/bin/Debug/Automate.UITests.dll" "UITests/bin/Debug/testresults.xml"