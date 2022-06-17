// See https://aka.ms/new-console-template for more information

// ReSharper disable once InconsistentNaming

using System.Diagnostics;
using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScriptGenerator;
using ScriptGenerator.Factories;
using ScriptGenerator.Services;

// var basePath = $@"K:\SOFTWARE DEVELOPMENT\PERSONAL\BranchValidator\Tooling\";
// var fileName = $"Hello{data}.txt";
// var fullPath = $"{basePath}{fileName}";
var fileSystem = new FileSystem();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<IDirectory>(_ => fileSystem.Directory);
        services.AddSingleton<IFile>(_ => fileSystem.File);
        services.AddSingleton<IPath>(_ => fileSystem.Path);
        services.AddSingleton<IScriptTemplateService, ScriptTemplateService>();
        services.AddSingleton<IAppService, AppService>();
        services.AddSingleton<IConsoleService, ConsoleService>();
        services.AddSingleton(_ => MutationFactory.CreateMutations());
        services.AddSingleton<IRelativePathResolverService, RelativePathResolverService>();
        services.AddSingleton<IArgParsingService<AppInputs>, ArgParsingService>();
        services.AddSingleton<IFileLoaderService<string>, TextFileLoaderService>();
        services.AddSingleton<IFileLoaderService<string[]>, TextFileLinesLoaderService>();
        services.AddSingleton<IFunctionExtractorService, FunctionExtractorService>();
        services.AddSingleton<IGeneratorService, GeneratorService>();
    }).Build();

IArgParsingService<AppInputs> argParsingService = host.Services.GetRequiredServiceAndHandleError<IArgParsingService<AppInputs>>();
IAppService appService = host.Services.GetRequiredServiceAndHandleError<IAppService>();
IConsoleService consoleService = host.Services.GetRequiredServiceAndHandleError<IConsoleService>();
IGeneratorService generatorService = host.Services.GetRequiredServiceAndHandleError<IGeneratorService>();

var process = new Process();
process.StartInfo.FileName = "calc.exe";
process.StartInfo.Arguments = "";
process.Start();

try
{
    await argParsingService.ParseArguments(
        new AppInputs(),
        args,
        async inputs =>
        {
            generatorService.GenerateScript(inputs.SourceFilePath, inputs.DestinationDirPath, inputs.FileName);
        }, errors =>
        {
            foreach (var error in errors)
            {
                consoleService.WriteError(error);
            }
        });
}
catch (Exception e)
{
    Debugger.Break();
}
