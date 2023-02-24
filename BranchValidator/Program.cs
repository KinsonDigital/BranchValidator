// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BranchValidator.Observables;
using BranchValidator.Services;
using BranchValidator.Services.Analyzers;
using BranchValidator.Services.Interfaces;
using KDActionUtils;
using KDActionUtils.Services;

namespace BranchValidator;

/// <summary>
/// The main application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Program
{
    private static IHost? host;

    /// <summary>
    /// The main entry point of the GitHub action.
    /// </summary>
    /// <param name="args">The incoming arguments(action inputs).</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task Main(string[] args)
    {
        host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IScriptService<(bool result, string[] funcResults)>, ScriptService<(bool result, string[] funcResults)>>();
                services.AddSingleton<IBranchNameObservable, UpdateBranchNameObservable>();
                services.AddSingleton<IEmbeddedResourceLoaderService<string>, TextResourceLoaderService>();
                services.AddSingleton<IAppService, AppService>();
                services.AddSingleton<IConsoleService<ConsoleContext>, GitHubConsoleService>();
                services.AddSingleton<IActionOutputService, ActionOutputService>();
                services.AddSingleton<ICSharpMethodService, CSharpMethodService>();
                services.AddSingleton<IParsingService, ParsingService>();
                services.AddSingleton<IArgParsingService<ActionInputs>, ArgParsingService<ActionInputs>>();
                services.AddSingleton<IFunctionExtractorService, FunctionExtractorService>();
                services.AddSingleton<IExpressionValidatorService, ExpressionValidatorService>();
                services.AddSingleton<IExpressionExecutorService, ExpressionExecutorService>();
                services.AddSingleton(serviceProvider =>
                {
                    var funNameExtractorService = serviceProvider.GetRequiredService<IFunctionExtractorService>();
                    var methodNamesService = serviceProvider.GetRequiredService<ICSharpMethodService>();

                    var result = new IAnalyzerService[]
                    {
                        new ParenAnalyzerService(),
                        new QuoteAnalyzerService(),
                        new OperatorAnalyzerService(),
                        new FunctionAnalyzerService(funNameExtractorService, methodNamesService),
                        new NegativeNumberAnalyzer(),
                    };

                    return result.ToReadOnlyCollection();
                });
                services.AddSingleton<IGitHubAction<ActionInputs, bool>, GitHubAction>();
            }).Build();

        var appService = host.Services.GetRequiredService<IAppService>();
        var consoleService = host.Services.GetRequiredService<IConsoleService<ConsoleContext>>();
        IGitHubAction<ActionInputs, bool>? gitHubAction = null;

        try
        {
            gitHubAction = host.Services.GetRequiredService<IGitHubAction<ActionInputs, bool>>();
        }
        catch (Exception e)
        {
            appService.ExitWithException(e);
        }

        var argParsingService = host.Services.GetRequiredService<IArgParsingService<ActionInputs>>();

        await argParsingService.ParseArguments(
            new ActionInputs(),
            args,
            async inputs =>
            {
                if (gitHubAction is null)
                {
                    appService.ExitWithException(new NullReferenceException("The GitHub action object is null."));
                    return;
                }

                await gitHubAction.Run(
                    inputs,
                    _ =>
                    {
                        host.Dispose();
                        Default.Dispose();
                        gitHubAction.Dispose();
                        appService.Exit(0);
                    },
                    e =>
                    {
                        host.Dispose();
                        Default.Dispose();
                        gitHubAction.Dispose();

                        e.HResult = 400;
                        appService.ExitWithException(e);
                    });
            }, errors =>
            {
                foreach (var error in errors)
                {
                    consoleService.WriteLine(error);
                }

                appService.ExitWithException(new Exception($"There were {errors.Length} errors.  Refer to the logs for more information."));
            });
    }
}
