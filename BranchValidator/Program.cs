// <copyright file="Program.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using BranchValidator.Factories;
using BranchValidator.Observables;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;

[assembly: InternalsVisibleTo("BranchValidatorTests", AllInternalsVisible = true)]

namespace BranchValidator;

/// <summary>
/// The main application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Program
{
    private static IHost host = null!;

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
                services.AddSingleton<IBranchNameObservable, UpdateBranchNameObservable>();
                services.AddSingleton<IJSONService, JSONService>();
                services.AddSingleton<IEmbeddedResourceLoaderService<string>, TextResourceLoaderService>();
                services.AddSingleton<IAppService, AppService>();
                services.AddSingleton<IGitHubConsoleService, GitHubConsoleService>();
                services.AddSingleton<IActionOutputService, ActionOutputService>();
                services.AddSingleton<IArgParsingService<ActionInputs>, ArgParsingService>();
                services.AddSingleton<IMethodExecutor, MethodExecutor>();
                services.AddSingleton<IFunctionDefinitions, FunctionDefinitions>();
                services.AddSingleton<IFunctionService, FunctionService>();
                services.AddSingleton<IExpressionValidatorService, ExpressionValidatorService>();
                services.AddSingleton<IAnalyzerFactory, AnalyzerFactory>();
                services.AddSingleton<IExpressionExecutorService, ExpressionExecutorService>();
                services.AddSingleton<IGitHubAction<bool>, GitHubAction>();
            }).Build();

        var appService = host.Services.GetRequiredService<IAppService>();
        var consoleService = host.Services.GetRequiredService<IGitHubConsoleService>();
        IGitHubAction<bool>? gitHubAction = null;

        try
        {
            gitHubAction = host.Services.GetRequiredService<IGitHubAction<bool>>();
        }
        catch (Exception e)
        {
#if DEBUG
            appService.ExitWithException(e);
#endif
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
