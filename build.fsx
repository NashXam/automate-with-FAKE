// (r)eferences an assembly.
#r @"FakeLib.dll"   

// Reads a source file, compiles it, and runs it.
#load "build-helpers.fsx"

// Declare imports
open Fake
open System
open System.IO
open System.Linq
open BuildHelpers
open Fake.XamarinHelper
open Fake.FileUtils

// http://fsharp.github.io/FAKE/
// Targets are the main unit of work in a "FAKE - F# Make" script. 
// Targets have a name and an action (given as a code block).

Target "ios-build" (fun () ->
    RestorePackages "Automate.sln"

    iOSBuild (fun defaults ->
        {defaults with
            ProjectPath = "Automate.sln"
            Configuration = "Debug"
            Target = "Build"
        })
)

Target "ios-uitests" (fun () ->
    let appPath = Directory.EnumerateDirectories(Path.Combine("iOS", "bin", "iPhoneSimulator", "Debug"), "*.app").First()

    RunUITests appPath "Automate.sln"
)

// Define dependencies
"ios-build"
  ==> "ios-uitests"

RunTarget()    