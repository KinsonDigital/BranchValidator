// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using BranchValidator.Services.Interfaces;
using BranchValidatorShared;
using BranchValidatorShared.Services;

namespace BranchValidator;

/// <inheritdoc/>
public sealed class GitHubAction : IGitHubAction<bool>
{
    private readonly IConsoleService consoleService;
    private readonly IActionOutputService actionOutputService;
    private readonly IExpressionValidatorService expressionValidatorService;
    private readonly IExpressionExecutorService expressionExecutorService;
    private readonly ICSharpMethodService csharpMethodService;
    private readonly IParsingService parsingService;
    private readonly IBranchNameObservable branchNameObservable;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAction"/> class.
    /// </summary>
    /// <param name="consoleService">Prints messages to the GitHub console.</param>
    /// <param name="actionOutputService">Sets the GitHub action outputs.</param>
    /// <param name="expressionValidatorService">Validates expression syntax.</param>
    /// <param name="expressionExecutorService">Executes expressions.</param>
    /// <param name="csharpMethodService">Provides data about <c>C#</c> methods.</param>
    /// <param name="parsingService">Provides parsing functionality.</param>
    /// <param name="branchNameObservable">Sends a push notification of the branch name.</param>
    public GitHubAction(
        IConsoleService consoleService,
        IActionOutputService actionOutputService,
        IExpressionValidatorService expressionValidatorService,
        IExpressionExecutorService expressionExecutorService,
        ICSharpMethodService csharpMethodService,
        IParsingService parsingService,
        IBranchNameObservable branchNameObservable)
    {
        EnsureThat.ParamIsNotNull(consoleService);
        EnsureThat.ParamIsNotNull(actionOutputService);
        EnsureThat.ParamIsNotNull(expressionValidatorService);
        EnsureThat.ParamIsNotNull(expressionExecutorService);
        EnsureThat.ParamIsNotNull(csharpMethodService);
        EnsureThat.ParamIsNotNull(parsingService);
        EnsureThat.ParamIsNotNull(branchNameObservable);

        this.consoleService = consoleService;
        this.actionOutputService = actionOutputService;
        this.expressionValidatorService = expressionValidatorService;
        this.expressionExecutorService = expressionExecutorService;
        this.csharpMethodService = csharpMethodService;
        this.parsingService = parsingService;
        this.branchNameObservable = branchNameObservable;
    }

    /// <inheritdoc/>
#pragma warning disable CS1998
    public async Task Run(ActionInputs inputs, Action<bool> onCompleted, Action<Exception> onError)
#pragma warning restore CS1998
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
        this.consoleService.BlankLine();

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

            this.consoleService.WriteLine("Validating expression . . .");
            var validSyntaxResult = this.expressionValidatorService.Validate(inputs.ValidationLogic);

            if (validSyntaxResult.isValid is false)
            {
                var exceptionMsg = $"Invalid Syntax:{Environment.NewLine}";
                exceptionMsg += $"\t{validSyntaxResult.msg}";

                throw new Exception(exceptionMsg)
                {
                    HResult = 500,
                };
            }

            this.consoleService.WriteLine("Expression validation complete.");

            this.consoleService.BlankLine();

            this.consoleService.WriteLine("Executing expression . . .");

            (bool branchIsValid, string msg) logicResult = this.expressionExecutorService.Execute(inputs.ValidationLogic, inputs.BranchName);
            branchIsValid = logicResult.branchIsValid;

            this.consoleService.WriteLine("Expression execution complete.");

            if (inputs.FailWhenNotValid is true && logicResult.branchIsValid is false)
            {
                throw new Exception(logicResult.msg)
                {
                    HResult = 600,
                };
            }

            this.consoleService.BlankLine();

            var executionResult = logicResult.branchIsValid
                ? "Branch Valid"
                : $"Branch Invalid: {logicResult.msg}";

            this.consoleService.WriteLine(executionResult);

            this.consoleService.BlankLine();
            this.consoleService.WriteLine(logicResult.msg);

            this.actionOutputService.SetOutputValue("valid-branch", logicResult.branchIsValid.ToString().ToLower());
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
        this.consoleService.BlankLine();
        this.consoleService.WriteLine("Welcome To The BranchValidator GitHub Action!!");
        this.consoleService.BlankLine();
    }
}
