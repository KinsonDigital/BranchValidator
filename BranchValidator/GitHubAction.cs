// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;

namespace BranchValidator;

/// <inheritdoc/>
public sealed class GitHubAction : IGitHubAction
{
    private readonly IGitHubConsoleService gitHubConsoleService;
    private readonly IActionOutputService actionOutputService;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAction"/> class.
    /// </summary>
    /// <param name="gitHubConsoleService">Writes to the console.</param>
    /// <param name="actionOutputService">Sets the output data of the action.</param>
    public GitHubAction(
        IGitHubConsoleService gitHubConsoleService,
        IActionOutputService actionOutputService)
    {
        this.gitHubConsoleService = gitHubConsoleService;
        this.actionOutputService = actionOutputService;
    }

    /// <inheritdoc/>
    public async Task Run(ActionInputs inputs, Action onCompleted, Action<Exception> onError)
    {
        ShowWelcomeMessage();

        try
        {
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
        this.gitHubConsoleService.WriteLine("Welcome To The BranchValidator GitHub Action!!");
        this.gitHubConsoleService.BlankLine();
    }
}
