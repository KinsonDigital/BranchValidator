// <copyright file="ExecutorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using BranchValidatorTests.Helpers;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

public class ExecutorServiceTests
{
    private readonly Mock<IExpressionValidatorService> mockValidationService;
    private readonly Mock<IFunctionService> mockFunctionService;

    public ExecutorServiceTests()
    {
        this.mockValidationService = new Mock<IExpressionValidatorService>();
        this.mockValidationService.Setup(m => m.Validate(It.IsAny<string>()))
            .Returns((true, "expression valid"));

        this.mockFunctionService = new Mock<IFunctionService>();
    }

    #region Method Tests
    [Fact]
    public void Execute_WithInvalidExpression_ReturnsCorrectResult()
    {
        // Arrange
        this.mockValidationService.Setup(m => m.Validate("test-expression"))
            .Returns((false, "expression invalid"));

        var service = CreateService();

        // Act
        var actual = service.Execute("test-expression", "test-branch");

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("expression invalid");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Execute_WithNullOrEmptyExpression_ReturnsCorrectResult(string expression)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Execute(expression, It.IsAny<string>());

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The expression must not be null or empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Execute_WithNullOrEmptyBranchName_ReturnsCorrectResult(string branchName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Execute("test-expression", branchName);

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The branch name must not be null or empty.");
    }

    [Theory]
    [InlineData("equalTo('feature/123-my-branch')", "equalTo", "'feature/123-my-branch'", "feature/123-my-branch", true, true, "branch valid")]
    [InlineData(" equalTo('feature/123-my-branch')", "equalTo", "'feature/123-my-branch'", "feature/123-my-branch", true, true, "branch valid")]
    [InlineData("equalTo('feature/123-my-branch') ", "equalTo", "'feature/123-my-branch'", "feature/123-my-branch", true, true, "branch valid")]
    [InlineData("isCharNum(8)", "isCharNum", "8", "feature/123-my-branch", true, true, "branch valid")]
    public void Execute_WithNoOperators_ReturnsCorrectResult(
        string expression,
        string funcName,
        string paramStr, // Comma delimited list
        string branchName,
        bool expectedFuncValid,
        bool expectedExecutionValid,
        string expectedMsg)
    {
        // Arrange
        var argValues = new List<string>();
        argValues.AddRange(paramStr.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
        argValues.Add($"'{branchName}'");

        this.mockFunctionService.Setup(m => m.Execute(funcName, argValues.ToArray()))
            .Returns((expectedFuncValid, expectedMsg));
        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        actual.valid.Should().Be(expectedExecutionValid);
    }

    [Fact]
    public void Execute_WithSingleFunctionAndNoOperators_ReturnsCorrectResult()
    {
        // Arrange
        const string branchName = "test-branch";
        const string funcName = "funcA";
        const string expression = $"{funcName}('{branchName}')";
        const bool expectedFuncValid = true;
        var expectedMsg = $"The function '{funcName}' returned a value of '{expectedFuncValid.ToString().ToLower()}'.";

        this.mockFunctionService.Setup(m => m.Execute(funcName, It.IsAny<string[]>()))
            .Returns((expectedFuncValid, expectedMsg));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        actual.valid.Should().BeTrue();
        actual.msg.Should().Be(expectedMsg);
    }

    [Fact]
    public void Execute_WithOnlyOrOperatorsAnd2Functions_ReturnsCorrectResult()
    {
        // Arrange
        const string expression = "funA('is-2-branch') || funB(3)";
        const string branchName = "is-2-branch";
        const bool expectedFuncValid = true;
        const bool expectedExecutionValid = true;
        const string expectedBranchParamValue = $"'{branchName}'";
        const string expectedMsg = "branch valid";

        this.mockFunctionService.Setup(m => m.Execute("funA", It.IsAny<string[]>()))
            .Returns((expectedFuncValid, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funB", It.IsAny<string[]>()))
            .Returns((expectedFuncValid, expectedMsg));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        this.mockFunctionService.VerifyOnce(m => m.Execute("funA", "'is-2-branch'", expectedBranchParamValue));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funB", "3", expectedBranchParamValue));
        actual.valid.Should().Be(expectedExecutionValid);
        actual.msg.Should().Be(expectedMsg);
    }

    [Theory]
    [InlineData(true, false, true, false, true, true, "branch valid")]
    [InlineData(true, false, false, false, false, false, "branch invalid")]
    [InlineData(true, false, false, true, false, false, "branch invalid")]
    public void Execute_WithBothOperatorTypesAnd2Functions_ReturnsCorrectResult(
        bool funcAValidResult,
        bool funcBValidResult,
        bool funcCValidResult,
        bool funcDValidResult,
        bool funcEValidResult,
        bool expectedExecuteResult,
        string expectedExecuteMsg)
    {
        // Arrange
        const string expression = "funA('value-A') && funB(10) || funC('value-C') || funD(20) && funE('value-E')";
        const string branchName = "test-branch";
        const string expectedMsg = "branch valid";
        const string expectedBranchParamValue = $"'{branchName}'";

        this.mockFunctionService.Setup(m => m.Execute("funA", It.IsAny<string[]>()))
            .Returns((funcAValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funB", It.IsAny<string[]>()))
            .Returns((funcBValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funC", It.IsAny<string[]>()))
            .Returns((funcCValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funD", It.IsAny<string[]>()))
            .Returns((funcDValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funE", It.IsAny<string[]>()))
            .Returns((funcEValidResult, expectedMsg));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        this.mockFunctionService.VerifyOnce(m => m.Execute("funA", "'value-A'", expectedBranchParamValue));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funB", "10", expectedBranchParamValue));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funC", "'value-C'", expectedBranchParamValue));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funD", "20", expectedBranchParamValue));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funE", "'value-E'", expectedBranchParamValue));
        actual.valid.Should().Be(expectedExecuteResult);
        actual.msg.Should().Be(expectedExecuteMsg);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="ExpressionExecutorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ExpressionExecutorService CreateService() => new (this.mockValidationService.Object, this.mockFunctionService.Object);
}
