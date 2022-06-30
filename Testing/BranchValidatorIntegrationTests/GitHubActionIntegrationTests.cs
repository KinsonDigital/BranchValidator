// <copyright file="GitHubActionIntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
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
    [InlineData("equalTo('test-branch')", "test-branch")]
    [InlineData("isCharNum(8)", "feature/123-test-branch")]
    [InlineData("isSectionNum(8, 10)", "feature/123-test-branch")]
    [InlineData("isSectionNum(8, '-')", "feature/123-test-branch")]
    [InlineData("contains('123-test')", "feature/123-test-branch")]
    [InlineData("notContains('not-contained')", "feature/123-test-branch")]
    [InlineData("existTotal('123-test', 1)", "feature/123-test-branch")]
    [InlineData("existTotal('test-branch', 2)", "feature/123-test-branch-test-branch-test")]
    [InlineData("existsLessThan('123-', 2)", "feature/123-test-branch")]
    [InlineData("existsGreaterThan('test', 2)", "feature/test-123-test-123-test-branch")]
    [InlineData("startsWith('feature/123')", "feature/123-test-branch")]
    [InlineData("notStartsWith('123')", "feature/123-test-branch")]
    [InlineData("endsWith('test-branch')", "feature/123-test-branch")]
    [InlineData("notEndsWith('123')", "feature/123-test-branch")]
    [InlineData("startsWithNum()", "123-test-branch")]
    [InlineData("endsWithNum()", "feature/123-test-branch-456")]
    [InlineData("lenLessThan(200)", "feature/123-test-branch")]
    [InlineData("isBefore('123', 'branch')", "feature/123-test-branch")]
    [InlineData("isAfter('test', 'feature')", "feature/123-test-branch")]
    public async void Run_WithValidBranches_ReturnsCorrectResult(string expression, string branchName)
    {
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

    [Theory]
    [InlineData("isCharNum(-8)", "Negative number argument values not aloud.")]
    [InlineData("isCharNum8)", "The expression is missing a '('.")]
    [InlineData("isCharNum(8", "The expression is missing a ')'.")]
    [InlineData("isCharNum((8)", "The expression is missing a ')'.")]
    [InlineData("isCharNum(8))", "The expression is missing a '('.")]
    [InlineData("(8)", "The expression cannot start with a '(' or ')' parenthesis.")]
    [InlineData("isCharNum()", "The expression function is missing an argument.")] // Missing a parameter
    public async void Run_WithExpressionSyntaxErrors_ReturnsCorrectInvalidResult(
        string expression,
        string expectedAnalyzerMsg)
    {
        // Arrange
        var expectedMsg = $"Invalid Syntax:{Environment.NewLine}\t{expectedAnalyzerMsg}";
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
    [InlineData("isCharNum(4)", "feature/123-test-branch", "isCharNum(number)")]
    [InlineData("isSectionNum(4, 8)", "feature/123-test-branch", "isSectionNum(number, number)")]
    [InlineData("contains('not-contained')", "feature/123-test-branch", "contains(string)")]
    [InlineData("notContains('123-test')", "feature/123-test-branch", "notContains(string)")]
    [InlineData("existTotal('456-test', 1)", "feature/123-test-branch", "existTotal(string, number)")]
    [InlineData("existsLessThan('123-test', 2)", "feature/123-test-123-test-branch", "existsLessThan(string, number)")]
    [InlineData("existsGreaterThan('test', 2)", "feature/123-test-123-test-branch", "existsGreaterThan(string, number)")]
    [InlineData("startsWith('123')", "feature/123-test-branch", "startsWith(string)")]
    [InlineData("notStartsWith('feature/123')", "feature/123-test-branch", "notStartsWith(string)")]
    [InlineData("endsWith('123')", "feature/123-test-branch", "endsWith(string)")]
    [InlineData("notEndsWith('test-branch')", "feature/123-test-branch", "notEndsWith(string)")]
    [InlineData("startsWithNum()", "feature/123-test-branch", "startsWithNum()")]
    [InlineData("endsWithNum()", "feature/123-test-branch", "endsWithNum()")]
    [InlineData("lenLessThan(10)", "feature/123-test-branch", "lenLessThan(number)")]
    [InlineData("isBefore('branch', '123')", "feature/123-test-branch", "isBefore(string, string)")]
    [InlineData("isAfter('feature', 'test')", "feature/123-test-branch", "isAfter(string, string)")]
    public async void Run_WithInvalidBranches_FailsActionWithException(
        string expression,
        string branchName,
        string funcSignature)
    {
        // Arrange
        var expectedMsg = $"Function Results:{Environment.NewLine}\t{funcSignature} -> False";
        var actionInputs = new ActionInputs
        {
            BranchName = branchName,
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
    [InlineData("contains('-') && isSectionNum(8, '-')", "feature/123-test-branch")]
    [InlineData("startsWith('feature/') || startsWith('preview')", "feature/123-branch")]
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
    private void CheckForOps(string value)
    {
        (value.Contains(AndOperator) || value.Contains(OrOperator))
            .Should()
            .BeTrue("this unit test requires operators to exist in the expression for proper testing.");
    }
}
