// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Exceptions;
using BranchValidator.Services.Interfaces;
using BranchValidatorShared;
using KDActionUtils;
using KDActionUtils.Services;

namespace BranchValidator;

/// <inheritdoc/>
public sealed class GitHubAction : IGitHubAction<bool>
{
    private readonly IConsoleService<ConsoleContext> consoleService;
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
        IConsoleService<ConsoleContext> consoleService,
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

        var branchNeedsTrimming = string.IsNullOrEmpty(inputs.TrimFromStart) is false &&
                                  inputs.BranchName.ToLower().StartsWith(inputs.TrimFromStart.ToLower());

        if (branchNeedsTrimming)
        {
            this.consoleService.WriteLine($"Branch Before Trimming: {inputs.BranchName}");

            inputs.BranchName = inputs.BranchName.TrimStart(inputs.TrimFromStart);

            this.consoleService.WriteLine($"The text '{inputs.TrimFromStart}' has been trimmed from the branch name.");
            this.consoleService.WriteLine($"Branch After Trimming: {inputs.BranchName}", false, true);
        }

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

            this.consoleService.Write("Validating expression . . . ");
            var validSyntaxResult = this.expressionValidatorService.Validate(inputs.ValidationLogic);

            if (inputs.FailWhenNotValid is true && validSyntaxResult.isValid is false)
            {
                var exceptionMsg = $"Invalid Syntax{Environment.NewLine}";
                exceptionMsg += $"\t{validSyntaxResult.msg}";

                this.consoleService.WriteLine("expression validation complete.");

                throw new InvalidSyntaxExpression(exceptionMsg)
                {
                    HResult = 500,
                };
            }

            this.consoleService.WriteLine("expression validation complete.", false, true);
            this.consoleService.Write("Executing expression . . . ");

            (bool branchIsValid, string msg) logicResult = this.expressionExecutorService.Execute(inputs.ValidationLogic, inputs.BranchName);
            branchIsValid = logicResult.branchIsValid;

            this.consoleService.WriteLine("expression execution complete.");

            if (inputs.FailWhenNotValid is true && logicResult.branchIsValid is false)
            {
                throw new InvalidBranchException($"Branch Invalid{Environment.NewLine}{Environment.NewLine}{logicResult.msg}");
            }

            this.consoleService.WriteLine($"Branch {(logicResult.branchIsValid ? "Valid" : "Invalid")}", true, false);
            this.consoleService.WriteLine(logicResult.msg, true, true);

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
        => this.consoleService.WriteLine("Welcome To The BranchValidator GitHub Action!!", true, true);
}
