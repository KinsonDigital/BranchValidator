// See https://aka.ms/new-console-template for more information

// ReSharper disable once InconsistentNaming

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScriptGenerator;
using ScriptGenerator.Services;

// var basePath = $@"K:\SOFTWARE DEVELOPMENT\PERSONAL\BranchValidator\Tooling\";
// var fileName = $"Hello{data}.txt";
// var fullPath = $"{basePath}{fileName}";

IHost host = null!;

host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<IArgParsingService<AppInputs>, ArgParsingService>();
        services.AddSingleton<IFileLoaderService<string>, TextualFileLoader>();
    }).Build();


var argParsingService = host.Services.GetRequiredService<IArgParsingService<AppInputs>>();

await argParsingService.ParseArguments(
    new AppInputs(),
    args,
    async inputs =>
    {
    }, errors =>
    {
    });
