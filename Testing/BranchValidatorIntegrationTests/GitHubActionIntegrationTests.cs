// <copyright file="GitHubActionIntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Factories;
using BranchValidator.Observables;
using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorIntegrationTests;

/// <summary>
/// Tests various classes integrated with each other.
/// </summary>
public class GitHubActionIntegrationTests : IDisposable
{
    private readonly GitHubAction action;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubActionIntegrationTests"/> class.
    /// </summary>
    public GitHubActionIntegrationTests()
    {
        var analyzerFactory = new AnalyzerFactory();
        var expressionValidationService = new ExpressionValidatorService(analyzerFactory);
        var methodExecutor = new MethodExecutor();
        var jsonService = new JSONService();
        var resourceLoaderService = new TextResourceLoaderService();
        var updateBranchNameObservable = new UpdateBranchNameObservable();
        var functionDefinitions = new FunctionDefinitions(updateBranchNameObservable);
        var functionService = new FunctionService(jsonService, resourceLoaderService, methodExecutor, functionDefinitions);
        var consoleService = new GitHubConsoleService();
        var outputService = new ActionOutputService(consoleService);
        var expressionExecutorService = new ExpressionExecutorService(expressionValidationService, functionService);

        this.action = new GitHubAction(consoleService, outputService, expressionExecutorService, functionService, updateBranchNameObservable);
    }

    [Theory]
    [InlineData("equalTo('test-branch')", "test-branch")]
    [InlineData("isCharNum(8)", "feature/123-test-branch")]
    [InlineData("isSectionNum(8, 10)", "feature/123-test-branch")]
    [InlineData("isSectionNum(8, '-')", "feature/123-test-branch")]
    [InlineData("contains('123-test')", "feature/123-test-branch")]
    public async void Execute_WithValidBranches_ReturnsCorrectResult(string expression, string branchName)
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
    [InlineData("equalTo('not-equal-branch')", "equalTo", "test-branch")]
    [InlineData("isCharNum(4)", "isCharNum", "feature/123-test-branch")]
    [InlineData("isCharNum(-8)", "isCharNum", "feature/123-test-branch")]
    [InlineData("isSectionNum(4, 8)", "isSectionNum", "feature/123-test-branch")]
    [InlineData("contains('not-contained')", "contains", "feature/123-test-branch")]
    public async void Execute_WithInvalidBranches_FailsActionWithException(
        string expression,
        string funcName,
        string branchName)
    {
        // Arrange
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
            .WithMessage($"The function '{funcName}' returned a value of 'false'.");
    }

    /// <summary>
    /// Disposes of the action.
    /// </summary>
    public void Dispose() => this.action.Dispose();
}
