// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;

namespace BranchValidator;

/// <inheritdoc/>
public sealed class GitHubAction : IGitHubAction
{
    private readonly IExpressionExecutorService expressionExecutorService;
    private readonly IGitHubConsoleService consoleService;
    private readonly IActionOutputService outputService;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAction"/> class.
    /// </summary>
    /// <param name="expressionExecutorService">Executes expressions.</param>
    /// <param name="consoleService">Prints messages to the GitHub console.</param>
    /// <param name="outputService">Sets the GitHub action outputs.</param>
    public GitHubAction(
        IExpressionExecutorService expressionExecutorService,
        IGitHubConsoleService consoleService,
        IActionOutputService outputService)
    {
        this.expressionExecutorService = expressionExecutorService;
        this.consoleService = consoleService;
        this.outputService = outputService;
    }

    /// <inheritdoc/>
    public async Task Run(ActionInputs inputs, Action onCompleted, Action<Exception> onError)
    {
        ShowWelcomeMessage();

        try
        {
            // TODO: Bring in the IFunctionService to get a list of functions to print to the console.
        }
        catch (Exception e)
        {
            onError(e);
        }

        onCompleted();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        // TODO: Dispose of something here.  If nothing to dispose, remove this comment
        this.isDisposed = true;
    }

    /// <summary>
    /// Shows a welcome message.
    /// </summary>
    private void ShowWelcomeMessage()
    {
        this.consoleService.WriteLine("Welcome To The BranchValidator GitHub Action!!");
        this.consoleService.BlankLine();
    }
}
