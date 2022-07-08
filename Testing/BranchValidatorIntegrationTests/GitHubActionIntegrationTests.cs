// <copyright file="GitHubActionIntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Exceptions;
using BranchValidator.Observables;
using BranchValidator.Services;
using BranchValidator.Services.Analyzers;
using BranchValidator.Services.Interfaces;
using FluentAssertions;

namespace BranchValidatorIntegrationTests;

/// <summary>
/// Tests various classes integrated with each other.
/// </summary>
public class GitHubActionIntegrationTests : IDisposable
{
    private const string AndOperator = "&&";
    private const string OrOperator = "||";

    private readonly GitHubAction action;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubActionIntegrationTests"/> class.
    /// </summary>
    public GitHubActionIntegrationTests()
    {
        var consoleService = new GitHubConsoleService();
        var outputService = new ActionOutputService(consoleService);
        var funMethodNameExtractorService = new FunctionExtractorService();
        var methodNamesService = new CSharpMethodService();

        var parenAnalyzerService = new ParenAnalyzerService();
        var quoteAnalyzerService = new QuoteAnalyzerService();
        var operatorAnalyzerService = new OperatorAnalyzerService();
        var funAnalyzerService = new FunctionAnalyzerService(funMethodNameExtractorService, methodNamesService);
        var negativeNumberAnalyzerService = new NegativeNumberAnalyzer();

        var analyzers = new IAnalyzerService[]
        {
            parenAnalyzerService,
            quoteAnalyzerService,
            operatorAnalyzerService,
            funAnalyzerService,
            negativeNumberAnalyzerService,
        }.ToReadOnlyCollection();

        var expressionValidationService = new ExpressionValidatorService(analyzers);

        var resourceLoaderService = new TextResourceLoaderService();
        var csharpMethodService = new CSharpMethodService();
        var updateBranchNameObservable = new UpdateBranchNameObservable();
        var scriptService = new ScriptService<(bool result, string[] funcResults)>();
        var expressionExecutorService = new ExpressionExecutorService(
            methodNamesService,
            resourceLoaderService,
            scriptService);
        var parsingService = new ParsingService();

        this.action = new GitHubAction(
            consoleService,
            outputService,
            expressionValidationService,
            expressionExecutorService,
            csharpMethodService,
            parsingService,
            updateBranchNameObservable);
    }

    [Theory]
    [InlineData("equalTo('test-branch')", "test-branch", "")]
    [InlineData("equalTo('release/v#.#.#-preview.#')", "release/v1.20.300-preview.4000", "refs/heads/")]
    [InlineData("equalTo('feature/#-*')", "feature/123-test-branch", "refs/heads/")]
    [InlineData("equalTo('master')", "master", "refs/heads/")]
    [InlineData("equalTo('develop')", "develop", "refs/heads/")]
    [InlineData("equalTo('preview/feature/v#.#.#-preview.#')", "preview/feature/v1.20.300-preview.4000", "refs/heads/")]
    [InlineData("equalTo('hotfix/v#.#.#')", "hotfix/v1.2.3", "refs/heads/")]
    [InlineData("equalTo('release/v#.#.#')", "release/v1.2.3", "refs/heads/")]
    [InlineData("charIsNum(8)", "feature/123-test-branch", "")]
    [InlineData("charIsNum(8)", "refs/heads/feature/123-test-branch", "refs/heads/")]
    [InlineData("contains('123-test')", "feature/123-test-branch", "")]
    [InlineData("notContains('not-contained')", "feature/123-test-branch", "")]
    [InlineData("existsTotal('123-test', 1)", "feature/123-test-branch", "")]
    [InlineData("existsTotal('test-branch', 2)", "feature/123-test-branch-test-branch-test", "")]
    [InlineData("allUpperCase()", "FEATURE/123-TEST-BRANCH", "")]
    [InlineData("allLowerCase()", "feature/123-test-branch", "")]
    [InlineData("contains('#-test')", "feature/123-test-branch", "")]
    [InlineData("contains('123-*-branch')", "feature/123-test-branch", "")]
    [InlineData("contains('release/v#.#.#-preview.#')", "refs/heads/release/v1.20.300-preview.4000", "refs/heads/")]
    public async void Run_WithValidBranches_ReturnsCorrectResult(string expression, string branchName, string trimFromStart)
    {
        // Arrange
        bool? branchIsValid = null;
        var actionInputs = new ActionInputs
        {
            BranchName = branchName,
            ValidationLogic = expression,
            TrimFromStart = trimFromStart,
            FailWhenNotValid = true,
        };

        // Act & Assert
        var act = () => this.action.Run(
            actionInputs,
            result =>
            {
                branchIsValid = result;
            }, e => throw e);

        // Assert
        await act.Should().NotThrowAsync();
        branchIsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("charIsNum(-8)", "Negative number argument values not aloud.")]
    [InlineData("charIsNum8)", "The expression is missing a '('.")]
    [InlineData("charIsNum(8", "The expression is missing a ')'.")]
    [InlineData("charIsNum((8)", "The expression is missing a ')'.")]
    [InlineData("charIsNum(8))", "The expression is missing a '('.")]
    [InlineData("(8)", "The expression cannot start with a '(' or ')' parenthesis.")]
    [InlineData("charIsNum()", "The expression function is missing an argument.")]
    [InlineData("equalTo('feature/123-*-branch'", "The expression is missing a ')'.")]
    [InlineData("equalTo('feature/#-test-branch'", "The expression is missing a ')'.")]
    public async void Run_WithExpressionSyntaxErrors_ReturnsCorrectInvalidResult(
        string expression,
        string expectedAnalyzerMsg)
    {
        // Arrange
        var expectedMsg = $"Invalid Syntax{Environment.NewLine}\t{expectedAnalyzerMsg}";
        var actionInputs = new ActionInputs
        {
            BranchName = "test-branch",
            ValidationLogic = expression,
            FailWhenNotValid = true,
        };

        // Act & Assert
        var act = () => this.action.Run(actionInputs, _ => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage(expectedMsg);
    }

    [Theory]
    [InlineData("equalTo('not-equal-branch')", "test-branch", "equalTo(string)")]
    [InlineData("equalTo('release/v#.#.#-preview.#')", "release/v1.20.test-preview.4000", "equalTo(string)")]
    [InlineData("equalTo('feature/#-*')", "feature/word-test-branch", "equalTo(string)")]
    [InlineData("charIsNum(4)", "feature/123-test-branch", "charIsNum(number)")]
    [InlineData("contains('not-contained')", "feature/123-test-branch", "contains(string)")]
    [InlineData("notContains('123-test')", "feature/123-test-branch", "notContains(string)")]
    [InlineData("existsTotal('456-test', 1)", "feature/123-test-branch", "existsTotal(string, number)")]
    [InlineData("allUpperCase()", "feature/123-test-branch", "allUpperCase()")]
    [InlineData("allLowerCase()", "FEATURE/123-TEST-BRANCH", "allLowerCase()")]
    [InlineData("equalTo('feature/123-#-branch')", "feature/123-test-branch", "equalTo(string)")]
    [InlineData("equalTo('feature/456-*-branch')", "feature/123-test-branch", "equalTo(string)")]
    [InlineData("equalTo('feature/*-#-branch')", "feature/123-test-branch", "equalTo(string)")]
    public async void Run_WithInvalidBranches_FailsActionWithException(
        string expression,
        string branchName,
        string funcSignature)
    {
        // Arrange
        var expectedMsg = $"Branch Invalid{Environment.NewLine}{Environment.NewLine}Function Results:{Environment.NewLine}\t{funcSignature} -> false";
        var actionInputs = new ActionInputs
        {
            BranchName = branchName,
            ValidationLogic = expression,
            FailWhenNotValid = true,
        };

        // Act & Assert
        var act = () => this.action.Run(actionInputs, _ => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<InvalidBranchException>()
            .WithMessage(expectedMsg);
    }

    [Theory]
    [InlineData("contains('-') && notContains('preview')", "feature/123-test-branch")]
    public async void Run_WithValidBranchesAndOperators_ReturnsCorrectResult(string expression, string branchName)
    {
        CheckForOps(expression);
        // Arrange
        bool? branchIsValid = null;
        var actionInputs = new ActionInputs
        {
            BranchName = branchName,
            ValidationLogic = expression,
            FailWhenNotValid = true,
        };

        // Act & Assert
        var act = () => this.action.Run(
            actionInputs,
            result =>
            {
                branchIsValid = result;
            }, e => throw e);

        // Assert
        await act.Should().NotThrowAsync();
        branchIsValid.Should().BeTrue();
    }

    /// <summary>
    /// Disposes of the action.
    /// </summary>
    public void Dispose() => this.action.Dispose();

    /// <summary>
    /// Asserts that the given <c>string</c> <paramref name="value"/> contains operators for testing to work properly.
    /// </summary>
    /// <param name="value">The value to check.</param>
    private static void CheckForOps(string value)
        => (value.Contains(AndOperator) || value.Contains(OrOperator))
        .Should()
        .BeTrue("this unit test requires operators to exist in the expression for proper testing.");
}
