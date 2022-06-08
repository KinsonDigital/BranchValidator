// <copyright file="IntegrationTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Factories;
using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.IntegrationTests;

/// <summary>
/// Tests various classes integrated with each other.
/// </summary>
public class IntegrationTests
{
    private readonly ExpressionExecutorService expressionExecutorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTests"/> class.
    /// </summary>
    public IntegrationTests()
    {
        var analyzerFactory = new AnalyzerFactory();
        var expressionValidationService = new ExpressionValidatorService(analyzerFactory);
        var methodExecutor = new MethodExecutor();
        var functionService = new FunctionService(methodExecutor);

        this.expressionExecutorService = new ExpressionExecutorService(expressionValidationService, functionService);
    }

    [Theory]
    [InlineData("equalTo('my-branch')", "my-branch", true, "The function 'equalTo' returned a value of 'true'.")]
    [InlineData("equalTo('not-equal-to-this')", "feature/123-my-branch", false, "The function 'equalTo' returned a value of 'false'.")]
    [InlineData("isCharNum(4)", "feature/123-my-branch", false, "The function 'isCharNum' returned a value of 'false'.")]
    [InlineData("isCharNum(8)", "feature/123-my-branch", true, "The function 'isCharNum' returned a value of 'true'.")]
    public void Execute_WhenInvoked_ReturnsCorrectResult(
        string expression,
        string branchName,
        bool expectedValidResult,
        string expectedMsgResult)
    {
        // Act
        var actual = this.expressionExecutorService.Execute(expression, branchName);

        // Assert
        actual.valid.Should().Be(expectedValidResult);
        actual.msg.Should().Be(expectedMsgResult);
    }
}
