// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using BranchValidator.Services.Interfaces;

namespace BranchValidator;

/// <inheritdoc/>
public sealed class GitHubAction : IGitHubAction<bool>
{
    private readonly IGitHubConsoleService consoleService;
    private readonly IActionOutputService outputService;
    private readonly IExpressionValidatorService expressionValidatorService;
    private readonly IExpressionExecutorService expressionExecutorService;
    private readonly ICSharpMethodService csharpMethodService;
    private readonly IParsingService parsingService;
    private readonly IBranchNameObservable branchNameObservable;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAction"/> class.
    /// </summary>
    /// <param name="expressionExecutorService">Executes expressions.</param>
    /// <param name="consoleService">Prints messages to the GitHub console.</param>
    /// <param name="outputService">Sets the GitHub action outputs.</param>
    /// <param name="branchNameObservable">Sends a push notification of the branch name.</param>
    public GitHubAction(
        IGitHubConsoleService consoleService,
        IActionOutputService outputService,
        IExpressionValidatorService expressionValidatorService,
        IExpressionExecutorService expressionExecutorService,
        ICSharpMethodService csharpMethodService,
        IParsingService parsingService,
        IBranchNameObservable branchNameObservable)
    {
        // TODO: Check if any of these are null.  Write unti tests
        this.consoleService = consoleService;
        this.outputService = outputService;
        this.expressionValidatorService = expressionValidatorService;
        this.expressionExecutorService = expressionExecutorService;
        this.csharpMethodService = csharpMethodService;
        this.parsingService = parsingService;
        this.branchNameObservable = branchNameObservable;
    }

    /// <inheritdoc/>
    public async Task Run(ActionInputs inputs, Action<bool> onCompleted, Action<Exception> onError)
    {
        var branchIsValid = false;
        ShowWelcomeMessage();

        // Update the function definitions object of the branch name
        this.branchNameObservable.PushNotification(inputs.BranchName);
        this.branchNameObservable.UnsubscribeAll();

        var methodSignatures = this.csharpMethodService.GetMethodSignatures(nameof(FunctionDefinitions))
            .ToArray();

        var functionSignatures = methodSignatures.Select(s => this.parsingService.ToExpressionFunctionSignature(s)).ToArray();

        this.consoleService.WriteGroup("Available Functions", functionSignatures);

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

            var validSyntaxResult = this.expressionValidatorService.Validate(inputs.ValidationLogic);

            if (validSyntaxResult.isValid is false)
            {
                var exceptionMsg = $"Invalid Syntax:{Environment.NewLine}";
                exceptionMsg += $"\t{validSyntaxResult.msg}";

                throw new Exception(exceptionMsg);
            }

            (bool branchIsValid, string msg) logicResult = this.expressionExecutorService.Execute(inputs.ValidationLogic, inputs.BranchName);
            branchIsValid = logicResult.branchIsValid;

            if (inputs.FailWhenNotValid is true && logicResult.branchIsValid is false)
            {
                throw new Exception(logicResult.msg);
            }

            this.consoleService.BlankLine();
            this.consoleService.WriteLine(logicResult.msg);

            this.outputService.SetOutputValue("valid-branch", logicResult.branchIsValid.ToString().ToLower());
        }
        catch (Exception e)
        {
            onError(e);
        }

        onCompleted(branchIsValid);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.branchNameObservable.Dispose();
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
