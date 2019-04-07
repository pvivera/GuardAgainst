#tool "nuget:?package=OpenCover&version=4.6.519"
#tool "nuget:?package=XunitXml.TestLogger&version=2.1.26"
#tool "nuget:?package=OpenCoverToCoberturaConverter&version=0.3.4"
#tool "nuget:?package=ReportGenerator&version=4.0.4"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var buildVersion = Argument("buildVersion", default(string));
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

if (buildVersion == null)
{
  Warning($"'buildVersion' argument was NULL");
}
else
{
  Information($"'buildVersion'='{buildVersion}'");
}

//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////

var solutionPath = MakeAbsolute(File("./src/GuardAgainstLib.sln")).FullPath;
var nuspecPath = MakeAbsolute(File("./src/GuardAgainst.nuspec")).FullPath;
var binariesArtifactsFolder = MakeAbsolute(Directory("./Artifacts/Binaries/"));
var nugetArtifactsFolder = MakeAbsolute(Directory("./Artifacts/NuGet/"));
var testArtifactsFolder = MakeAbsolute(Directory("./Artifacts/TestOutput/"));
var xunitTestLoggerFolder = MakeAbsolute(Directory("./tools/XunitXml.TestLogger.2.1.26/build/_common"));

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    var testResultFolders = GetDirectories("./**/TestResults");    
    var artifactsFolders = GetDirectories("./**/Artifacts");
    var binFolders = GetDirectories("./src/**/bin");
    var objFolders = GetDirectories("./src/**/obj");
    var foldersToDelete = testResultFolders.Concat(artifactsFolders).Concat(binFolders).Concat(objFolders);
    
    DeleteDirectories(foldersToDelete, new DeleteDirectorySettings {
        Recursive = true,
        Force = true
    });

    DotNetCoreClean(solutionPath);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => 
{
    DotNetCoreRestore(solutionPath, new DotNetCoreRestoreSettings
    {
        Verbosity = DotNetCoreVerbosity.Minimal
    });
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild(solutionPath, new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        NoRestore = true,
        Verbosity = DotNetCoreVerbosity.Minimal
    });
});
    
Task("TestAndCoverage")
    .IsDependentOn("Build")
    .Does(() =>
{       
        CreateDirectory(testArtifactsFolder);

        var relativeCoverageResultPath = $"{testArtifactsFolder}/OpenCover-Coverage.xml";
        var mergeCoverageOutput = false;
        var projects = GetFiles("./src/**/*Test.csproj");

        foreach(var project in projects)
        {
            var projectName = project.GetFilenameWithoutExtension().FullPath;
            var projectPath = project.GetDirectory();
            var relativePath = projectPath.GetRelativePath(testArtifactsFolder);
            var relativeTestResultPath = $"{relativePath}/{projectName}.xml";

            var openCoverArguments = new ProcessArgumentBuilder()
                    .Append("-target:\"c:\\Program Files\\dotnet\\dotnet.exe\"")
                    .Append($"-targetargs:\"test -c {configuration} \"{project.FullPath}\" --no-build --no-restore --logger \"xunit;LogFilePath={relativeTestResultPath}\" --test-adapter-path \"{xunitTestLoggerFolder}\"\"")
                    .Append($"-output:\"{relativeCoverageResultPath}\"")
                    .Append("-filter:\"+[GuardAgainst*]* -[*Test]*\"")
                    .Append($"-searchdirs:\"{xunitTestLoggerFolder}\"")
                    .Append("-register:user")
                    .Append("-oldStyle")
                    .Append("-hideskipped:All");
                    
            if (mergeCoverageOutput)
            {
              openCoverArguments = openCoverArguments.Append("-mergeoutput");
            }
            
            mergeCoverageOutput = true;
                
            StartProcess("./tools/OpenCover.4.6.519/tools/OpenCover.Console.exe", new ProcessSettings { Arguments = openCoverArguments });
        }
        
        var openCoverToCoberturaConverterArguments = new ProcessArgumentBuilder()
          .Append($"-input:\"{relativeCoverageResultPath}\"")
          .Append($"-output:\"{testArtifactsFolder}/Cobertura-Coverage.xml\"")
          .Append($"-sources:\"{xunitTestLoggerFolder}\"")
          .Append($"-includeGettersSetters:true");

        StartProcess("./tools/OpenCoverToCoberturaConverter.0.3.4/tools/OpenCoverToCoberturaConverter.exe", new ProcessSettings { Arguments = openCoverToCoberturaConverterArguments });

        var reportGeneratorArguments = new ProcessArgumentBuilder()
          .Append($"-reports:\"{relativeCoverageResultPath}\"")
          .Append($"-targetdir:\"{testArtifactsFolder}/Coverage-Report/\"");
                    
        StartProcess("./tools/ReportGenerator.4.0.4/tools/net47/ReportGenerator.exe", new ProcessSettings { Arguments = reportGeneratorArguments });
});

Task("Publish")
    .IsDependentOn("TestAndCoverage")
    .Does(() =>
{
    var projects = GetFiles("./src/**/GuardAgainstLib.csproj");

    foreach(var project in projects)
    {
        var projectName = project.GetFilenameWithoutExtension().FullPath;
        var settings = new DotNetCorePublishSettings
        {
            Framework = "netstandard2.0",
            Configuration = "Release",
            OutputDirectory = $"{binariesArtifactsFolder}/{projectName}/",
            NoRestore = true,
            SelfContained = false
        };

        DotNetCorePublish(project.FullPath, settings);

        DeleteFiles($"{binariesArtifactsFolder}/{projectName}/*.deps.json");

        Zip($"{binariesArtifactsFolder}/{projectName}/", $"{binariesArtifactsFolder}/{projectName}.zip");

        DeleteDirectory($"{binariesArtifactsFolder}/{projectName}/", new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        });
    }

});

Task("Pack")
    //.WithCriteria(() => buildVersion != null)
    .IsDependentOn("Publish")
    .Does(() =>
{
                  var nuGetPackSettings   = new NuGetPackSettings {
                                     Id                      = "GuardAgainst",
                                     Version                 = buildVersion ?? "1.0.0",
                                     Title                   = "GuardAgainst",
                                     Authors                 = new[] {"Paul Mcilreavy"},
                                     Owners                  = new[] {"Paul Mcilreavy"},
                                     Description             = "Useful guard clauses that simplify argument validity checking and make your code more readable.",
                                     ProjectUrl              = new Uri("https://github.com/pmcilreavy/GuardAgainst"),
                                     IconUrl                 = new Uri("http://cdn.rawgit.com/pmcilreavy/GuardAgainst/master/GuardAgainst.png"),
                                     LicenseUrl              = new Uri("https://github.com/pmcilreavy/GuardAgainst/blob/master/LICENSE"),
                                     Tags                    = new [] {"GuardAgainst", "guard", "dotnet", "contracts", "arguments", "validity"},
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
                                     Files                   = new [] { 
                                      new NuSpecContent {Source = "./GuardAgainstLib.dll", Target = "lib/netstandard2.0/GuardAgainstLib.dll"},
                                      new NuSpecContent {Source = "./GuardAgainstLib.xml", Target = "lib/netstandard2.0/GuardAgainstLib.xml"},
                                     },
                                     BasePath                = "./src/GuardAgainstLib/bin/release/netstandard2.0",
                                     OutputDirectory         = nugetArtifactsFolder.FullPath
                                 };

     NuGetPack(nuspecPath, nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);