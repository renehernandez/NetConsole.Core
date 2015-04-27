// include Fake lib

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.ReleaseNotesHelper
open Fake.Git

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
let version = release.NugetVersion

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

Target "BuildPackage" (fun _ ->
    Paket.Pack(fun p ->
        { p with
            OutputPath = binDir
            Version = release.NugetVersion
            ReleaseNotes = toLines release.Notes})
)

Target "BuildZip" (fun _ ->
        !! (buildDir + "/**/*.*")
            -- "*.zip"
            |> Zip buildDir (binDir + projectName + "." +  version + ".zip" )
)

Target "ReleasePackage" (fun _ ->
    Paket.Push(fun p ->
        { p with
            WorkingDir = binDir
        })
)

Target "PushDevelop" (fun _ ->
    Branches.pushBranch "" "origin" "develop"
)

Target "PushMaster" (fun _ ->
    Branches.pushBranch "" "origin" "master"

    Branches.tag "" release.NugetVersion
    Branches.pushTag "" "origin" release.NugetVersion
)

Target "Master" (fun _ ->
    trace "Build completed"
)

// Default target
Target "Develop" (fun _ ->
        trace "Build completed"
)

"Clean"
    ==> "BuildApp"
    ==> "BuildTest"
    ==> "Test"
    ==> "BuildZip"
    ==> "BuildPackage"
    ==> "PushDevelop"
    ==> "Develop"

"Clean"
    ==> "BuildApp"
    ==> "BuildTest"
    ==> "Test"
    ==> "BuildZip"
    ==> "BuildPackage"
    ==> "PushMaster"
    ==> "Master"

// start build
RunTargetOrDefault "Develop"
