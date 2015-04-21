// include Fake lib

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.ReleaseNotesHelper

// Properties

// Scaffold Properties
let srcDir = "./src/"
let buildDir = "./build/"
let testDir = "./tests/"
let libDir = "./lib/"
let binDir = "./bin/"
let docDir = "./docs/"

// Filesets Properties
let appReferences = !! "src/app/**/*.csproj"
let testReferences = !! "src/tests/**/*.csproj"

// Project Properties

let projectName = "NetConsole.Core"
let release = LoadReleaseNotes "RELEASE_NOTES.md"
let version = "0.0.1" // or retrieve from CI server


Target "Clean" (fun _ ->
        CleanDirs [buildDir; testDir; binDir; docDir]
)

Target "BuildApp" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
            |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Build" testReferences
            |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
        !! (testDir + "*Tests*.dll")
            |> NUnit (fun p ->
                {
                    p with
                        DisableShadowCopy = true;
                        OutputFile = testDir + "TestResults.xml"
                }
            )
)

Target "NuGet" (fun _ ->
    Paket.Pack(fun p ->
        { p with
            OutputPath = binDir
            Version = release.NugetVersion
            ReleaseNotes = toLines release.Notes})
)

Target "Zip" (fun _ ->
        !! (buildDir + "/**/*.*")
            -- "*.zip"
            |> Zip buildDir (binDir + projectName + "." +  version + ".zip" )
)

Target "Deploy" DoNothing

// Default target
Target "All" (fun _ ->
        trace "Build completed"
)

"Clean"
    ==> "BuildApp"
    ==> "BuildTest"
    ==> "Test"
    ==> "Zip"
    ==> "All"

// start build
RunTargetOrDefault "All"
