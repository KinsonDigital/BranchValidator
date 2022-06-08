// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;

namespace BranchValidator;

/// <inheritdoc/>
public sealed class GitHubAction : IGitHubAction
{
    private readonly IGitHubConsoleService consoleService;
    private readonly IActionOutputService outputService;
    private readonly IExpressionExecutorService expressionExecutorService;
    private readonly IFunctionService functionService;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAction"/> class.
    /// </summary>
    /// <param name="expressionExecutorService">Executes expressions.</param>
    /// <param name="consoleService">Prints messages to the GitHub console.</param>
    /// <param name="outputService">Sets the GitHub action outputs.</param>
    public GitHubAction(
        IGitHubConsoleService consoleService,
        IActionOutputService outputService,
        IExpressionExecutorService expressionExecutorService,
        IFunctionService functionService)
    {
        this.consoleService = consoleService;
        this.outputService = outputService;
        this.expressionExecutorService = expressionExecutorService;
        this.functionService = functionService;
    }

    /// <inheritdoc/>
    public async Task Run(ActionInputs inputs, Action onCompleted, Action<Exception> onError)
    {
        ShowWelcomeMessage();

        var functionSignatures = this.functionService.FunctionSignatures;

        var functionList = "Available Functions:";

        foreach (var functionSignature in functionSignatures)
        {
            functionList += $"{Environment.NewLine}\t{functionSignature}";
        }

        this.consoleService.WriteLine(functionList);

        try
        {
            if (string.IsNullOrEmpty(inputs.BranchName))
            {
                throw new InvalidActionInput("The 'branch-name' action input cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(inputs.ValidationLogic))
            {
                throw new InvalidActionInput("The 'validation-logic' action input cannot be null or empty.");
            }

            (bool branchIsValid, string msg) logicResult = this.expressionExecutorService.Execute(inputs.ValidationLogic, inputs.BranchName);

            this.consoleService.WriteLine(logicResult.msg);

            if (inputs.FailWhenNotValid ?? false)
            {
                throw new Exception(logicResult.msg);
            }

            this.outputService.SetOutputValue("valid-branch", logicResult.branchIsValid.ToString().ToLower());
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
