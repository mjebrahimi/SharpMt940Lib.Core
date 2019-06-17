// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System.IO
open Fake.ReleaseNotesHelper
open Fake.AssemblyInfoFile
open Fake.Testing

// Nuget
let authors = [ "Jaco Adriaansen" ]
let projectName = "Raptorious.Finance.Swift.Mt940"
let projectTitle = "SharpMt940Lib"
let projectDescription = "SharpMT940Lib implements the MT940 format in C# and is based on specifications of multiple banks. You can use it as a base for financial software or for conversions to other formats like CSV or OFX."
let projectSummary = "SharpMt940Lib is a C# library that allows you to read MT940 formatted files and convert it to in-memory .NET objects."

let publish = bool.Parse(getBuildParamOrDefault "publish" bool.FalseString)
let apikey = getBuildParamOrDefault "nugetkey" ""
let configuration = getBuildParamOrDefault "configuration" "debug"
let suffix = getBuildParamOrDefault "suffix" ""
let buildnumber = getBuildParamOrDefault "buildnumber" "0"

let releaseDir = ".artifacts/release"
let testDir = ".artifacts/test"
let packagesDirectory = ".artifacts/packages"

let release = LoadReleaseNotes ("Raptorious.SharpMT940Lib" @@ "CHANGELOG.md")

let CompileProjects = (fun configuration arch -> 
    let includes = 
        !! "Raptorious.SharpMT940Lib/*.csproj" 
            ++ "Raptorious.SharpMT940Lib.Tests/*.csproj" 

    let buildDir = releaseDir

    includes
        |> MSBuild buildDir "Build" ["Configuration", configuration; "Platform", arch]
        |> Log "Ouput"
)

let Compile arch = CompileProjects configuration arch
let CompileDebug arch = CompileProjects "Debug" arch
let CompileRelease arch = CompileProjects "Release" arch

RestorePackages()

// Default target
Target "Default" (fun _ ->
    printf "%b" publish
)

Target "Clean" (fun _ ->
    CleanDirs [| releaseDir; testDir; packagesDirectory |]
)

Target "Build" (fun _ ->
    trace ""
)

Target "Build-AnyCPU" (fun _ -> 
    Compile "AnyCPU"
)

Target "Build-x86" (fun _ -> 
    Compile "x86"
)

Target "Build-x64" (fun _ -> 
    Compile "x64"
)

Target "Test" (fun _ -> 
     !! (releaseDir + "/Raptorious.*.Tests.dll")
     |> NUnit3 (fun p -> 
        { p with 
            ShadowCopy = false;
            ToolPath = "packages/NUnit.ConsoleRunner/tools/nunit3-console.exe";
        })
)

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo "raptorious.sharpmt940lib/properties/assemblyinfo.cs"
        [Attribute.Title projectName
         Attribute.Description projectDescription
         Attribute.Product projectName
         Attribute.Copyright ("Copyright 2011-" + System.DateTime.Now.Year.ToString() + " " + (authors |> List.reduce(+)))
         Attribute.Version (release.AssemblyVersion + "." + buildnumber)
         Attribute.FileVersion (release.AssemblyVersion + "." + buildnumber)
         Attribute.InformationalVersion (release.AssemblyVersion + "." + buildnumber + suffix)
         Attribute.Guid "abd839a7-a3e5-475c-8fe1-e936c5091652"
         Attribute.ComVisible false
         Attribute.CLSCompliant true]
)

Target "Package" (fun _ ->
    let nuspec =  "Raptorious.SharpMT940Lib.nuspec"

    let assembly = releaseDir @@ "Raptorious.SharpMt940Lib.dll"
    let symbols = releaseDir @@ "Raptorious.SharpMt940Lib.pdb"
    let xmldoc = releaseDir @@ "Raptorious.Finance.Swift.xml"
               
    let libDir = (packagesDirectory @@ "lib");
   
    CreateDir packagesDirectory
    CreateDir libDir
    
    CopyFile packagesDirectory ("Raptorious.SharpMT940Lib" @@ nuspec)
    CopyFile libDir assembly
    
    
    let setParams p = 
        { p with
            Authors = authors
            Title = projectTitle
            Project = projectName
            ReleaseNotes = release.Notes |> toLines
            Description = projectDescription                               
            OutputPath = packagesDirectory
            Summary = projectSummary
            WorkingDir = packagesDirectory
            Version = (release.NugetVersion + "." + buildnumber + suffix)
            AccessKey = apikey
            Publish = publish
            SymbolPackage = NugetSymbolPackage.Nuspec }

    NuGet setParams (packagesDirectory @@ nuspec)
)



"Clean"
==> "AssemblyInfo"
==> "Build"



"Build-AnyCPU"
==> "Build"

"Build"
==> "Test"

"Test"
==> "Package"

// start build
RunTargetOrDefault "Default"